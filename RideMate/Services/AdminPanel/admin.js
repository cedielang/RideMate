// --- FILE: admin.js ---
// This file contains the security and presentation logic for the RideMate Admin Panel.
// It relies on Firebase SDK (app, auth, firestore) being loaded in the HTML.

let auth;
let db;
let allUsers = []; // Stores the master list of all users fetched from the server
let allRides = []; // Stores the master list of all rides fetched from the server
let allPayments = []; // Stores the master list of all payments fetched from the server
let currentAdminUID = 'N/A';
let currentActiveTab = 'dashboard';

// --- INITIALIZATION ---
window.onload = function () {
    try {
        // Retrieve configuration variables embedded in the HTML file
        const firebaseConfig = JSON.parse(__firebase_config);

        // Initialize Firebase using compat SDK
        const app = firebase.initializeApp(firebaseConfig);
        auth = firebase.auth(app);
        db = firebase.firestore(app);

        initializeAuth();

    } catch (e) {
        console.error("Firebase Initialization Error:", e);
        document.getElementById('adminEmail').textContent = "ERROR: Failed to load Firebase config.";
    }
};

async function initializeAuth() {
    const initialAuthToken = __initial_auth_token;

    try {
        if (initialAuthToken) {
            // Sign in using the secure custom token (Admin authentication)
            const userCredential = await auth.signInWithCustomToken(initialAuthToken);
            currentAdminUID = userCredential.user.uid;
            document.getElementById('adminEmail').textContent = userCredential.user.email || `Admin UID: ${currentAdminUID.substring(0, 8)}...`;
            loadAllData(true); // Load data immediately after successful sign-in
        } else {
            // Fallback: Use anonymous sign-in if token is missing
            await auth.signInAnonymously();
            currentAdminUID = auth.currentUser.uid;
            document.getElementById('adminEmail').textContent = `Anon UID: ${currentAdminUID.substring(0, 8)}... (No Privileges)`;
            loadAllData(false); // Attempt to load public data
        }
    } catch (error) {
        console.error("Admin Authentication Error:", error);
        document.getElementById('adminEmail').textContent = `Auth FAILED (${error.code})`;
    }
}

// --- SECURE API CALLS (To Cloud Functions) ---
// This function verifies the admin's identity before executing privileged actions.

async function secureFetch(endpoint, method = 'GET', body = null) {
    if (!auth.currentUser || auth.currentUser.isAnonymous) {
        console.warn("Attempted privileged call while anonymous. Blocked.");
        throw new Error("Authentication required for privileged action.");
    }

    // Get the current admin user's ID token for server verification
    const idToken = await auth.currentUser.getIdToken();

    const options = {
        method: method,
        headers: {
            'Authorization': `Bearer ${idToken}`,
            'Content-Type': 'application/json'
        },
        body: body ? JSON.stringify(body) : null
    };

    const response = await fetch(`${FUNCTIONS_BASE_URL}/${endpoint}`, options);

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({ message: response.statusText }));
        throw new Error(errorData.message || `API Error: ${response.status}`);
    }

    return response.json();
}

// --- DATA LOADING ---

window.loadAllData = async function (isAdmin) {
    // Show loading spinners for all content
    document.getElementById('dashboardLoading').style.display = 'block';
    document.getElementById('driversLoading').style.display = 'block';
    document.getElementById('passengersLoading').style.display = 'block';
    document.getElementById('ridesLoading').style.display = 'block';
    document.getElementById('paymentsLoading').style.display = 'block';

    try {
        // 1. Fetch Users and Profiles (REQUIRES ADMIN PRIVILEGE VIA CLOUD FUNCTION)
        if (isAdmin) {
            const userData = await secureFetch('listUserProfiles');
            allUsers = userData.users || [];
        } else {
            // If not admin, attempt to fetch user count only via non-privileged means if necessary
            allUsers = [];
        }

        // 2. Fetch Rides (Last 50) - Read-only client query
        const ridesSnapshot = await db.collection('rides').orderBy('RequestTime', 'desc').limit(50).get();
        allRides = ridesSnapshot.docs.map(doc => ({ id: doc.id, ...doc.data(), RequestTime: doc.data().RequestTime?.toDate() }));

        // 3. Fetch Payments (Last 50) - Read-only client query
        const paymentsSnapshot = await db.collection('payments').orderBy('Timestamp', 'desc').limit(50).get();
        allPayments = paymentsSnapshot.docs.map(doc => ({ id: doc.id, ...doc.data(), Timestamp: doc.data().Timestamp?.toDate() }));

        // Update UI
        updateDashboardStats();
        renderUserTables(allUsers, 'driver', true);
        renderUserTables(allUsers, 'passenger', true);
        renderRideTables();
        renderPaymentTables();

    } catch (error) {
        console.error("Failed to load data:", error);
        if (isAdmin) alert(`Error loading data. Ensure Cloud Functions are deployed and URL is correct. Details: ${error.message}`);

    } finally {
        // Hide all loading spinners and show tables
        document.getElementById('dashboardLoading').style.display = 'none';
        document.getElementById('driversLoading').style.display = 'none';
        document.getElementById('passengersLoading').style.display = 'none';
        document.getElementById('ridesLoading').style.display = 'none';
        document.getElementById('paymentsLoading').style.display = 'none';
        document.getElementById('driversTable').style.display = 'block';
        document.getElementById('passengersTable').style.display = 'block';
        document.getElementById('ridesTable').style.display = 'block';
        document.getElementById('paymentsTable').style.display = 'block';
    }
}

// --- UI RENDERING ---

function updateDashboardStats() {
    const totalUsers = allUsers.length;
    const totalDrivers = allUsers.filter(u => u.role === 'driver').length;
    const blockedUsers = allUsers.filter(u => u.isBlocked).length;
    const activeRides = allRides.filter(r => r.Status === 'Requested' || r.Status === 'Accepted').length;

    document.getElementById('totalUsersCount').textContent = totalUsers;
    document.getElementById('totalDriversCount').textContent = totalDrivers;
    document.getElementById('blockedUsersCount').textContent = blockedUsers;
    document.getElementById('totalRidesCount').textContent = activeRides;
}

function renderUserTables(users, targetRole, skipFilter = false) {
    const filteredUsers = users.filter(u => u.role === targetRole);
    const tableBodyId = targetRole === 'driver' ? 'driversTableBody' : 'passengersTableBody';
    const tableBody = document.getElementById(tableBodyId);

    // If skipFilter is false, apply filtering based on search inputs
    const usersToRender = skipFilter ? filteredUsers : applyFilter(filteredUsers, targetRole);

    tableBody.innerHTML = usersToRender.map(user => {
        const isDriver = user.role === 'driver';
        const isBlocked = user.isBlocked;
        const displayName = user.name || user.email?.split('@')[0] || user.uid.substring(0, 8);
        const phone = user.phone || user.phoneNumber || 'N/A';

        // Block/Unblock buttons only for drivers, if user is authenticated (not anonymous)
        const canBlock = auth.currentUser && !auth.currentUser.isAnonymous;

        const buttonHtml = canBlock && isDriver ?
            `<button class="btn btn-view" onclick="openModal('${user.uid}', '${user.role}')">View</button>
             <button class="btn ${isBlocked ? 'btn-unblock' : 'btn-block'}" onclick="toggleBlockStatus('${user.uid}', ${!isBlocked})">${isBlocked ? 'Unblock' : 'Block'}</button>`
            : `<button class="btn btn-view" onclick="openModal('${user.uid}', '${user.role}')">View</button>`;

        const mainFields = isDriver ?
            `<td>${displayName}</td>
             <td>${phone}</td>
             <td>${user.vehicleModel || 'N/A'} (${user.plateNumber || 'N/A'})</td>`
            :
            `<td>${displayName}</td>
             <td>${phone}</td>
             <td>${user.email || 'N/A'}</td>`;

        return `
            <tr>
                ${mainFields}
                <td><span class="badge ${isBlocked ? 'blocked' : 'active'}">${isBlocked ? 'BLOCKED' : 'ACTIVE'}</span></td>
                <td><div class="action-buttons">${buttonHtml}</div></td>
            </tr>
        `;
    }).join('');
}

function renderRideTables() {
    const rideBody = document.getElementById('ridesTableBody');
    rideBody.innerHTML = allRides.map(ride => {
        const fare = ride.Fare ? `₱${ride.Fare.toFixed(2)}` : 'N/A';
        const date = ride.RequestTime ? ride.RequestTime.toLocaleDateString() : 'N/A';
        return `
            <tr>
                <td>${ride.id.substring(0, 6)}...</td>
                <td>${ride.PassengerName || 'N/A'}</td>
                <td>${ride.DriverName || 'Unassigned'}</td>
                <td>${ride.DestinationAddress || 'N/A'}</td>
                <td>${fare}</td>
                <td><span class="badge ${ride.Status === 'Completed' ? 'active' : 'blocked'}">${ride.Status}</span></td>
                <td>${date}</td>
            </tr>
        `;
    }).join('');
}

function renderPaymentTables() {
    const paymentBody = document.getElementById('paymentsTableBody');
    paymentBody.innerHTML = allPayments.map(payment => {
        const amount = payment.Amount ? `₱${payment.Amount.toFixed(2)}` : 'N/A';
        const date = payment.Timestamp ? payment.Timestamp.toLocaleDateString() : 'N/A';

        return `
            <tr>
                <td>${payment.RideId?.substring(0, 6) || 'N/A'}...</td>
                <td>${amount}</td>
                <td>${payment.Method || 'N/A'}</td>
                <td>${date}</td>
            </tr>
        `;
    }).join('');
}


// --- FILTERING LOGIC ---

function applyFilter(userList, role) {
    const searchId = role === 'driver' ? 'driverSearch' : 'passengerSearch';
    const statusId = role === 'driver' ? 'driverStatusFilter' : 'passengerStatusFilter';

    const query = document.getElementById(searchId)?.value?.toLowerCase() || '';
    const status = document.getElementById(statusId)?.value || 'all';

    return userList.filter(user => {
        const matchesStatus = status === 'all' ||
            (status === 'blocked' && user.isBlocked) ||
            (status === 'active' && !user.isBlocked);

        const matchesQuery = query === '' ||
            (user.name && user.name.toLowerCase().includes(query)) ||
            (user.phone && user.phone.includes(query)) ||
            (user.email && user.email.toLowerCase().includes(query));

        return matchesStatus && matchesQuery;
    });
}

window.filterUsers = function () {
    if (allUsers.length === 0) return;
    renderUserTables(allUsers, currentActiveTab === 'drivers' ? 'driver' : 'passenger', false);
}


// --- DRIVER ACTION (Block/Unblock) ---

window.toggleBlockStatus = async function (userId, newBlockStatus) {
    if (!confirm(`Are you sure you want to ${newBlockStatus ? 'BLOCK' : 'UNBLOCK'} this user?`)) {
        return;
    }

    try {
        // Call the secure Cloud Function to update the status
        const result = await secureFetch('updateDriverStatus', 'POST', {
            userId: userId,
            isBlocked: newBlockStatus
        });

        alert(`Success: User ${newBlockStatus ? 'BLOCKED' : 'UNBLOCKED'}. Refreshing data...`);

        // Refresh data to update the table immediately
        loadAllData(true);

    } catch (error) {
        console.error("Block/Unblock Error:", error);
        alert(`Failed to update status. Error: ${error.message}`);
    }
}

// --- NEW FUNCTION: Save User Details (Placeholder for future Cloud Function) ---
window.saveUserDetails = async function (userId, role) {
    const name = document.getElementById('modalUserName')?.value;
    const email = document.getElementById('modalUserEmail')?.value;

    // Placeholder for API call to save details
    console.log(`Attempting to save details for ${role} ${userId}: Name=${name}, Email=${email}`);

    // In a real application, you would call a new Cloud Function here:
    // try {
    //     await secureFetch('updateUserProfile', 'POST', { userId, name, email });
    //     alert("User details updated successfully!");
    //     loadAllData(true); // Refresh global data
    // } catch (error) {
    //     alert("Failed to save details: " + error.message);
    // }

    alert("Profile update functionality is secured behind a pending Cloud Function. Changes logged to console.");
    closeModal();
};


// --- MODAL & TABS ---

window.openModal = function (uid, role) {
    const user = allUsers.find(u => u.uid === uid);
    if (!user) return;

    const initial = (user.name || user.email || 'U')[0].toUpperCase();
    const avatarColor = role === 'driver' ? '#2196F3' : '#FF9800'; // Blue for Driver, Orange for Passenger
    const displayName = user.name || 'N/A';
    const displayEmail = user.email || 'N/A';

    document.getElementById('modalTitle').textContent = `${role.toUpperCase()} Details: ${displayName}`;

    let bodyHtml = `
        <div style="text-align: center; margin-bottom: 25px;">
            <div style="display: inline-block; width: 70px; height: 70px; line-height: 70px; border-radius: 50%; background: ${avatarColor}; color: white; font-size: 32px; font-weight: bold; box-shadow: 0 2px 5px rgba(0,0,0,0.2);">
                ${initial}
            </div>
        </div>

        <h3 style="margin-top: 15px; border-top: 1px solid #ddd; padding-top: 15px;">Edit Profile</h3>
        <div class="info-row">
            <span class="info-label">Full Name</span>
            <input type="text" id="modalUserName" class="info-value" value="${displayName}" style="text-align: right; border: 1px solid #ddd; padding: 5px; border-radius: 3px; max-width: 60%;"/>
        </div>
        <div class="info-row">
            <span class="info-label">Email</span>
            <input type="email" id="modalUserEmail" class="info-value" value="${displayEmail}" style="text-align: right; border: 1px solid #ddd; padding: 5px; border-radius: 3px; max-width: 60%;"/>
        </div>
        <div class="info-row">
            <span class="info-label">Phone</span>
            <span class="info-value">${user.phone || user.phoneNumber || 'N/A'}</span>
        </div>
        
        <h3 style="margin-top: 15px; border-top: 1px solid #ddd; padding-top: 15px;">Identity & Status</h3>
        <div class="info-row">
            <span class="info-label">UID</span>
            <span class="info-value">${uid.substring(0, 10)}...</span>
        </div>
        <div class="info-row">
            <span class="info-label">Status</span>
            <span class="info-value" id="modalUserStatusDisplay">
                <span class="badge ${user.isBlocked ? 'blocked' : 'active'}">${user.isBlocked ? 'BLOCKED' : 'ACTIVE'}</span>
            </span>
        </div>
        <div class="info-row">
            <span class="info-label">Registered Since</span>
            <span class="info-value">${user.metadata?.creationTime ? new Date(user.metadata.creationTime).toLocaleDateString() : 'N/A'}</span>
        </div>

    `;

    if (role === 'driver') {
        bodyHtml += `
            <h3 style="margin-top: 15px; border-top: 1px solid #ddd; padding-top: 15px;">Vehicle Info</h3>
            <div class="info-row"><span class="info-label">License</span><span class="info-value">${user.licenseNumber || 'N/A'}</span></div>
            <div class="info-row"><span class="info-label">Model/Type</span><span class="info-value">${user.vehicleModel || 'N/A'} (${user.vehicleType || 'N/A'})</span></div>
            <div class="info-row"><span class="info-label">Plate No.</span><span class="info-value">${user.plateNumber || 'N/A'}</span></div>
        `;
    }


    // Placeholder for "My Trips" (showing that the data layer is separate)
    bodyHtml += `
        <h3 style="margin-top: 15px; border-top: 1px solid #ddd; padding-top: 15px;">Recent Trips</h3>
        <div class="info-row" style="display: block;">
            <p style="color: #999; font-size: 14px;">To view recent trips, please access the 'Rides' tab and filter by UID: <span style="font-weight: 600;">${uid.substring(0, 10)}...</span></p>
            <p style="color: #999; font-size: 14px;">(Full trip history requires a dedicated server query.)</p>
        </div>
    `;


    document.getElementById('modalBody').innerHTML = bodyHtml;

    // Add the SAVE button outside the standard info rows
    const saveButtonHtml = `<button class="btn btn-unblock" style="width: 100%; margin-top: 20px;" onclick="saveUserDetails('${uid}', '${role}')">Save Changes</button>`;
    document.getElementById('modalBody').insertAdjacentHTML('beforeend', saveButtonHtml);

    document.getElementById('userModal').classList.add('active');
}

window.closeModal = function () {
    document.getElementById('userModal').classList.remove('active');
}

window.switchTab = function (tabId) {
    document.querySelectorAll('.tab-content').forEach(content => content.classList.remove('active'));
    document.getElementById(tabId).classList.add('active');

    document.querySelectorAll('.tab-button').forEach(button => button.classList.remove('active'));
    document.getElementById(`btn-${tabId}`).classList.add('active');

    currentActiveTab = tabId;

    // Trigger data loading or filtering for the initial view of the tab
    if (tabId === 'drivers' || tabId === 'passengers') {
        filterUsers();
    }
}

window.logout = function () {
    if (auth) {
        // NOTE: Use document.execCommand('copy') for robust clipboard operation
        auth.signOut().then(() => {
            alert("Logged out successfully.");
            window.location.reload();
        }).catch(error => {
            console.error("Logout Failed:", error);
        });
    }
}