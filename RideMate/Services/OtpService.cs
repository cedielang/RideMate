namespace RideMate.Services
{
    // Simple OTP service for phone verification
    public class OtpService
    {
        // Store OTP codes temporarily (in real app, use database)
        private Dictionary<string, string> _otpCodes = new Dictionary<string, string>();

        // Send OTP to phone number (mock implementation)
        public string SendOTP(string phoneNumber)
        {
            // Generate random 6-digit OTP
            string otp = "123456"; // For testing, always use 123456
            
            // In real app, send SMS here
            // For now, just store it
            string verificationId = Guid.NewGuid().ToString();
            _otpCodes[verificationId] = otp;
            
            return verificationId;
        }

        // Verify OTP code
        public bool VerifyOTP(string verificationId, string enteredOtp)
        {
            // Check if verification ID exists
            if (_otpCodes.ContainsKey(verificationId))
            {
                // Check if OTP matches
                return _otpCodes[verificationId] == enteredOtp;
            }
            
            return false;
        }

        // Get OTP for testing purposes
        public string GetOTP(string verificationId)
        {
            if (_otpCodes.ContainsKey(verificationId))
            {
                return _otpCodes[verificationId];
            }
            
            return string.Empty;
        }
    }
}
