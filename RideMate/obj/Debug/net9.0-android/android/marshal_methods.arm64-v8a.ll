; ModuleID = 'marshal_methods.arm64-v8a.ll'
source_filename = "marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [378 x ptr] zeroinitializer, align 8

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [1134 x i64] [
	i64 u0x0010bf7088f76c5f, ; 0: Google.Cloud.Firestore.V1 => 182
	i64 u0x001e58127c546039, ; 1: lib_System.Globalization.dll.so => 42
	i64 u0x0024d0f62dee05bd, ; 2: Xamarin.KotlinX.Coroutines.Core.dll => 333
	i64 u0x0071cf2d27b7d61e, ; 3: lib_Xamarin.AndroidX.SwipeRefreshLayout.dll.so => 279
	i64 u0x01109b0e4d99e61f, ; 4: System.ComponentModel.Annotations.dll => 13
	i64 u0x020f428300334897, ; 5: Grpc.Net.Client.dll => 189
	i64 u0x02123411c4e01926, ; 6: lib_Xamarin.AndroidX.Navigation.Runtime.dll.so => 269
	i64 u0x0284512fad379f7e, ; 7: System.Runtime.Handles => 105
	i64 u0x02abedc11addc1ed, ; 8: lib_Mono.Android.Runtime.dll.so => 171
	i64 u0x02f55bf70672f5c8, ; 9: lib_System.IO.FileSystem.DriveInfo.dll.so => 48
	i64 u0x032267b2a94db371, ; 10: lib_Xamarin.AndroidX.AppCompat.dll.so => 224
	i64 u0x03621c804933a890, ; 11: System.Buffers => 7
	i64 u0x0399610510a38a38, ; 12: lib_System.Private.DataContractSerialization.dll.so => 86
	i64 u0x043032f1d071fae0, ; 13: ru/Microsoft.Maui.Controls.resources => 363
	i64 u0x044440a55165631e, ; 14: lib-cs-Microsoft.Maui.Controls.resources.dll.so => 341
	i64 u0x046eb1581a80c6b0, ; 15: vi/Microsoft.Maui.Controls.resources => 369
	i64 u0x047408741db2431a, ; 16: Xamarin.AndroidX.DynamicAnimation => 244
	i64 u0x04acae429ea0efac, ; 17: Xamarin.Grpc.Context => 317
	i64 u0x0517ef04e06e9f76, ; 18: System.Net.Primitives => 71
	i64 u0x051a3be159e4ef99, ; 19: Xamarin.GooglePlayServices.Tasks => 314
	i64 u0x0565d18c6da3de38, ; 20: Xamarin.AndroidX.RecyclerView => 272
	i64 u0x0581db89237110e9, ; 21: lib_System.Collections.dll.so => 12
	i64 u0x05989cb940b225a9, ; 22: Microsoft.Maui.dll => 203
	i64 u0x05a1c25e78e22d87, ; 23: lib_System.Runtime.CompilerServices.Unsafe.dll.so => 102
	i64 u0x06076b5d2b581f08, ; 24: zh-HK/Microsoft.Maui.Controls.resources => 370
	i64 u0x06388ffe9f6c161a, ; 25: System.Xml.Linq.dll => 156
	i64 u0x06600c4c124cb358, ; 26: System.Configuration.dll => 19
	i64 u0x067f95c5ddab55b3, ; 27: lib_Xamarin.AndroidX.Fragment.Ktx.dll.so => 249
	i64 u0x0680a433c781bb3d, ; 28: Xamarin.AndroidX.Collection.Jvm => 231
	i64 u0x069fff96ec92a91d, ; 29: System.Xml.XPath.dll => 161
	i64 u0x070b0847e18dab68, ; 30: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 246
	i64 u0x0739448d84d3b016, ; 31: lib_Xamarin.AndroidX.VectorDrawable.dll.so => 282
	i64 u0x07469f2eecce9e85, ; 32: mscorlib.dll => 167
	i64 u0x07c57877c7ba78ad, ; 33: ru/Microsoft.Maui.Controls.resources.dll => 363
	i64 u0x07dcdc7460a0c5e4, ; 34: System.Collections.NonGeneric => 10
	i64 u0x08122e52765333c8, ; 35: lib_Microsoft.Extensions.Logging.Debug.dll.so => 198
	i64 u0x088610fc2509f69e, ; 36: lib_Xamarin.AndroidX.VectorDrawable.Animated.dll.so => 283
	i64 u0x08a7c865576bbde7, ; 37: System.Reflection.Primitives => 96
	i64 u0x08c9d051a4a817e5, ; 38: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 242
	i64 u0x08f3c9788ee2153c, ; 39: Xamarin.AndroidX.DrawerLayout => 243
	i64 u0x09138715c92dba90, ; 40: lib_System.ComponentModel.Annotations.dll.so => 13
	i64 u0x0919c28b89381a0b, ; 41: lib_Microsoft.Extensions.Options.dll.so => 199
	i64 u0x092266563089ae3e, ; 42: lib_System.Collections.NonGeneric.dll.so => 10
	i64 u0x098b50f911ccea8d, ; 43: lib_Xamarin.GooglePlayServices.Basement.dll.so => 313
	i64 u0x09d144a7e214d457, ; 44: System.Security.Cryptography => 127
	i64 u0x09da6dfc3439e851, ; 45: lib_Xamarin.Firebase.Components.dll.so => 296
	i64 u0x09e2b9f743db21a8, ; 46: lib_System.Reflection.Metadata.dll.so => 95
	i64 u0x0abb3e2b271edc45, ; 47: System.Threading.Channels.dll => 140
	i64 u0x0b06b1feab070143, ; 48: System.Formats.Tar => 39
	i64 u0x0b3b632c3bbee20c, ; 49: sk/Microsoft.Maui.Controls.resources => 364
	i64 u0x0b6aff547b84fbe9, ; 50: Xamarin.KotlinX.Serialization.Core.Jvm => 337
	i64 u0x0be2e1f8ce4064ed, ; 51: Xamarin.AndroidX.ViewPager => 285
	i64 u0x0c279376b1ae96ae, ; 52: lib_System.CodeDom.dll.so => 212
	i64 u0x0c3ca6cc978e2aae, ; 53: pt-BR/Microsoft.Maui.Controls.resources => 360
	i64 u0x0c59ad9fbbd43abe, ; 54: Mono.Android => 172
	i64 u0x0c65741e86371ee3, ; 55: lib_Xamarin.Android.Glide.GifDecoder.dll.so => 218
	i64 u0x0c74af560004e816, ; 56: Microsoft.Win32.Registry.dll => 5
	i64 u0x0c7790f60165fc06, ; 57: lib_Microsoft.Maui.Essentials.dll.so => 204
	i64 u0x0c83c82812e96127, ; 58: lib_System.Net.Mail.dll.so => 67
	i64 u0x0cce4bce83380b7f, ; 59: Xamarin.AndroidX.Security.SecurityCrypto => 276
	i64 u0x0d13cd7cce4284e4, ; 60: System.Security.SecureString => 130
	i64 u0x0d3b5ab8b2766190, ; 61: lib_Microsoft.Bcl.AsyncInterfaces.dll.so => 191
	i64 u0x0d565cb22b8879da, ; 62: lib_Grpc.Core.Api.dll.so => 188
	i64 u0x0d63f4f73521c24f, ; 63: lib_Xamarin.AndroidX.SavedState.SavedState.Ktx.dll.so => 275
	i64 u0x0e04e702012f8463, ; 64: Xamarin.AndroidX.Emoji2 => 245
	i64 u0x0e14e73a54dda68e, ; 65: lib_System.Net.NameResolution.dll.so => 68
	i64 u0x0f37dd7a62ae99af, ; 66: lib_Xamarin.AndroidX.Collection.Ktx.dll.so => 232
	i64 u0x0f5e7abaa7cf470a, ; 67: System.Net.HttpListener => 66
	i64 u0x1001f97bbe242e64, ; 68: System.IO.UnmanagedMemoryStream => 57
	i64 u0x102861e4055f511a, ; 69: Microsoft.Bcl.AsyncInterfaces.dll => 191
	i64 u0x102a31b45304b1da, ; 70: Xamarin.AndroidX.CustomView => 241
	i64 u0x1065c4cb554c3d75, ; 71: System.IO.IsolatedStorage.dll => 52
	i64 u0x10f6cfcbcf801616, ; 72: System.IO.Compression.Brotli => 43
	i64 u0x114443cdcf2091f1, ; 73: System.Security.Cryptography.Primitives => 125
	i64 u0x11a603952763e1d4, ; 74: System.Net.Mail => 67
	i64 u0x11a70d0e1009fb11, ; 75: System.Net.WebSockets.dll => 81
	i64 u0x11f26371eee0d3c1, ; 76: lib_Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll.so => 259
	i64 u0x11fbe62d469cc1c8, ; 77: Microsoft.VisualStudio.DesignTools.TapContract.dll => 375
	i64 u0x12128b3f59302d47, ; 78: lib_System.Xml.Serialization.dll.so => 158
	i64 u0x123639456fb056da, ; 79: System.Reflection.Emit.Lightweight.dll => 92
	i64 u0x12521e9764603eaa, ; 80: lib_System.Resources.Reader.dll.so => 99
	i64 u0x125b7f94acb989db, ; 81: Xamarin.AndroidX.RecyclerView.dll => 272
	i64 u0x12d3b63863d4ab0b, ; 82: lib_System.Threading.Overlapped.dll.so => 141
	i64 u0x12dc743c916f95cf, ; 83: lib_RideMate.dll.so => 0
	i64 u0x12f23aabd624cf79, ; 84: lib_Google.Cloud.Firestore.V1.dll.so => 182
	i64 u0x134eab1061c395ee, ; 85: System.Transactions => 151
	i64 u0x138567fa954faa55, ; 86: Xamarin.AndroidX.Browser => 228
	i64 u0x13a01de0cbc3f06c, ; 87: lib-fr-Microsoft.Maui.Controls.resources.dll.so => 347
	i64 u0x13beedefb0e28a45, ; 88: lib_System.Xml.XmlDocument.dll.so => 162
	i64 u0x13f1e5e209e91af4, ; 89: lib_Java.Interop.dll.so => 169
	i64 u0x13f1e880c25d96d1, ; 90: he/Microsoft.Maui.Controls.resources => 348
	i64 u0x1403071365bcd83a, ; 91: Xamarin.Firebase.Annotations => 290
	i64 u0x142d604b786f5eb8, ; 92: lib_Xamarin.CodeHaus.Mojo.AnimalSnifferAnnotations.dll.so => 289
	i64 u0x143d8ea60a6a4011, ; 93: Microsoft.Extensions.DependencyInjection.Abstractions => 195
	i64 u0x1497051b917530bd, ; 94: lib_System.Net.WebSockets.dll.so => 81
	i64 u0x14a644965fc9a91b, ; 95: Xamarin.Google.Android.Recaptcha.dll => 302
	i64 u0x14b78ce3adce0011, ; 96: Microsoft.VisualStudio.DesignTools.TapContract => 375
	i64 u0x14d612a531c79c05, ; 97: Xamarin.JSpecify.dll => 326
	i64 u0x14e68447938213b7, ; 98: Xamarin.AndroidX.Collection.Ktx.dll => 232
	i64 u0x152a448bd1e745a7, ; 99: Microsoft.Win32.Primitives => 4
	i64 u0x1557de0138c445f4, ; 100: lib_Microsoft.Win32.Registry.dll.so => 5
	i64 u0x15bdc156ed462f2f, ; 101: lib_System.IO.FileSystem.dll.so => 51
	i64 u0x15e300c2c1668655, ; 102: System.Resources.Writer.dll => 101
	i64 u0x16726eac78495e6d, ; 103: Xamarin.Grpc.Stub => 321
	i64 u0x16bf2a22df043a09, ; 104: System.IO.Pipes.dll => 56
	i64 u0x16ea2b318ad2d830, ; 105: System.Security.Cryptography.Algorithms => 120
	i64 u0x16eeae54c7ebcc08, ; 106: System.Reflection.dll => 98
	i64 u0x17125c9a85b4929f, ; 107: lib_netstandard.dll.so => 168
	i64 u0x1716866f7416792e, ; 108: lib_System.Security.AccessControl.dll.so => 118
	i64 u0x174f71c46216e44a, ; 109: Xamarin.KotlinX.Coroutines.Core => 333
	i64 u0x1752c12f1e1fc00c, ; 110: System.Core => 21
	i64 u0x17b56e25558a5d36, ; 111: lib-hu-Microsoft.Maui.Controls.resources.dll.so => 351
	i64 u0x17f9358913beb16a, ; 112: System.Text.Encodings.Web => 137
	i64 u0x1809fb23f29ba44a, ; 113: lib_System.Reflection.TypeExtensions.dll.so => 97
	i64 u0x18402a709e357f3b, ; 114: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 337
	i64 u0x18a9befae51bb361, ; 115: System.Net.WebClient => 77
	i64 u0x18f0ce884e87d89a, ; 116: nb/Microsoft.Maui.Controls.resources.dll => 357
	i64 u0x19777fba3c41b398, ; 117: Xamarin.AndroidX.Startup.StartupRuntime.dll => 278
	i64 u0x19a4c090f14ebb66, ; 118: System.Security.Claims => 119
	i64 u0x1a539258f88190d6, ; 119: lib_System.Linq.Async.dll.so => 213
	i64 u0x1a91866a319e9259, ; 120: lib_System.Collections.Concurrent.dll.so => 8
	i64 u0x1aac34d1917ba5d3, ; 121: lib_System.dll.so => 165
	i64 u0x1aad60783ffa3e5b, ; 122: lib-th-Microsoft.Maui.Controls.resources.dll.so => 366
	i64 u0x1aea8f1c3b282172, ; 123: lib_System.Net.Ping.dll.so => 70
	i64 u0x1b4b7a1d0d265fa2, ; 124: Xamarin.Android.Glide.DiskLruCache => 217
	i64 u0x1bbdb16cfa73e785, ; 125: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android => 260
	i64 u0x1bc766e07b2b4241, ; 126: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 273
	i64 u0x1bf5791bb4435277, ; 127: lib_Xamarin.GoogleAndroid.Annotations.dll.so => 310
	i64 u0x1c753b5ff15bce1b, ; 128: Mono.Android.Runtime.dll => 171
	i64 u0x1cb6a0ededc839e2, ; 129: lib_Google.Apis.Auth.dll.so => 179
	i64 u0x1cd47467799d8250, ; 130: System.Threading.Tasks.dll => 145
	i64 u0x1d23eafdc6dc346c, ; 131: System.Globalization.Calendars.dll => 40
	i64 u0x1d33fb39b11be1fe, ; 132: Xamarin.CodeHaus.Mojo.AnimalSnifferAnnotations => 289
	i64 u0x1da4110562816681, ; 133: Xamarin.AndroidX.Security.SecurityCrypto.dll => 276
	i64 u0x1db6820994506bf5, ; 134: System.IO.FileSystem.AccessControl.dll => 47
	i64 u0x1dba6509cc55b56f, ; 135: lib_Google.Protobuf.dll.so => 185
	i64 u0x1dbb0c2c6a999acb, ; 136: System.Diagnostics.StackTrace => 30
	i64 u0x1dcda680b17dc5bb, ; 137: lib_Xamarin.Google.Guava.FailureAccess.dll.so => 308
	i64 u0x1e3d87657e9659bc, ; 138: Xamarin.AndroidX.Navigation.UI => 270
	i64 u0x1e71143913d56c10, ; 139: lib-ko-Microsoft.Maui.Controls.resources.dll.so => 355
	i64 u0x1e7c31185e2fb266, ; 140: lib_System.Threading.Tasks.Parallel.dll.so => 144
	i64 u0x1ed8fcce5e9b50a0, ; 141: Microsoft.Extensions.Options.dll => 199
	i64 u0x1f055d15d807e1b2, ; 142: System.Xml.XmlSerializer => 163
	i64 u0x1f1ed22c1085f044, ; 143: lib_System.Diagnostics.FileVersionInfo.dll.so => 28
	i64 u0x1f61df9c5b94d2c1, ; 144: lib_System.Numerics.dll.so => 84
	i64 u0x1f750bb5421397de, ; 145: lib_Xamarin.AndroidX.Tracing.Tracing.dll.so => 280
	i64 u0x20237ea48006d7a8, ; 146: lib_System.Net.WebClient.dll.so => 77
	i64 u0x209375905fcc1bad, ; 147: lib_System.IO.Compression.Brotli.dll.so => 43
	i64 u0x20e085517023eec8, ; 148: lib_Google.Api.Gax.dll.so => 176
	i64 u0x20fab3cf2dfbc8df, ; 149: lib_System.Diagnostics.Process.dll.so => 29
	i64 u0x2110167c128cba15, ; 150: System.Globalization => 42
	i64 u0x21419508838f7547, ; 151: System.Runtime.CompilerServices.VisualC => 103
	i64 u0x216f61b3e8798976, ; 152: RideMate.dll => 0
	i64 u0x2174319c0d835bc9, ; 153: System.Runtime => 117
	i64 u0x218ae22aa3ec33e7, ; 154: Xamarin.Grpc.Protobuf.Lite.dll => 320
	i64 u0x2198e5bc8b7153fa, ; 155: Xamarin.AndroidX.Annotation.Experimental.dll => 222
	i64 u0x219ea1b751a4dee4, ; 156: lib_System.IO.Compression.ZipFile.dll.so => 45
	i64 u0x21cc7e445dcd5469, ; 157: System.Reflection.Emit.ILGeneration => 91
	i64 u0x220fd4f2e7c48170, ; 158: th/Microsoft.Maui.Controls.resources => 366
	i64 u0x224538d85ed15a82, ; 159: System.IO.Pipes => 56
	i64 u0x22908438c6bed1af, ; 160: lib_System.Threading.Timer.dll.so => 148
	i64 u0x22fbc14e981e3b45, ; 161: lib_Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll.so => 374
	i64 u0x2347c268e3e4e536, ; 162: Xamarin.GooglePlayServices.Basement.dll => 313
	i64 u0x237be844f1f812c7, ; 163: System.Threading.Thread.dll => 146
	i64 u0x23852b3bdc9f7096, ; 164: System.Resources.ResourceManager => 100
	i64 u0x23986dd7e5d4fc01, ; 165: System.IO.FileSystem.Primitives.dll => 49
	i64 u0x23b0dd507a933aa9, ; 166: Google.Api.Gax => 176
	i64 u0x23bbd31edb1647bc, ; 167: lib_Plugin.Firebase.Core.dll.so => 208
	i64 u0x2407aef2bbe8fadf, ; 168: System.Console => 20
	i64 u0x240abe014b27e7d3, ; 169: Xamarin.AndroidX.Core.dll => 237
	i64 u0x247619fe4413f8bf, ; 170: System.Runtime.Serialization.Primitives.dll => 114
	i64 u0x24b87318591adabe, ; 171: lib_Xamarin.Firebase.Database.Collection.dll.so => 297
	i64 u0x24b95d581a70fbee, ; 172: Grpc.Auth.dll => 187
	i64 u0x24d4238047d7310f, ; 173: Google.Apis.Auth => 179
	i64 u0x24de8d301281575e, ; 174: Xamarin.Android.Glide => 215
	i64 u0x252073cc3caa62c2, ; 175: fr/Microsoft.Maui.Controls.resources.dll => 347
	i64 u0x256b8d41255f01b1, ; 176: Xamarin.Google.Crypto.Tink.Android => 304
	i64 u0x25da9c898b4437a3, ; 177: Xamarin.Google.Android.Play.Integrity => 301
	i64 u0x25ffffb1789c4579, ; 178: Plugin.Firebase.Core => 208
	i64 u0x2662c629b96b0b30, ; 179: lib_Xamarin.Kotlin.StdLib.dll.so => 327
	i64 u0x268c1439f13bcc29, ; 180: lib_Microsoft.Extensions.Primitives.dll.so => 200
	i64 u0x26918e5f13c8fc7c, ; 181: Xamarin.Firebase.Firestore => 298
	i64 u0x26a670e154a9c54b, ; 182: System.Reflection.Extensions.dll => 94
	i64 u0x26d077d9678fe34f, ; 183: System.IO.dll => 58
	i64 u0x273f3515de5faf0d, ; 184: id/Microsoft.Maui.Controls.resources.dll => 352
	i64 u0x2742545f9094896d, ; 185: hr/Microsoft.Maui.Controls.resources => 350
	i64 u0x2759af78ab94d39b, ; 186: System.Net.WebSockets => 81
	i64 u0x27b2b16f3e9de038, ; 187: Xamarin.Google.Crypto.Tink.Android.dll => 304
	i64 u0x27b410442fad6cf1, ; 188: Java.Interop.dll => 169
	i64 u0x27b97e0d52c3034a, ; 189: System.Diagnostics.Debug => 26
	i64 u0x27d88445c936a1af, ; 190: lib_Xamarin.Grpc.Android.dll.so => 315
	i64 u0x27eb21c6eb99d774, ; 191: Xamarin.Kotlin.StdLib.Jdk8.dll => 329
	i64 u0x2801845a2c71fbfb, ; 192: System.Net.Primitives.dll => 71
	i64 u0x286835e259162700, ; 193: lib_Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll.so => 271
	i64 u0x2949f3617a02c6b2, ; 194: Xamarin.AndroidX.ExifInterface => 247
	i64 u0x29f947844fb7fc11, ; 195: Microsoft.Maui.Controls.HotReload.Forms => 373
	i64 u0x2a128783efe70ba0, ; 196: uk/Microsoft.Maui.Controls.resources.dll => 368
	i64 u0x2a3b095612184159, ; 197: lib_System.Net.NetworkInformation.dll.so => 69
	i64 u0x2a6507a5ffabdf28, ; 198: System.Diagnostics.TraceSource.dll => 33
	i64 u0x2ad156c8e1354139, ; 199: fi/Microsoft.Maui.Controls.resources => 346
	i64 u0x2ad5d6b13b7a3e04, ; 200: System.ComponentModel.DataAnnotations.dll => 14
	i64 u0x2af298f63581d886, ; 201: System.Text.RegularExpressions.dll => 139
	i64 u0x2afc1c4f898552ee, ; 202: lib_System.Formats.Asn1.dll.so => 38
	i64 u0x2b148910ed40fbf9, ; 203: zh-Hant/Microsoft.Maui.Controls.resources.dll => 372
	i64 u0x2b6989d78cba9a15, ; 204: Xamarin.AndroidX.Concurrent.Futures.dll => 233
	i64 u0x2c8bd14bb93a7d82, ; 205: lib-pl-Microsoft.Maui.Controls.resources.dll.so => 359
	i64 u0x2cbd9262ca785540, ; 206: lib_System.Text.Encoding.CodePages.dll.so => 134
	i64 u0x2cc9e1fed6257257, ; 207: lib_System.Reflection.Emit.Lightweight.dll.so => 92
	i64 u0x2cd723e9fe623c7c, ; 208: lib_System.Private.Xml.Linq.dll.so => 88
	i64 u0x2d169d318a968379, ; 209: System.Threading.dll => 149
	i64 u0x2d1d1413dd10c597, ; 210: Xamarin.Google.Guava.FailureAccess => 308
	i64 u0x2d47774b7d993f59, ; 211: sv/Microsoft.Maui.Controls.resources.dll => 365
	i64 u0x2d5ffcae1ad0aaca, ; 212: System.Data.dll => 24
	i64 u0x2d6267ac7de1d619, ; 213: Xamarin.Firebase.Database.Collection.dll => 297
	i64 u0x2db915caf23548d2, ; 214: System.Text.Json.dll => 138
	i64 u0x2dcaa0bb15a4117a, ; 215: System.IO.UnmanagedMemoryStream.dll => 57
	i64 u0x2e5a40c319acb800, ; 216: System.IO.FileSystem => 51
	i64 u0x2e6f1f226821322a, ; 217: el/Microsoft.Maui.Controls.resources.dll => 344
	i64 u0x2f02f94df3200fe5, ; 218: System.Diagnostics.Process => 29
	i64 u0x2f2e98e1c89b1aff, ; 219: System.Xml.ReaderWriter => 157
	i64 u0x2f5911d9ba814e4e, ; 220: System.Diagnostics.Tracing => 34
	i64 u0x2f84070a459bc31f, ; 221: lib_System.Xml.dll.so => 164
	i64 u0x2fd92a71c7638cfd, ; 222: Xamarin.Firebase.Database.Collection => 297
	i64 u0x309ee9eeec09a71e, ; 223: lib_Xamarin.AndroidX.Fragment.dll.so => 248
	i64 u0x30c6dda129408828, ; 224: System.IO.IsolatedStorage => 52
	i64 u0x31195fef5d8fb552, ; 225: _Microsoft.Android.Resource.Designer.dll => 377
	i64 u0x312c8ed623cbfc8d, ; 226: Xamarin.AndroidX.Window.dll => 287
	i64 u0x31496b779ed0663d, ; 227: lib_System.Reflection.DispatchProxy.dll.so => 90
	i64 u0x315f08d19390dc36, ; 228: Xamarin.Google.ErrorProne.TypeAnnotations => 306
	i64 u0x32243413e774362a, ; 229: Xamarin.AndroidX.CardView.dll => 229
	i64 u0x3235427f8d12dae1, ; 230: lib_System.Drawing.Primitives.dll.so => 35
	i64 u0x329753a17a517811, ; 231: fr/Microsoft.Maui.Controls.resources => 347
	i64 u0x32aa989ff07a84ff, ; 232: lib_System.Xml.ReaderWriter.dll.so => 157
	i64 u0x33829542f112d59b, ; 233: System.Collections.Immutable => 9
	i64 u0x33a31443733849fe, ; 234: lib-es-Microsoft.Maui.Controls.resources.dll.so => 345
	i64 u0x33ec63a7e226adfb, ; 235: Google.Cloud.Location.dll => 183
	i64 u0x341abc357fbb4ebf, ; 236: lib_System.Net.Sockets.dll.so => 76
	i64 u0x342397b849d48e49, ; 237: Xamarin.Grpc.Core => 318
	i64 u0x3496c1e2dcaf5ecc, ; 238: lib_System.IO.Pipes.AccessControl.dll.so => 55
	i64 u0x34dfd74fe2afcf37, ; 239: Microsoft.Maui => 203
	i64 u0x34e292762d9615df, ; 240: cs/Microsoft.Maui.Controls.resources.dll => 341
	i64 u0x3508234247f48404, ; 241: Microsoft.Maui.Controls => 201
	i64 u0x353590da528c9d22, ; 242: System.ComponentModel.Annotations => 13
	i64 u0x353c74869339570c, ; 243: lib_Xamarin.Firebase.Auth.dll.so => 292
	i64 u0x3549870798b4cd30, ; 244: lib_Xamarin.AndroidX.ViewPager2.dll.so => 286
	i64 u0x355282fc1c909694, ; 245: Microsoft.Extensions.Configuration => 192
	i64 u0x3552fc5d578f0fbf, ; 246: Xamarin.AndroidX.Arch.Core.Common => 226
	i64 u0x355c649948d55d97, ; 247: lib_System.Runtime.Intrinsics.dll.so => 109
	i64 u0x356fd122ba041cb4, ; 248: lib_Xamarin.Grpc.Protobuf.Lite.dll.so => 320
	i64 u0x35ea9d1c6834bc8c, ; 249: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 263
	i64 u0x3628ab68db23a01a, ; 250: lib_System.Diagnostics.Tools.dll.so => 32
	i64 u0x364703ab05867b92, ; 251: Xamarin.Firebase.Components => 296
	i64 u0x3673b042508f5b6b, ; 252: lib_System.Runtime.Extensions.dll.so => 104
	i64 u0x36740f1a8ecdc6c4, ; 253: System.Numerics => 84
	i64 u0x36b2b50fdf589ae2, ; 254: System.Reflection.Emit.Lightweight => 92
	i64 u0x36cada77dc79928b, ; 255: System.IO.MemoryMappedFiles => 53
	i64 u0x374ef46b06791af6, ; 256: System.Reflection.Primitives.dll => 96
	i64 u0x376bf93e521a5417, ; 257: lib_Xamarin.Jetbrains.Annotations.dll.so => 325
	i64 u0x379e6c338e5508ad, ; 258: lib_Google.Api.Gax.Grpc.dll.so => 177
	i64 u0x37bc29f3183003b6, ; 259: lib_System.IO.dll.so => 58
	i64 u0x380134e03b1e160a, ; 260: System.Collections.Immutable.dll => 9
	i64 u0x38049b5c59b39324, ; 261: System.Runtime.CompilerServices.Unsafe => 102
	i64 u0x385c17636bb6fe6e, ; 262: Xamarin.AndroidX.CustomView.dll => 241
	i64 u0x38869c811d74050e, ; 263: System.Net.NameResolution.dll => 68
	i64 u0x393c226616977fdb, ; 264: lib_Xamarin.AndroidX.ViewPager.dll.so => 285
	i64 u0x395e37c3334cf82a, ; 265: lib-ca-Microsoft.Maui.Controls.resources.dll.so => 340
	i64 u0x39aa39fda111d9d3, ; 266: Newtonsoft.Json => 206
	i64 u0x3ab5859054645f72, ; 267: System.Security.Cryptography.Primitives.dll => 125
	i64 u0x3ad75090c3fac0e9, ; 268: lib_Xamarin.AndroidX.ResourceInspection.Annotation.dll.so => 273
	i64 u0x3ae44ac43a1fbdbb, ; 269: System.Runtime.Serialization => 116
	i64 u0x3b860f9932505633, ; 270: lib_System.Text.Encoding.Extensions.dll.so => 135
	i64 u0x3c3aafb6b3a00bf6, ; 271: lib_System.Security.Cryptography.X509Certificates.dll.so => 126
	i64 u0x3c4049146b59aa90, ; 272: System.Runtime.InteropServices.JavaScript => 106
	i64 u0x3c51334447dec9e7, ; 273: Google.LongRunning => 184
	i64 u0x3c7c495f58ac5ee9, ; 274: Xamarin.Kotlin.StdLib => 327
	i64 u0x3c7e5ed3d5db71bb, ; 275: System.Security => 131
	i64 u0x3c90a7b70f45292a, ; 276: Xamarin.Grpc.OkHttp.dll => 319
	i64 u0x3cc1676a8781bdbc, ; 277: Xamarin.Firebase.Auth.Interop.dll => 293
	i64 u0x3cd9d281d402eb9b, ; 278: Xamarin.AndroidX.Browser.dll => 228
	i64 u0x3d1c50cc001a991e, ; 279: Xamarin.Google.Guava.ListenableFuture.dll => 309
	i64 u0x3d2b1913edfc08d7, ; 280: lib_System.Threading.ThreadPool.dll.so => 147
	i64 u0x3d46f0b995082740, ; 281: System.Xml.Linq => 156
	i64 u0x3d8a8f400514a790, ; 282: Xamarin.AndroidX.Fragment.Ktx.dll => 249
	i64 u0x3d9c2a242b040a50, ; 283: lib_Xamarin.AndroidX.Core.dll.so => 237
	i64 u0x3daa14724d8f58e8, ; 284: Google.Protobuf.dll => 185
	i64 u0x3dbb6b9f5ab90fa7, ; 285: lib_Xamarin.AndroidX.DynamicAnimation.dll.so => 244
	i64 u0x3e027e6e728d7f1c, ; 286: Google.LongRunning.dll => 184
	i64 u0x3e03e682b4fd9c54, ; 287: BCrypt.Net-Core => 174
	i64 u0x3e5441657549b213, ; 288: Xamarin.AndroidX.ResourceInspection.Annotation => 273
	i64 u0x3e57d4d195c53c2e, ; 289: System.Reflection.TypeExtensions => 97
	i64 u0x3e616ab4ed1f3f15, ; 290: lib_System.Data.dll.so => 24
	i64 u0x3f1d226e6e06db7e, ; 291: Xamarin.AndroidX.SlidingPaneLayout.dll => 277
	i64 u0x3f510adf788828dd, ; 292: System.Threading.Tasks.Extensions => 143
	i64 u0x407a10bb4bf95829, ; 293: lib_Xamarin.AndroidX.Navigation.Common.dll.so => 267
	i64 u0x40c98b6bd77346d4, ; 294: Microsoft.VisualBasic.dll => 3
	i64 u0x41406d6f37320d99, ; 295: Google.Api.Gax.Grpc.dll => 177
	i64 u0x41833cf766d27d96, ; 296: mscorlib => 167
	i64 u0x41cab042be111c34, ; 297: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 225
	i64 u0x423a9ecc4d905a88, ; 298: lib_System.Resources.ResourceManager.dll.so => 100
	i64 u0x423bf51ae7def810, ; 299: System.Xml.XPath => 161
	i64 u0x42418aba44539ffd, ; 300: Google.Cloud.Firestore => 181
	i64 u0x42462ff15ddba223, ; 301: System.Resources.Reader.dll => 99
	i64 u0x4266c67fd9a4ee79, ; 302: Google.Api.CommonProtos => 175
	i64 u0x4291015ff4e5ef71, ; 303: Xamarin.AndroidX.Core.ViewTree.dll => 239
	i64 u0x42a31b86e6ccc3f0, ; 304: System.Diagnostics.Contracts => 25
	i64 u0x42d3cd7add035099, ; 305: System.Management.dll => 214
	i64 u0x430e95b891249788, ; 306: lib_System.Reflection.Emit.dll.so => 93
	i64 u0x43375950ec7c1b6a, ; 307: netstandard.dll => 168
	i64 u0x434c4e1d9284cdae, ; 308: Mono.Android.dll => 172
	i64 u0x43505013578652a0, ; 309: lib_Xamarin.AndroidX.Activity.Ktx.dll.so => 220
	i64 u0x437d06c381ed575a, ; 310: lib_Microsoft.VisualBasic.dll.so => 3
	i64 u0x43950f84de7cc79a, ; 311: pl/Microsoft.Maui.Controls.resources.dll => 359
	i64 u0x43e8ca5bc927ff37, ; 312: lib_Xamarin.AndroidX.Emoji2.ViewsHelper.dll.so => 246
	i64 u0x448bd33429269b19, ; 313: Microsoft.CSharp => 1
	i64 u0x4499fa3c8e494654, ; 314: lib_System.Runtime.Serialization.Primitives.dll.so => 114
	i64 u0x4515080865a951a5, ; 315: Xamarin.Kotlin.StdLib.dll => 327
	i64 u0x4545802489b736b9, ; 316: Xamarin.AndroidX.Fragment.Ktx => 249
	i64 u0x454b4d1e66bb783c, ; 317: Xamarin.AndroidX.Lifecycle.Process => 256
	i64 u0x45b31d67ff6f2b8a, ; 318: lib_Google.Apis.dll.so => 178
	i64 u0x45c40276a42e283e, ; 319: System.Diagnostics.TraceSource => 33
	i64 u0x45d443f2a29adc37, ; 320: System.AppContext.dll => 6
	i64 u0x46a4213bc97fe5ae, ; 321: lib-ru-Microsoft.Maui.Controls.resources.dll.so => 363
	i64 u0x46e11054e74afdb7, ; 322: Xamarin.Firebase.AppCheck.Interop => 291
	i64 u0x47358bd471172e1d, ; 323: lib_System.Xml.Linq.dll.so => 156
	i64 u0x4747e19ad6a1d4bb, ; 324: Grpc.Net.Common => 190
	i64 u0x47a2af602ae797ed, ; 325: lib_Xamarin.KotlinX.Coroutines.Play.Services.dll.so => 335
	i64 u0x47daf4e1afbada10, ; 326: pt/Microsoft.Maui.Controls.resources => 361
	i64 u0x480c0a47dd42dd81, ; 327: lib_System.IO.MemoryMappedFiles.dll.so => 53
	i64 u0x49946f0d78a813ad, ; 328: Xamarin.GooglePlayServices.Auth.Api.Phone => 311
	i64 u0x49e952f19a4e2022, ; 329: System.ObjectModel => 85
	i64 u0x49f6ab815e178ca9, ; 330: lib_Xamarin.Firebase.Common.dll.so => 294
	i64 u0x49f9e6948a8131e4, ; 331: lib_Xamarin.AndroidX.VersionedParcelable.dll.so => 284
	i64 u0x4a5667b2462a664b, ; 332: lib_Xamarin.AndroidX.Navigation.UI.dll.so => 270
	i64 u0x4a7a18981dbd56bc, ; 333: System.IO.Compression.FileSystem.dll => 44
	i64 u0x4aa5c60350917c06, ; 334: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll.so => 255
	i64 u0x4b07a0ed0ab33ff4, ; 335: System.Runtime.Extensions.dll => 104
	i64 u0x4b576d47ac054f3c, ; 336: System.IO.FileSystem.AccessControl => 47
	i64 u0x4b7b6532ded934b7, ; 337: System.Text.Json => 138
	i64 u0x4bb6f6e34164a378, ; 338: Square.OkIO.JVM.dll => 211
	i64 u0x4c7755cf07ad2d5f, ; 339: System.Net.Http.Json.dll => 64
	i64 u0x4cc5f15266470798, ; 340: lib_Xamarin.AndroidX.Loader.dll.so => 265
	i64 u0x4cf6f67dc77aacd2, ; 341: System.Net.NetworkInformation.dll => 69
	i64 u0x4d3183dd245425d4, ; 342: System.Net.WebSockets.Client.dll => 80
	i64 u0x4d479f968a05e504, ; 343: System.Linq.Expressions.dll => 59
	i64 u0x4d55a010ffc4faff, ; 344: System.Private.Xml => 89
	i64 u0x4d5cbe77561c5b2e, ; 345: System.Web.dll => 154
	i64 u0x4d77512dbd86ee4c, ; 346: lib_Xamarin.AndroidX.Arch.Core.Common.dll.so => 226
	i64 u0x4d7793536e79c309, ; 347: System.ServiceProcess => 133
	i64 u0x4d95fccc1f67c7ca, ; 348: System.Runtime.Loader.dll => 110
	i64 u0x4dcf44c3c9b076a2, ; 349: it/Microsoft.Maui.Controls.resources.dll => 353
	i64 u0x4dd9247f1d2c3235, ; 350: Xamarin.AndroidX.Loader.dll => 265
	i64 u0x4e2aeee78e2c4a87, ; 351: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 271
	i64 u0x4e32f00cb0937401, ; 352: Mono.Android.Runtime => 171
	i64 u0x4e5eea4668ac2b18, ; 353: System.Text.Encoding.CodePages => 134
	i64 u0x4ebd0c4b82c5eefc, ; 354: lib_System.Threading.Channels.dll.so => 140
	i64 u0x4ee8eaa9c9c1151a, ; 355: System.Globalization.Calendars => 40
	i64 u0x4f21ee6ef9eb527e, ; 356: ca/Microsoft.Maui.Controls.resources => 340
	i64 u0x4ff8ea8951a69b9f, ; 357: Xamarin.Grpc.Android.dll => 315
	i64 u0x5037f0be3c28c7a3, ; 358: lib_Microsoft.Maui.Controls.dll.so => 201
	i64 u0x508c1fa6b57728d9, ; 359: Grpc.Net.Common.dll => 190
	i64 u0x50c3a29b21050d45, ; 360: System.Linq.Parallel.dll => 60
	i64 u0x5131bbe80989093f, ; 361: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 262
	i64 u0x515d61d6527dac70, ; 362: lib_Xamarin.Firebase.Auth.Interop.dll.so => 293
	i64 u0x516324a5050a7e3c, ; 363: System.Net.WebProxy => 79
	i64 u0x516d6f0b21a303de, ; 364: lib_System.Diagnostics.Contracts.dll.so => 25
	i64 u0x51bb8a2afe774e32, ; 365: System.Drawing => 36
	i64 u0x51d5fd31c5909faf, ; 366: lib_Xamarin.Google.Android.Recaptcha.dll.so => 302
	i64 u0x5247c5c32a4140f0, ; 367: System.Resources.Reader => 99
	i64 u0x526bb15e3c386364, ; 368: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 259
	i64 u0x526ce79eb8e90527, ; 369: lib_System.Net.Primitives.dll.so => 71
	i64 u0x5277169428c6ebf6, ; 370: lib_Grpc.Net.Common.dll.so => 190
	i64 u0x52829f00b4467c38, ; 371: lib_System.Data.Common.dll.so => 22
	i64 u0x529ffe06f39ab8db, ; 372: Xamarin.AndroidX.Core => 237
	i64 u0x52ff996554dbf352, ; 373: Microsoft.Maui.Graphics => 205
	i64 u0x535f7e40e8fef8af, ; 374: lib-sk-Microsoft.Maui.Controls.resources.dll.so => 364
	i64 u0x53978aac584c666e, ; 375: lib_System.Security.Cryptography.Cng.dll.so => 121
	i64 u0x53a96d5c86c9e194, ; 376: System.Net.NetworkInformation => 69
	i64 u0x53be1038a61e8d44, ; 377: System.Runtime.InteropServices.RuntimeInformation.dll => 107
	i64 u0x53c3014b9437e684, ; 378: lib-zh-HK-Microsoft.Maui.Controls.resources.dll.so => 370
	i64 u0x53e450ebd586f842, ; 379: lib_Xamarin.AndroidX.LocalBroadcastManager.dll.so => 266
	i64 u0x5435e6f049e9bc37, ; 380: System.Security.Claims.dll => 119
	i64 u0x54795225dd1587af, ; 381: lib_System.Runtime.dll.so => 117
	i64 u0x547a34f14e5f6210, ; 382: Xamarin.AndroidX.Lifecycle.Common.dll => 251
	i64 u0x54b42cc2b8e65a84, ; 383: Google.Apis.Core.dll => 180
	i64 u0x556e8b63b660ab8b, ; 384: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 252
	i64 u0x5588627c9a108ec9, ; 385: System.Collections.Specialized => 11
	i64 u0x55a898e4f42e3fae, ; 386: Microsoft.VisualBasic.Core.dll => 2
	i64 u0x55fa0c610fe93bb1, ; 387: lib_System.Security.Cryptography.OpenSsl.dll.so => 124
	i64 u0x56442b99bc64bb47, ; 388: System.Runtime.Serialization.Xml.dll => 115
	i64 u0x56a8b26e1aeae27b, ; 389: System.Threading.Tasks.Dataflow => 142
	i64 u0x56eacd1e7e04c04e, ; 390: lib_Xamarin.Google.Android.Play.Integrity.dll.so => 301
	i64 u0x56f932d61e93c07f, ; 391: System.Globalization.Extensions => 41
	i64 u0x571c5cfbec5ae8e2, ; 392: System.Private.Uri => 87
	i64 u0x57201164aeb974e3, ; 393: Xamarin.Google.Guava.FailureAccess.dll => 308
	i64 u0x576499c9f52fea31, ; 394: Xamarin.AndroidX.Annotation => 221
	i64 u0x579a06fed6eec900, ; 395: System.Private.CoreLib.dll => 173
	i64 u0x57c542c14049b66d, ; 396: System.Diagnostics.DiagnosticSource => 27
	i64 u0x581a8bd5cfda563e, ; 397: System.Threading.Timer => 148
	i64 u0x58601b2dda4a27b9, ; 398: lib-ja-Microsoft.Maui.Controls.resources.dll.so => 354
	i64 u0x58688d9af496b168, ; 399: Microsoft.Extensions.DependencyInjection.dll => 194
	i64 u0x588c167a79db6bfb, ; 400: lib_Xamarin.Google.ErrorProne.Annotations.dll.so => 305
	i64 u0x5906028ae5151104, ; 401: Xamarin.AndroidX.Activity.Ktx => 220
	i64 u0x595a356d23e8da9a, ; 402: lib_Microsoft.CSharp.dll.so => 1
	i64 u0x59a935a032dbc08c, ; 403: lib_Grpc.Auth.dll.so => 187
	i64 u0x59f9e60b9475085f, ; 404: lib_Xamarin.AndroidX.Annotation.Experimental.dll.so => 222
	i64 u0x5a0aca0ebb32457d, ; 405: Xamarin.Grpc.Util => 322
	i64 u0x5a6dc081b000c5d7, ; 406: lib_Xamarin.Grpc.OkHttp.dll.so => 319
	i64 u0x5a745f5101a75527, ; 407: lib_System.IO.Compression.FileSystem.dll.so => 44
	i64 u0x5a89a886ae30258d, ; 408: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 236
	i64 u0x5a8f6699f4a1caa9, ; 409: lib_System.Threading.dll.so => 149
	i64 u0x5ae9cd33b15841bf, ; 410: System.ComponentModel => 18
	i64 u0x5aeb8cd498d4823e, ; 411: lib_Xamarin.Google.Guava.dll.so => 307
	i64 u0x5b54391bdc6fcfe6, ; 412: System.Private.DataContractSerialization => 86
	i64 u0x5b5f0e240a06a2a2, ; 413: da/Microsoft.Maui.Controls.resources.dll => 342
	i64 u0x5b755276902c8414, ; 414: Xamarin.GooglePlayServices.Base => 312
	i64 u0x5b8109e8e14c5e3e, ; 415: System.Globalization.Extensions.dll => 41
	i64 u0x5bddd04d72a9e350, ; 416: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 255
	i64 u0x5bdf16b09da116ab, ; 417: Xamarin.AndroidX.Collection => 230
	i64 u0x5bff6a70194300bd, ; 418: lib_Xamarin.Kotlin.StdLib.Jdk8.dll.so => 329
	i64 u0x5c019d5266093159, ; 419: lib_Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android.dll.so => 260
	i64 u0x5c30a4a35f9cc8c4, ; 420: lib_System.Reflection.Extensions.dll.so => 94
	i64 u0x5c393624b8176517, ; 421: lib_Microsoft.Extensions.Logging.dll.so => 196
	i64 u0x5c53c29f5073b0c9, ; 422: System.Diagnostics.FileVersionInfo => 28
	i64 u0x5c87463c575c7616, ; 423: lib_System.Globalization.Extensions.dll.so => 41
	i64 u0x5d0a4a29b02d9d3c, ; 424: System.Net.WebHeaderCollection.dll => 78
	i64 u0x5d40c9b15181641f, ; 425: lib_Xamarin.AndroidX.Emoji2.dll.so => 245
	i64 u0x5d6ca10d35e9485b, ; 426: lib_Xamarin.AndroidX.Concurrent.Futures.dll.so => 233
	i64 u0x5d7ec76c1c703055, ; 427: System.Threading.Tasks.Parallel => 144
	i64 u0x5db0cbbd1028510e, ; 428: lib_System.Runtime.InteropServices.dll.so => 108
	i64 u0x5db30905d3e5013b, ; 429: Xamarin.AndroidX.Collection.Jvm.dll => 231
	i64 u0x5e467bc8f09ad026, ; 430: System.Collections.Specialized.dll => 11
	i64 u0x5e5173b3208d97e7, ; 431: System.Runtime.Handles.dll => 105
	i64 u0x5ea92fdb19ec8c4c, ; 432: System.Text.Encodings.Web.dll => 137
	i64 u0x5eb8046dd40e9ac3, ; 433: System.ComponentModel.Primitives => 16
	i64 u0x5ec272d219c9aba4, ; 434: System.Security.Cryptography.Csp.dll => 122
	i64 u0x5eee1376d94c7f5e, ; 435: System.Net.HttpListener.dll => 66
	i64 u0x5f36ccf5c6a57e24, ; 436: System.Xml.ReaderWriter.dll => 157
	i64 u0x5f4294b9b63cb842, ; 437: System.Data.Common => 22
	i64 u0x5f9a2d823f664957, ; 438: lib-el-Microsoft.Maui.Controls.resources.dll.so => 344
	i64 u0x5fa6da9c3cd8142a, ; 439: lib_Xamarin.KotlinX.Serialization.Core.dll.so => 336
	i64 u0x5fac98e0b37a5b9d, ; 440: System.Runtime.CompilerServices.Unsafe.dll => 102
	i64 u0x609f4b7b63d802d4, ; 441: lib_Microsoft.Extensions.DependencyInjection.dll.so => 194
	i64 u0x60cd4e33d7e60134, ; 442: Xamarin.KotlinX.Coroutines.Core.Jvm => 334
	i64 u0x60f62d786afcf130, ; 443: System.Memory => 63
	i64 u0x61bb78c89f867353, ; 444: System.IO => 58
	i64 u0x61be8d1299194243, ; 445: Microsoft.Maui.Controls.Xaml => 202
	i64 u0x61d2cba29557038f, ; 446: de/Microsoft.Maui.Controls.resources => 343
	i64 u0x61d88f399afb2f45, ; 447: lib_System.Runtime.Loader.dll.so => 110
	i64 u0x622eef6f9e59068d, ; 448: System.Private.CoreLib => 173
	i64 u0x62e976fd765a2339, ; 449: Xamarin.Firebase.Auth.Interop => 293
	i64 u0x63982c87366f9be8, ; 450: Xamarin.Google.Guava => 307
	i64 u0x63cdbd66ac39bb46, ; 451: lib_Microsoft.VisualStudio.DesignTools.XamlTapContract.dll.so => 376
	i64 u0x63d5e3aa4ef9b931, ; 452: Xamarin.KotlinX.Coroutines.Android.dll => 332
	i64 u0x63f1f6883c1e23c2, ; 453: lib_System.Collections.Immutable.dll.so => 9
	i64 u0x6400f68068c1e9f1, ; 454: Xamarin.Google.Android.Material.dll => 300
	i64 u0x640e3b14dbd325c2, ; 455: System.Security.Cryptography.Algorithms.dll => 120
	i64 u0x64587004560099b9, ; 456: System.Reflection => 98
	i64 u0x64b1529a438a3c45, ; 457: lib_System.Runtime.Handles.dll.so => 105
	i64 u0x64f30e567cb41fac, ; 458: Xamarin.KotlinX.Coroutines.Play.Services => 335
	i64 u0x6565fba2cd8f235b, ; 459: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 263
	i64 u0x65ecac39144dd3cc, ; 460: Microsoft.Maui.Controls.dll => 201
	i64 u0x65ece51227bfa724, ; 461: lib_System.Runtime.Numerics.dll.so => 111
	i64 u0x661722438787b57f, ; 462: Xamarin.AndroidX.Annotation.Jvm.dll => 223
	i64 u0x6679b2337ee6b22a, ; 463: lib_System.IO.FileSystem.Primitives.dll.so => 49
	i64 u0x6692e924eade1b29, ; 464: lib_System.Console.dll.so => 20
	i64 u0x66a4e5c6a3fb0bae, ; 465: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 262
	i64 u0x66d13304ce1a3efa, ; 466: Xamarin.AndroidX.CursorAdapter => 240
	i64 u0x674303f65d8fad6f, ; 467: lib_System.Net.Quic.dll.so => 72
	i64 u0x6756ca4cad62e9d6, ; 468: lib_Xamarin.AndroidX.ConstraintLayout.Core.dll.so => 235
	i64 u0x67c0802770244408, ; 469: System.Windows.dll => 155
	i64 u0x68100b69286e27cd, ; 470: lib_System.Formats.Tar.dll.so => 39
	i64 u0x68558ec653afa616, ; 471: lib-da-Microsoft.Maui.Controls.resources.dll.so => 342
	i64 u0x6872ec7a2e36b1ac, ; 472: System.Drawing.Primitives.dll => 35
	i64 u0x68bb2c417aa9b61c, ; 473: Xamarin.KotlinX.AtomicFU.dll => 330
	i64 u0x68fbbbe2eb455198, ; 474: System.Formats.Asn1 => 38
	i64 u0x69063fc0ba8e6bdd, ; 475: he/Microsoft.Maui.Controls.resources.dll => 348
	i64 u0x69a3e26c76f6eec4, ; 476: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 288
	i64 u0x6a4d7577b2317255, ; 477: System.Runtime.InteropServices.dll => 108
	i64 u0x6a61e5b7d5907416, ; 478: lib_Xamarin.GooglePlayServices.Auth.Api.Phone.dll.so => 311
	i64 u0x6ace3b74b15ee4a4, ; 479: nb/Microsoft.Maui.Controls.resources => 357
	i64 u0x6afcedb171067e2b, ; 480: System.Core.dll => 21
	i64 u0x6bc822f45373a1d6, ; 481: Google.Apis.dll => 178
	i64 u0x6bef98e124147c24, ; 482: Xamarin.Jetbrains.Annotations => 325
	i64 u0x6ce874bff138ce2b, ; 483: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 261
	i64 u0x6cfa2bd175637715, ; 484: Square.OkIO.JVM => 211
	i64 u0x6d12bfaa99c72b1f, ; 485: lib_Microsoft.Maui.Graphics.dll.so => 205
	i64 u0x6d70755158ca866e, ; 486: lib_System.ComponentModel.EventBasedAsync.dll.so => 15
	i64 u0x6d79993361e10ef2, ; 487: Microsoft.Extensions.Primitives => 200
	i64 u0x6d7eeca99577fc8b, ; 488: lib_System.Net.WebProxy.dll.so => 79
	i64 u0x6d8515b19946b6a2, ; 489: System.Net.WebProxy.dll => 79
	i64 u0x6d86d56b84c8eb71, ; 490: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 240
	i64 u0x6d9bea6b3e895cf7, ; 491: Microsoft.Extensions.Primitives.dll => 200
	i64 u0x6e25a02c3833319a, ; 492: lib_Xamarin.AndroidX.Navigation.Fragment.dll.so => 268
	i64 u0x6e79c6bd8627412a, ; 493: Xamarin.AndroidX.SavedState.SavedState.Ktx => 275
	i64 u0x6e838d9a2a6f6c9e, ; 494: lib_System.ValueTuple.dll.so => 152
	i64 u0x6e9965ce1095e60a, ; 495: lib_System.Core.dll.so => 21
	i64 u0x6fd2265da78b93a4, ; 496: lib_Microsoft.Maui.dll.so => 203
	i64 u0x6fdfc7de82c33008, ; 497: cs/Microsoft.Maui.Controls.resources => 341
	i64 u0x6ffc4967cc47ba57, ; 498: System.IO.FileSystem.Watcher.dll => 50
	i64 u0x701cd46a1c25a5fe, ; 499: System.IO.FileSystem.dll => 51
	i64 u0x70664ad3307f4fbf, ; 500: Xamarin.Grpc.Core.dll => 318
	i64 u0x70e99f48c05cb921, ; 501: tr/Microsoft.Maui.Controls.resources.dll => 367
	i64 u0x70fd3deda22442d2, ; 502: lib-nb-Microsoft.Maui.Controls.resources.dll.so => 357
	i64 u0x71485e7ffdb4b958, ; 503: System.Reflection.Extensions => 94
	i64 u0x7162a2fce67a945f, ; 504: lib_Xamarin.Android.Glide.Annotations.dll.so => 216
	i64 u0x719176ec28437f1c, ; 505: Xamarin.Protobuf.JavaLite => 338
	i64 u0x71a495ea3761dde8, ; 506: lib-it-Microsoft.Maui.Controls.resources.dll.so => 353
	i64 u0x71ad672adbe48f35, ; 507: System.ComponentModel.Primitives.dll => 16
	i64 u0x7207f5c4519b0c98, ; 508: Plugin.Firebase.Auth.dll => 207
	i64 u0x720f102581a4a5c8, ; 509: Xamarin.AndroidX.Core.ViewTree => 239
	i64 u0x725f5a9e82a45c81, ; 510: System.Security.Cryptography.Encoding => 123
	i64 u0x72b1fb4109e08d7b, ; 511: lib-hr-Microsoft.Maui.Controls.resources.dll.so => 350
	i64 u0x72e0300099accce1, ; 512: System.Xml.XPath.XDocument => 160
	i64 u0x730bfb248998f67a, ; 513: System.IO.Compression.ZipFile => 45
	i64 u0x732b2d67b9e5c47b, ; 514: Xamarin.Google.ErrorProne.Annotations.dll => 305
	i64 u0x734b76fdc0dc05bb, ; 515: lib_GoogleGson.dll.so => 186
	i64 u0x73a6be34e822f9d1, ; 516: lib_System.Runtime.Serialization.dll.so => 116
	i64 u0x73e4ce94e2eb6ffc, ; 517: lib_System.Memory.dll.so => 63
	i64 u0x743a1eccf080489a, ; 518: WindowsBase.dll => 166
	i64 u0x755a91767330b3d4, ; 519: lib_Microsoft.Extensions.Configuration.dll.so => 192
	i64 u0x75c326eb821b85c4, ; 520: lib_System.ComponentModel.DataAnnotations.dll.so => 14
	i64 u0x76012e7334db86e5, ; 521: lib_Xamarin.AndroidX.SavedState.dll.so => 274
	i64 u0x76ca07b878f44da0, ; 522: System.Runtime.Numerics.dll => 111
	i64 u0x7736c8a96e51a061, ; 523: lib_Xamarin.AndroidX.Annotation.Jvm.dll.so => 223
	i64 u0x778a805e625329ef, ; 524: System.Linq.Parallel => 60
	i64 u0x779290cc2b801eb7, ; 525: Xamarin.KotlinX.AtomicFU.Jvm => 331
	i64 u0x77f8a4acc2fdc449, ; 526: System.Security.Cryptography.Cng.dll => 121
	i64 u0x780bc73597a503a9, ; 527: lib-ms-Microsoft.Maui.Controls.resources.dll.so => 356
	i64 u0x782c5d8eb99ff201, ; 528: lib_Microsoft.VisualBasic.Core.dll.so => 2
	i64 u0x783606d1e53e7a1a, ; 529: th/Microsoft.Maui.Controls.resources.dll => 366
	i64 u0x784b4ff3eed363ff, ; 530: Xamarin.Firebase.Common => 294
	i64 u0x78a45e51311409b6, ; 531: Xamarin.AndroidX.Fragment.dll => 248
	i64 u0x78ed4ab8f9d800a1, ; 532: Xamarin.AndroidX.Lifecycle.ViewModel => 261
	i64 u0x7a39601d6f0bb831, ; 533: lib_Xamarin.KotlinX.AtomicFU.dll.so => 330
	i64 u0x7a5207a7c82d30b4, ; 534: lib_Xamarin.JSpecify.dll.so => 326
	i64 u0x7a7e7eddf79c5d26, ; 535: lib_Xamarin.AndroidX.Lifecycle.ViewModel.dll.so => 261
	i64 u0x7a9a57d43b0845fa, ; 536: System.AppContext => 6
	i64 u0x7ad0f4f1e5d08183, ; 537: Xamarin.AndroidX.Collection.dll => 230
	i64 u0x7adb8da2ac89b647, ; 538: fi/Microsoft.Maui.Controls.resources.dll => 346
	i64 u0x7b13d9eaa944ade8, ; 539: Xamarin.AndroidX.DynamicAnimation.dll => 244
	i64 u0x7b1644fe7249da0c, ; 540: Plugin.Firebase.Firestore => 209
	i64 u0x7b41df00b4b7f385, ; 541: Xamarin.CodeHaus.Mojo.AnimalSnifferAnnotations.dll => 289
	i64 u0x7bef86a4335c4870, ; 542: System.ComponentModel.TypeConverter => 17
	i64 u0x7c0820144cd34d6a, ; 543: sk/Microsoft.Maui.Controls.resources.dll => 364
	i64 u0x7c2a0bd1e0f988fc, ; 544: lib-de-Microsoft.Maui.Controls.resources.dll.so => 343
	i64 u0x7c41d387501568ba, ; 545: System.Net.WebClient.dll => 77
	i64 u0x7c47a030f8618534, ; 546: RideMate => 0
	i64 u0x7c482cd79bd24b13, ; 547: lib_Xamarin.AndroidX.ConstraintLayout.dll.so => 234
	i64 u0x7cb95ad2a929d044, ; 548: Xamarin.GooglePlayServices.Basement => 313
	i64 u0x7cd2ec8eaf5241cd, ; 549: System.Security.dll => 131
	i64 u0x7cf9ae50dd350622, ; 550: Xamarin.Jetbrains.Annotations.dll => 325
	i64 u0x7d649b75d580bb42, ; 551: ms/Microsoft.Maui.Controls.resources.dll => 356
	i64 u0x7d8ee2bdc8e3aad1, ; 552: System.Numerics.Vectors => 83
	i64 u0x7df5df8db8eaa6ac, ; 553: Microsoft.Extensions.Logging.Debug => 198
	i64 u0x7dfc3d6d9d8d7b70, ; 554: System.Collections => 12
	i64 u0x7e2e564fa2f76c65, ; 555: lib_System.Diagnostics.Tracing.dll.so => 34
	i64 u0x7e302e110e1e1346, ; 556: lib_System.Security.Claims.dll.so => 119
	i64 u0x7e4465b3f78ad8d0, ; 557: Xamarin.KotlinX.Serialization.Core.dll => 336
	i64 u0x7e571cad5915e6c3, ; 558: lib_Xamarin.AndroidX.Lifecycle.Process.dll.so => 256
	i64 u0x7e65340ed0da76d2, ; 559: Xamarin.Grpc.OkHttp => 319
	i64 u0x7e6b1ca712437d7d, ; 560: Xamarin.AndroidX.Emoji2.ViewsHelper => 246
	i64 u0x7e946809d6008ef2, ; 561: lib_System.ObjectModel.dll.so => 85
	i64 u0x7ea0272c1b4a9635, ; 562: lib_Xamarin.Android.Glide.dll.so => 215
	i64 u0x7eb4f0dc47488736, ; 563: lib_Xamarin.GooglePlayServices.Tasks.dll.so => 314
	i64 u0x7ecc13347c8fd849, ; 564: lib_System.ComponentModel.dll.so => 18
	i64 u0x7f00ddd9b9ca5a13, ; 565: Xamarin.AndroidX.ViewPager.dll => 285
	i64 u0x7f9351cd44b1273f, ; 566: Microsoft.Extensions.Configuration.Abstractions => 193
	i64 u0x7fbd557c99b3ce6f, ; 567: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 254
	i64 u0x8076a9a44a2ca331, ; 568: System.Net.Quic => 72
	i64 u0x80b7e726b0280681, ; 569: Microsoft.VisualStudio.DesignTools.MobileTapContracts => 374
	i64 u0x80da183a87731838, ; 570: System.Reflection.Metadata => 95
	i64 u0x812c069d5cdecc17, ; 571: System.dll => 165
	i64 u0x81381be520a60adb, ; 572: Xamarin.AndroidX.Interpolator.dll => 250
	i64 u0x8145faf772692484, ; 573: Google.Cloud.Firestore.V1.dll => 182
	i64 u0x81657cec2b31e8aa, ; 574: System.Net => 82
	i64 u0x81ab745f6c0f5ce6, ; 575: zh-Hant/Microsoft.Maui.Controls.resources => 372
	i64 u0x81db25321ea48ab3, ; 576: lib_Xamarin.Firebase.AppCheck.Interop.dll.so => 291
	i64 u0x8277f2be6b5ce05f, ; 577: Xamarin.AndroidX.AppCompat => 224
	i64 u0x828f06563b30bc50, ; 578: lib_Xamarin.AndroidX.CardView.dll.so => 229
	i64 u0x82920a8d9194a019, ; 579: Xamarin.KotlinX.AtomicFU.Jvm.dll => 331
	i64 u0x82b399cb01b531c4, ; 580: lib_System.Web.dll.so => 154
	i64 u0x82df8f5532a10c59, ; 581: lib_System.Drawing.dll.so => 36
	i64 u0x82f0b6e911d13535, ; 582: lib_System.Transactions.dll.so => 151
	i64 u0x82f6403342e12049, ; 583: uk/Microsoft.Maui.Controls.resources => 368
	i64 u0x83c14ba66c8e2b8c, ; 584: zh-Hans/Microsoft.Maui.Controls.resources => 371
	i64 u0x846ce984efea52c7, ; 585: System.Threading.Tasks.Parallel.dll => 144
	i64 u0x84ae73148a4557d2, ; 586: lib_System.IO.Pipes.dll.so => 56
	i64 u0x84b01102c12a9232, ; 587: System.Runtime.Serialization.Json.dll => 113
	i64 u0x850c5ba0b57ce8e7, ; 588: lib_Xamarin.AndroidX.Collection.dll.so => 230
	i64 u0x851d02edd334b044, ; 589: Xamarin.AndroidX.VectorDrawable => 282
	i64 u0x85410a0ce2b82e74, ; 590: lib_Xamarin.Grpc.Context.dll.so => 317
	i64 u0x85c919db62150978, ; 591: Xamarin.AndroidX.Transition.dll => 281
	i64 u0x85fe8aab7ca9ed11, ; 592: Xamarin.Google.Android.Recaptcha => 302
	i64 u0x8662aaeb94fef37f, ; 593: lib_System.Dynamic.Runtime.dll.so => 37
	i64 u0x86a909228dc7657b, ; 594: lib-zh-Hant-Microsoft.Maui.Controls.resources.dll.so => 372
	i64 u0x86b3e00c36b84509, ; 595: Microsoft.Extensions.Configuration.dll => 192
	i64 u0x86b62cb077ec4fd7, ; 596: System.Runtime.Serialization.Xml => 115
	i64 u0x8706ffb12bf3f53d, ; 597: Xamarin.AndroidX.Annotation.Experimental => 222
	i64 u0x872a5b14c18d328c, ; 598: System.ComponentModel.DataAnnotations => 14
	i64 u0x872fb9615bc2dff0, ; 599: Xamarin.Android.Glide.Annotations.dll => 216
	i64 u0x8794c7c19600413d, ; 600: Xamarin.Grpc.Protobuf.Lite => 320
	i64 u0x87b7bede2c8fef74, ; 601: Xamarin.Firebase.ProtoliteWellKnownTypes.dll => 299
	i64 u0x87c69b87d9283884, ; 602: lib_System.Threading.Thread.dll.so => 146
	i64 u0x87f6569b25707834, ; 603: System.IO.Compression.Brotli.dll => 43
	i64 u0x87fe2bfaae9f8412, ; 604: lib_Plugin.Firebase.Auth.dll.so => 207
	i64 u0x87fef727071b7fe5, ; 605: Grpc.Net.Client => 189
	i64 u0x8842b3a5d2d3fb36, ; 606: Microsoft.Maui.Essentials => 204
	i64 u0x88926583efe7ee86, ; 607: Xamarin.AndroidX.Activity.Ktx.dll => 220
	i64 u0x88b16a1a7051ebe2, ; 608: Xamarin.Firebase.Annotations.dll => 290
	i64 u0x88ba6bc4f7762b03, ; 609: lib_System.Reflection.dll.so => 98
	i64 u0x88bda98e0cffb7a9, ; 610: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 334
	i64 u0x8930322c7bd8f768, ; 611: netstandard => 168
	i64 u0x897a606c9e39c75f, ; 612: lib_System.ComponentModel.Primitives.dll.so => 16
	i64 u0x89911a22005b92b7, ; 613: System.IO.FileSystem.DriveInfo.dll => 48
	i64 u0x89c5188089ec2cd5, ; 614: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 107
	i64 u0x8a19e3dc71b34b2c, ; 615: System.Reflection.TypeExtensions.dll => 97
	i64 u0x8a90bab2026e5b88, ; 616: Google.Cloud.Firestore.dll => 181
	i64 u0x8ad229ea26432ee2, ; 617: Xamarin.AndroidX.Loader => 265
	i64 u0x8b4ff5d0fdd5faa1, ; 618: lib_System.Diagnostics.DiagnosticSource.dll.so => 27
	i64 u0x8b541d476eb3774c, ; 619: System.Security.Principal.Windows => 128
	i64 u0x8b8d01333a96d0b5, ; 620: System.Diagnostics.Process.dll => 29
	i64 u0x8b9ceca7acae3451, ; 621: lib-he-Microsoft.Maui.Controls.resources.dll.so => 348
	i64 u0x8c230514448fef34, ; 622: Xamarin.Firebase.ProtoliteWellKnownTypes => 299
	i64 u0x8cb8f612b633affb, ; 623: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 275
	i64 u0x8cdfdb4ce85fb925, ; 624: lib_System.Security.Principal.Windows.dll.so => 128
	i64 u0x8cdfe7b8f4caa426, ; 625: System.IO.Compression.FileSystem => 44
	i64 u0x8d0f420977c2c1c7, ; 626: Xamarin.AndroidX.CursorAdapter.dll => 240
	i64 u0x8d52f7ea2796c531, ; 627: Xamarin.AndroidX.Emoji2.dll => 245
	i64 u0x8d7b8ab4b3310ead, ; 628: System.Threading => 149
	i64 u0x8da188285aadfe8e, ; 629: System.Collections.Concurrent => 8
	i64 u0x8dfc1cfbf8858f95, ; 630: Grpc.Core.Api.dll => 188
	i64 u0x8ec6e06a61c1baeb, ; 631: lib_Newtonsoft.Json.dll.so => 206
	i64 u0x8ed807bfe9858dfc, ; 632: Xamarin.AndroidX.Navigation.Common => 267
	i64 u0x8ee08b8194a30f48, ; 633: lib-hi-Microsoft.Maui.Controls.resources.dll.so => 349
	i64 u0x8ef7601039857a44, ; 634: lib-ro-Microsoft.Maui.Controls.resources.dll.so => 362
	i64 u0x8efbc0801a122264, ; 635: Xamarin.GooglePlayServices.Tasks.dll => 314
	i64 u0x8f32c6f611f6ffab, ; 636: pt/Microsoft.Maui.Controls.resources.dll => 361
	i64 u0x8f44b45eb046bbd1, ; 637: System.ServiceModel.Web.dll => 132
	i64 u0x8f8829d21c8985a4, ; 638: lib-pt-BR-Microsoft.Maui.Controls.resources.dll.so => 360
	i64 u0x8f8fc9ec4cd22542, ; 639: lib_BCrypt.Net-Core.dll.so => 174
	i64 u0x8fbf5b0114c6dcef, ; 640: System.Globalization.dll => 42
	i64 u0x8fcc8c2a81f3d9e7, ; 641: Xamarin.KotlinX.Serialization.Core => 336
	i64 u0x8fd42635e63de49e, ; 642: Xamarin.Grpc.Context.dll => 317
	i64 u0x8feffa44c5924324, ; 643: lib_Xamarin.Protobuf.JavaLite.dll.so => 338
	i64 u0x90263f8448b8f572, ; 644: lib_System.Diagnostics.TraceSource.dll.so => 33
	i64 u0x903101b46fb73a04, ; 645: _Microsoft.Android.Resource.Designer => 377
	i64 u0x90393bd4865292f3, ; 646: lib_System.IO.Compression.dll.so => 46
	i64 u0x905e2b8e7ae91ae6, ; 647: System.Threading.Tasks.Extensions.dll => 143
	i64 u0x90634f86c5ebe2b5, ; 648: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 262
	i64 u0x907b636704ad79ef, ; 649: lib_Microsoft.Maui.Controls.Xaml.dll.so => 202
	i64 u0x90e9efbfd68593e0, ; 650: lib_Xamarin.AndroidX.Lifecycle.LiveData.dll.so => 253
	i64 u0x91413101e5d9f995, ; 651: Xamarin.Firebase.Auth => 292
	i64 u0x91418dc638b29e68, ; 652: lib_Xamarin.AndroidX.CustomView.dll.so => 241
	i64 u0x9157bd523cd7ed36, ; 653: lib_System.Text.Json.dll.so => 138
	i64 u0x91a74f07b30d37e2, ; 654: System.Linq.dll => 62
	i64 u0x91cb86ea3b17111d, ; 655: System.ServiceModel.Web => 132
	i64 u0x91fa41a87223399f, ; 656: ca/Microsoft.Maui.Controls.resources.dll => 340
	i64 u0x92054e486c0c7ea7, ; 657: System.IO.FileSystem.DriveInfo => 48
	i64 u0x928614058c40c4cd, ; 658: lib_System.Xml.XPath.XDocument.dll.so => 160
	i64 u0x92a698e6d582778f, ; 659: Xamarin.Firebase.Components.dll => 296
	i64 u0x92b138fffca2b01e, ; 660: lib_Xamarin.AndroidX.Arch.Core.Runtime.dll.so => 227
	i64 u0x92dfc2bfc6c6a888, ; 661: Xamarin.AndroidX.Lifecycle.LiveData => 253
	i64 u0x933da2c779423d68, ; 662: Xamarin.Android.Glide.Annotations => 216
	i64 u0x9388aad9b7ae40ce, ; 663: lib_Xamarin.AndroidX.Lifecycle.Common.dll.so => 251
	i64 u0x93cfa73ab28d6e35, ; 664: ms/Microsoft.Maui.Controls.resources => 356
	i64 u0x941c00d21e5c0679, ; 665: lib_Xamarin.AndroidX.Transition.dll.so => 281
	i64 u0x944077d8ca3c6580, ; 666: System.IO.Compression.dll => 46
	i64 u0x948cffedc8ed7960, ; 667: System.Xml => 164
	i64 u0x94c8990839c4bdb1, ; 668: lib_Xamarin.AndroidX.Interpolator.dll.so => 250
	i64 u0x967fc325e09bfa8c, ; 669: es/Microsoft.Maui.Controls.resources => 345
	i64 u0x9686161486d34b81, ; 670: lib_Xamarin.AndroidX.ExifInterface.dll.so => 247
	i64 u0x9729c8c4c069c478, ; 671: Google.Apis.Core => 180
	i64 u0x9732d8dbddea3d9a, ; 672: id/Microsoft.Maui.Controls.resources => 352
	i64 u0x97695ad5e6dbb63d, ; 673: Xamarin.Io.PerfMark.PerfMarkApi.dll => 323
	i64 u0x978be80e5210d31b, ; 674: Microsoft.Maui.Graphics.dll => 205
	i64 u0x979ab54025cc1c7f, ; 675: lib_Xamarin.GooglePlayServices.Base.dll.so => 312
	i64 u0x97b8c771ea3e4220, ; 676: System.ComponentModel.dll => 18
	i64 u0x97e144c9d3c6976e, ; 677: System.Collections.Concurrent.dll => 8
	i64 u0x97e55f96df4ddd72, ; 678: lib_Xamarin.Firebase.Annotations.dll.so => 290
	i64 u0x984184e3c70d4419, ; 679: GoogleGson => 186
	i64 u0x9843944103683dd3, ; 680: Xamarin.AndroidX.Core.Core.Ktx => 238
	i64 u0x98d720cc4597562c, ; 681: System.Security.Cryptography.OpenSsl => 124
	i64 u0x991d510397f92d9d, ; 682: System.Linq.Expressions => 59
	i64 u0x996ceeb8a3da3d67, ; 683: System.Threading.Overlapped.dll => 141
	i64 u0x99a00ca5270c6878, ; 684: Xamarin.AndroidX.Navigation.Runtime => 269
	i64 u0x99cdc6d1f2d3a72f, ; 685: ko/Microsoft.Maui.Controls.resources.dll => 355
	i64 u0x9a01b1da98b6ee10, ; 686: Xamarin.AndroidX.Lifecycle.Runtime.dll => 257
	i64 u0x9a5ccc274fd6e6ee, ; 687: Jsr305Binding.dll => 303
	i64 u0x9aca298b5a43ad5c, ; 688: Plugin.Firebase.Core.dll => 208
	i64 u0x9ae6940b11c02876, ; 689: lib_Xamarin.AndroidX.Window.dll.so => 287
	i64 u0x9b211a749105beac, ; 690: System.Transactions.Local => 150
	i64 u0x9b8734714671022d, ; 691: System.Threading.Tasks.Dataflow.dll => 142
	i64 u0x9bb3e2d53d840e51, ; 692: Xamarin.Protobuf.JavaLite.dll => 338
	i64 u0x9bc6aea27fbf034f, ; 693: lib_Xamarin.KotlinX.Coroutines.Core.dll.so => 333
	i64 u0x9bd8cc74558ad4c7, ; 694: Xamarin.KotlinX.AtomicFU => 330
	i64 u0x9c244ac7cda32d26, ; 695: System.Security.Cryptography.X509Certificates.dll => 126
	i64 u0x9c465f280cf43733, ; 696: lib_Xamarin.KotlinX.Coroutines.Android.dll.so => 332
	i64 u0x9c8f6872beab6408, ; 697: System.Xml.XPath.XDocument.dll => 160
	i64 u0x9ce01cf91101ae23, ; 698: System.Xml.XmlDocument => 162
	i64 u0x9d128180c81d7ce6, ; 699: Xamarin.AndroidX.CustomView.PoolingContainer => 242
	i64 u0x9d5dbcf5a48583fe, ; 700: lib_Xamarin.AndroidX.Activity.dll.so => 219
	i64 u0x9d74dee1a7725f34, ; 701: Microsoft.Extensions.Configuration.Abstractions.dll => 193
	i64 u0x9e4534b6adaf6e84, ; 702: nl/Microsoft.Maui.Controls.resources => 358
	i64 u0x9e4b95dec42769f7, ; 703: System.Diagnostics.Debug.dll => 26
	i64 u0x9eaf1efdf6f7267e, ; 704: Xamarin.AndroidX.Navigation.Common.dll => 267
	i64 u0x9ef542cf1f78c506, ; 705: Xamarin.AndroidX.Lifecycle.LiveData.Core => 254
	i64 u0xa00832eb975f56a8, ; 706: lib_System.Net.dll.so => 82
	i64 u0xa0ad78236b7b267f, ; 707: Xamarin.AndroidX.Window => 287
	i64 u0xa0d8259f4cc284ec, ; 708: lib_System.Security.Cryptography.dll.so => 127
	i64 u0xa0e17ca50c77a225, ; 709: lib_Xamarin.Google.Crypto.Tink.Android.dll.so => 304
	i64 u0xa0ff9b3e34d92f11, ; 710: lib_System.Resources.Writer.dll.so => 101
	i64 u0xa123c421dc71de88, ; 711: lib_Xamarin.Grpc.Util.dll.so => 322
	i64 u0xa12fbfb4da97d9f3, ; 712: System.Threading.Timer.dll => 148
	i64 u0xa1440773ee9d341e, ; 713: Xamarin.Google.Android.Material => 300
	i64 u0xa1b9d7c27f47219f, ; 714: Xamarin.AndroidX.Navigation.UI.dll => 270
	i64 u0xa2572680829d2c7c, ; 715: System.IO.Pipelines.dll => 54
	i64 u0xa258230b8fad57ac, ; 716: lib_Xamarin.Io.PerfMark.PerfMarkApi.dll.so => 323
	i64 u0xa26597e57ee9c7f6, ; 717: System.Xml.XmlDocument.dll => 162
	i64 u0xa308401900e5bed3, ; 718: lib_mscorlib.dll.so => 167
	i64 u0xa395572e7da6c99d, ; 719: lib_System.Security.dll.so => 131
	i64 u0xa3e683f24b43af6f, ; 720: System.Dynamic.Runtime.dll => 37
	i64 u0xa4145becdee3dc4f, ; 721: Xamarin.AndroidX.VectorDrawable.Animated => 283
	i64 u0xa46aa1eaa214539b, ; 722: ko/Microsoft.Maui.Controls.resources => 355
	i64 u0xa4edc8f2ceae241a, ; 723: System.Data.Common.dll => 22
	i64 u0xa5494f40f128ce6a, ; 724: System.Runtime.Serialization.Formatters.dll => 112
	i64 u0xa54b74df83dce92b, ; 725: System.Reflection.DispatchProxy => 90
	i64 u0xa5b7152421ed6d98, ; 726: lib_System.IO.FileSystem.Watcher.dll.so => 50
	i64 u0xa5c3844f17b822db, ; 727: lib_System.Linq.Parallel.dll.so => 60
	i64 u0xa5ce5c755bde8cb8, ; 728: lib_System.Security.Cryptography.Csp.dll.so => 122
	i64 u0xa5e599d1e0524750, ; 729: System.Numerics.Vectors.dll => 83
	i64 u0xa5f1ba49b85dd355, ; 730: System.Security.Cryptography.dll => 127
	i64 u0xa5f1e826b58a6998, ; 731: System.Linq.Async.dll => 213
	i64 u0xa61975a5a37873ea, ; 732: lib_System.Xml.XmlSerializer.dll.so => 163
	i64 u0xa6593e21584384d2, ; 733: lib_Jsr305Binding.dll.so => 303
	i64 u0xa66cbee0130865f7, ; 734: lib_WindowsBase.dll.so => 166
	i64 u0xa67dbee13e1df9ca, ; 735: Xamarin.AndroidX.SavedState.dll => 274
	i64 u0xa684b098dd27b296, ; 736: lib_Xamarin.AndroidX.Security.SecurityCrypto.dll.so => 276
	i64 u0xa68a420042bb9b1f, ; 737: Xamarin.AndroidX.DrawerLayout.dll => 243
	i64 u0xa6d26156d1cacc7c, ; 738: Xamarin.Android.Glide.dll => 215
	i64 u0xa75386b5cb9595aa, ; 739: Xamarin.AndroidX.Lifecycle.Runtime.Android => 258
	i64 u0xa763fbb98df8d9fb, ; 740: lib_Microsoft.Win32.Primitives.dll.so => 4
	i64 u0xa78ce3745383236a, ; 741: Xamarin.AndroidX.Lifecycle.Common.Jvm => 252
	i64 u0xa7c31b56b4dc7b33, ; 742: hu/Microsoft.Maui.Controls.resources => 351
	i64 u0xa7eab29ed44b4e7a, ; 743: Mono.Android.Export => 170
	i64 u0xa8195217cbf017b7, ; 744: Microsoft.VisualBasic.Core => 2
	i64 u0xa843f6095f0d247d, ; 745: Xamarin.GooglePlayServices.Base.dll => 312
	i64 u0xa859a95830f367ff, ; 746: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll.so => 263
	i64 u0xa8b52f21e0dbe690, ; 747: System.Runtime.Serialization.dll => 116
	i64 u0xa8c84ce526c2b4bd, ; 748: Microsoft.VisualStudio.DesignTools.XamlTapContract.dll => 376
	i64 u0xa8ee4ed7de2efaee, ; 749: Xamarin.AndroidX.Annotation.dll => 221
	i64 u0xa9302afdfc455810, ; 750: Xamarin.GoogleAndroid.Annotations.dll => 310
	i64 u0xa952cc4a0d808a59, ; 751: lib_Google.Api.CommonProtos.dll.so => 175
	i64 u0xa95590e7c57438a4, ; 752: System.Configuration => 19
	i64 u0xaa2219c8e3449ff5, ; 753: Microsoft.Extensions.Logging.Abstractions => 197
	i64 u0xaa2de94d374e55df, ; 754: Xamarin.Firebase.Common.Ktx => 295
	i64 u0xaa443ac34067eeef, ; 755: System.Private.Xml.dll => 89
	i64 u0xaa52de307ef5d1dd, ; 756: System.Net.Http => 65
	i64 u0xaa571d3c32740d65, ; 757: Plugin.Firebase.Auth => 207
	i64 u0xaa9a7b0214a5cc5c, ; 758: System.Diagnostics.StackTrace.dll => 30
	i64 u0xaaaf86367285a918, ; 759: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 195
	i64 u0xaaf84bb3f052a265, ; 760: el/Microsoft.Maui.Controls.resources => 344
	i64 u0xab375658f5084c9f, ; 761: lib_Google.Cloud.Firestore.dll.so => 181
	i64 u0xab9af77b5b67a0b8, ; 762: Xamarin.AndroidX.ConstraintLayout.Core => 235
	i64 u0xab9c1b2687d86b0b, ; 763: lib_System.Linq.Expressions.dll.so => 59
	i64 u0xac2af3fa195a15ce, ; 764: System.Runtime.Numerics => 111
	i64 u0xac5376a2a538dc10, ; 765: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 254
	i64 u0xac5acae88f60357e, ; 766: System.Diagnostics.Tools.dll => 32
	i64 u0xac65e40f62b6b90e, ; 767: Google.Protobuf => 185
	i64 u0xac79c7e46047ad98, ; 768: System.Security.Principal.Windows.dll => 128
	i64 u0xac98d31068e24591, ; 769: System.Xml.XDocument => 159
	i64 u0xacd46e002c3ccb97, ; 770: ro/Microsoft.Maui.Controls.resources => 362
	i64 u0xacdd9e4180d56dda, ; 771: Xamarin.AndroidX.Concurrent.Futures => 233
	i64 u0xacf42eea7ef9cd12, ; 772: System.Threading.Channels => 140
	i64 u0xad89c07347f1bad6, ; 773: nl/Microsoft.Maui.Controls.resources.dll => 358
	i64 u0xadbb53caf78a79d2, ; 774: System.Web.HttpUtility => 153
	i64 u0xadc90ab061a9e6e4, ; 775: System.ComponentModel.TypeConverter.dll => 17
	i64 u0xadca1b9030b9317e, ; 776: Xamarin.AndroidX.Collection.Ktx => 232
	i64 u0xadd8eda2edf396ad, ; 777: Xamarin.Android.Glide.GifDecoder => 218
	i64 u0xadf4cf30debbeb9a, ; 778: System.Net.ServicePoint.dll => 75
	i64 u0xadf511667bef3595, ; 779: System.Net.Security => 74
	i64 u0xae0aaa94fdcfce0f, ; 780: System.ComponentModel.EventBasedAsync.dll => 15
	i64 u0xae282bcd03739de7, ; 781: Java.Interop => 169
	i64 u0xae53579c90db1107, ; 782: System.ObjectModel.dll => 85
	i64 u0xaeb080014622ef84, ; 783: Xamarin.JavaX.Inject => 324
	i64 u0xaec7c0c7e2ed4575, ; 784: lib_Xamarin.KotlinX.AtomicFU.Jvm.dll.so => 331
	i64 u0xaefb3ebdf2b6cb76, ; 785: Xamarin.Grpc.Util.dll => 322
	i64 u0xaf732d0b2193b8f5, ; 786: System.Security.Cryptography.OpenSsl.dll => 124
	i64 u0xafdb94dbccd9d11c, ; 787: Xamarin.AndroidX.Lifecycle.LiveData.dll => 253
	i64 u0xafe29f45095518e7, ; 788: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 264
	i64 u0xb03ae931fb25607e, ; 789: Xamarin.AndroidX.ConstraintLayout => 234
	i64 u0xb05cc42cd94c6d9d, ; 790: lib-sv-Microsoft.Maui.Controls.resources.dll.so => 365
	i64 u0xb0ac21bec8f428c5, ; 791: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android.dll => 260
	i64 u0xb0bb43dc52ea59f9, ; 792: System.Diagnostics.Tracing.dll => 34
	i64 u0xb1dd05401aa8ee63, ; 793: System.Security.AccessControl => 118
	i64 u0xb220631954820169, ; 794: System.Text.RegularExpressions => 139
	i64 u0xb2376e1dbf8b4ed7, ; 795: System.Security.Cryptography.Csp => 122
	i64 u0xb2a1959fe95c5402, ; 796: lib_System.Runtime.InteropServices.JavaScript.dll.so => 106
	i64 u0xb2a3f67f3bf29fce, ; 797: da/Microsoft.Maui.Controls.resources => 342
	i64 u0xb3005ac9c8a035c5, ; 798: lib_Xamarin.JavaX.Inject.dll.so => 324
	i64 u0xb3011a0a57f7ffb2, ; 799: Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll => 374
	i64 u0xb3874072ee0ecf8c, ; 800: Xamarin.AndroidX.VectorDrawable.Animated.dll => 283
	i64 u0xb39eed1decc0cd95, ; 801: Google.Api.Gax.dll => 176
	i64 u0xb3f0a0fcda8d3ebc, ; 802: Xamarin.AndroidX.CardView => 229
	i64 u0xb404462cdf3bffdb, ; 803: lib_Xamarin.Firebase.ProtoliteWellKnownTypes.dll.so => 299
	i64 u0xb4512edf6d2b372b, ; 804: Google.Cloud.Location => 183
	i64 u0xb46be1aa6d4fff93, ; 805: hi/Microsoft.Maui.Controls.resources => 349
	i64 u0xb477491be13109d8, ; 806: ar/Microsoft.Maui.Controls.resources => 339
	i64 u0xb4b4a8794e933f72, ; 807: lib_Plugin.Firebase.Firestore.dll.so => 209
	i64 u0xb4bd7015ecee9d86, ; 808: System.IO.Pipelines => 54
	i64 u0xb4c53d9749c5f226, ; 809: lib_System.IO.FileSystem.AccessControl.dll.so => 47
	i64 u0xb4f8071f1e28b58e, ; 810: Xamarin.Firebase.AppCheck.Interop.dll => 291
	i64 u0xb4ff710863453fda, ; 811: System.Diagnostics.FileVersionInfo.dll => 28
	i64 u0xb5c38bf497a4cfe2, ; 812: lib_System.Threading.Tasks.dll.so => 145
	i64 u0xb5c7fcdafbc67ee4, ; 813: Microsoft.Extensions.Logging.Abstractions.dll => 197
	i64 u0xb5e2ea1bb00704d6, ; 814: Xamarin.Kotlin.StdLib.Jdk7.dll => 328
	i64 u0xb5ea31d5244c6626, ; 815: System.Threading.ThreadPool.dll => 147
	i64 u0xb7212c4683a94afe, ; 816: System.Drawing.Primitives => 35
	i64 u0xb7b7753d1f319409, ; 817: sv/Microsoft.Maui.Controls.resources => 365
	i64 u0xb81a2c6e0aee50fe, ; 818: lib_System.Private.CoreLib.dll.so => 173
	i64 u0xb898d1802c1a108c, ; 819: lib_System.Management.dll.so => 214
	i64 u0xb8b0a9b3dfbc5cb7, ; 820: Xamarin.AndroidX.Window.Extensions.Core.Core => 288
	i64 u0xb8c60af47c08d4da, ; 821: System.Net.ServicePoint => 75
	i64 u0xb8e68d20aad91196, ; 822: lib_System.Xml.XPath.dll.so => 161
	i64 u0xb90ff82c284e9af9, ; 823: Grpc.Core.Api => 188
	i64 u0xb9185c33a1643eed, ; 824: Microsoft.CSharp.dll => 1
	i64 u0xb963484256d34b23, ; 825: BCrypt.Net-Core.dll => 174
	i64 u0xb97187307f9dba5e, ; 826: Xamarin.KotlinX.Coroutines.Play.Services.dll => 335
	i64 u0xb9b8001adf4ed7cc, ; 827: lib_Xamarin.AndroidX.SlidingPaneLayout.dll.so => 277
	i64 u0xb9f64d3b230def68, ; 828: lib-pt-Microsoft.Maui.Controls.resources.dll.so => 361
	i64 u0xb9fc3c8a556e3691, ; 829: ja/Microsoft.Maui.Controls.resources => 354
	i64 u0xba4670aa94a2b3c6, ; 830: lib_System.Xml.XDocument.dll.so => 159
	i64 u0xba48785529705af9, ; 831: System.Collections.dll => 12
	i64 u0xba965b8c86359996, ; 832: lib_System.Windows.dll.so => 155
	i64 u0xbb286883bc35db36, ; 833: System.Transactions.dll => 151
	i64 u0xbb6026d73f757bcf, ; 834: Google.Api.Gax.Grpc => 177
	i64 u0xbb65706fde942ce3, ; 835: System.Net.Sockets => 76
	i64 u0xbba28979413cad9e, ; 836: lib_System.Runtime.CompilerServices.VisualC.dll.so => 103
	i64 u0xbbd180354b67271a, ; 837: System.Runtime.Serialization.Formatters => 112
	i64 u0xbc0e640e7c6bcdf8, ; 838: Xamarin.Grpc.Api => 316
	i64 u0xbc260cdba33291a3, ; 839: Xamarin.AndroidX.Arch.Core.Common.dll => 226
	i64 u0xbc727ece861e474e, ; 840: lib_Square.OkIO.JVM.dll.so => 211
	i64 u0xbd0e2c0d55246576, ; 841: System.Net.Http.dll => 65
	i64 u0xbd3fbd85b9e1cb29, ; 842: lib_System.Net.HttpListener.dll.so => 66
	i64 u0xbd437a2cdb333d0d, ; 843: Xamarin.AndroidX.ViewPager2 => 286
	i64 u0xbd4f572d2bd0a789, ; 844: System.IO.Compression.ZipFile.dll => 45
	i64 u0xbd5d0b88d3d647a5, ; 845: lib_Xamarin.AndroidX.Browser.dll.so => 228
	i64 u0xbd877b14d0b56392, ; 846: System.Runtime.Intrinsics.dll => 109
	i64 u0xbe65a49036345cf4, ; 847: lib_System.Buffers.dll.so => 7
	i64 u0xbee38d4a88835966, ; 848: Xamarin.AndroidX.AppCompat.AppCompatResources => 225
	i64 u0xbef9919db45b4ca7, ; 849: System.IO.Pipes.AccessControl => 55
	i64 u0xbf0fa68611139208, ; 850: lib_Xamarin.AndroidX.Annotation.dll.so => 221
	i64 u0xbfc1e1fb3095f2b3, ; 851: lib_System.Net.Http.Json.dll.so => 64
	i64 u0xc040a4ab55817f58, ; 852: ar/Microsoft.Maui.Controls.resources.dll => 339
	i64 u0xc07cadab29efeba0, ; 853: Xamarin.AndroidX.Core.Core.Ktx.dll => 238
	i64 u0xc0d928351ab5ca77, ; 854: System.Console.dll => 20
	i64 u0xc0f5a221a9383aea, ; 855: System.Runtime.Intrinsics => 109
	i64 u0xc111030af54d7191, ; 856: System.Resources.Writer => 101
	i64 u0xc12b8b3afa48329c, ; 857: lib_System.Linq.dll.so => 62
	i64 u0xc1649f545b2f76aa, ; 858: Grpc.Auth => 187
	i64 u0xc183ca0b74453aa9, ; 859: lib_System.Threading.Tasks.Dataflow.dll.so => 142
	i64 u0xc1ff9ae3cdb6e1e6, ; 860: Xamarin.AndroidX.Activity.dll => 219
	i64 u0xc226d517e7e30388, ; 861: lib_Xamarin.Grpc.Stub.dll.so => 321
	i64 u0xc26c064effb1dea9, ; 862: System.Buffers.dll => 7
	i64 u0xc2850fbba221599d, ; 863: lib_Google.Apis.Core.dll.so => 180
	i64 u0xc28c50f32f81cc73, ; 864: ja/Microsoft.Maui.Controls.resources.dll => 354
	i64 u0xc2902f6cf5452577, ; 865: lib_Mono.Android.Export.dll.so => 170
	i64 u0xc2a3bca55b573141, ; 866: System.IO.FileSystem.Watcher => 50
	i64 u0xc2bcfec99f69365e, ; 867: Xamarin.AndroidX.ViewPager2.dll => 286
	i64 u0xc30b52815b58ac2c, ; 868: lib_System.Runtime.Serialization.Xml.dll.so => 115
	i64 u0xc36d7d89c652f455, ; 869: System.Threading.Overlapped => 141
	i64 u0xc396b285e59e5493, ; 870: GoogleGson.dll => 186
	i64 u0xc3c86c1e5e12f03d, ; 871: WindowsBase => 166
	i64 u0xc421b61fd853169d, ; 872: lib_System.Net.WebSockets.Client.dll.so => 80
	i64 u0xc463e077917aa21d, ; 873: System.Runtime.Serialization.Json => 113
	i64 u0xc4d3858ed4d08512, ; 874: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 264
	i64 u0xc50fded0ded1418c, ; 875: lib_System.ComponentModel.TypeConverter.dll.so => 17
	i64 u0xc519125d6bc8fb11, ; 876: lib_System.Net.Requests.dll.so => 73
	i64 u0xc5293b19e4dc230e, ; 877: Xamarin.AndroidX.Navigation.Fragment => 268
	i64 u0xc5325b2fcb37446f, ; 878: lib_System.Private.Xml.dll.so => 89
	i64 u0xc535cb9a21385d9b, ; 879: lib_Xamarin.Android.Glide.DiskLruCache.dll.so => 217
	i64 u0xc5a0f4b95a699af7, ; 880: lib_System.Private.Uri.dll.so => 87
	i64 u0xc5cdcd5b6277579e, ; 881: lib_System.Security.Cryptography.Algorithms.dll.so => 120
	i64 u0xc5d608afb58abba2, ; 882: Google.Apis.Auth.dll => 179
	i64 u0xc5ec286825cb0bf4, ; 883: Xamarin.AndroidX.Tracing.Tracing => 280
	i64 u0xc62af3e2d6d38289, ; 884: lib_Xamarin.Firebase.Firestore.dll.so => 298
	i64 u0xc6706bc8aa7fe265, ; 885: Xamarin.AndroidX.Annotation.Jvm => 223
	i64 u0xc7c01e7d7c93a110, ; 886: System.Text.Encoding.Extensions.dll => 135
	i64 u0xc7ce851898a4548e, ; 887: lib_System.Web.HttpUtility.dll.so => 153
	i64 u0xc809d4089d2556b2, ; 888: System.Runtime.InteropServices.JavaScript.dll => 106
	i64 u0xc858a28d9ee5a6c5, ; 889: lib_System.Collections.Specialized.dll.so => 11
	i64 u0xc8ac7c6bf1c2ec51, ; 890: System.Reflection.DispatchProxy.dll => 90
	i64 u0xc9c62c8f354ac568, ; 891: lib_System.Diagnostics.TextWriterTraceListener.dll.so => 31
	i64 u0xca3a723e7342c5b6, ; 892: lib-tr-Microsoft.Maui.Controls.resources.dll.so => 367
	i64 u0xca5801070d9fccfb, ; 893: System.Text.Encoding => 136
	i64 u0xcab3493c70141c2d, ; 894: pl/Microsoft.Maui.Controls.resources => 359
	i64 u0xcab69b9a31439815, ; 895: lib_Xamarin.Google.ErrorProne.TypeAnnotations.dll.so => 306
	i64 u0xcacfddc9f7c6de76, ; 896: ro/Microsoft.Maui.Controls.resources.dll => 362
	i64 u0xcadbc92899a777f0, ; 897: Xamarin.AndroidX.Startup.StartupRuntime => 278
	i64 u0xcba1cb79f45292b5, ; 898: Xamarin.Android.Glide.GifDecoder.dll => 218
	i64 u0xcbb5f80c7293e696, ; 899: lib_System.Globalization.Calendars.dll.so => 40
	i64 u0xcbd4fdd9cef4a294, ; 900: lib__Microsoft.Android.Resource.Designer.dll.so => 377
	i64 u0xcc15da1e07bbd994, ; 901: Xamarin.AndroidX.SlidingPaneLayout => 277
	i64 u0xcc182c3afdc374d6, ; 902: Microsoft.Bcl.AsyncInterfaces => 191
	i64 u0xcc2876b32ef2794c, ; 903: lib_System.Text.RegularExpressions.dll.so => 139
	i64 u0xcc5c3bb714c4561e, ; 904: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 334
	i64 u0xcc76886e09b88260, ; 905: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 337
	i64 u0xcc9fa2923aa1c9ef, ; 906: System.Diagnostics.Contracts.dll => 25
	i64 u0xccf25c4b634ccd3a, ; 907: zh-Hans/Microsoft.Maui.Controls.resources.dll => 371
	i64 u0xcd10a42808629144, ; 908: System.Net.Requests => 73
	i64 u0xcdca1b920e9f53ba, ; 909: Xamarin.AndroidX.Interpolator => 250
	i64 u0xcdd0c48b6937b21c, ; 910: Xamarin.AndroidX.SwipeRefreshLayout => 279
	i64 u0xcde1fa22dc303670, ; 911: Microsoft.VisualStudio.DesignTools.XamlTapContract => 376
	i64 u0xce9594d842cf35a9, ; 912: lib_Xamarin.Firebase.Common.Ktx.dll.so => 295
	i64 u0xcf1f7a2359f1a539, ; 913: Xamarin.JavaX.Inject.dll => 324
	i64 u0xcf23d8093f3ceadf, ; 914: System.Diagnostics.DiagnosticSource.dll => 27
	i64 u0xcf5ff6b6b2c4c382, ; 915: System.Net.Mail.dll => 67
	i64 u0xcf8fc898f98b0d34, ; 916: System.Private.Xml.Linq => 88
	i64 u0xd04b5f59ed596e31, ; 917: System.Reflection.Metadata.dll => 95
	i64 u0xd063299fcfc0c93f, ; 918: lib_System.Runtime.Serialization.Json.dll.so => 113
	i64 u0xd0de8a113e976700, ; 919: System.Diagnostics.TextWriterTraceListener => 31
	i64 u0xd0fc33d5ae5d4cb8, ; 920: System.Runtime.Extensions => 104
	i64 u0xd1194e1d8a8de83c, ; 921: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 252
	i64 u0xd12beacdfc14f696, ; 922: System.Dynamic.Runtime => 37
	i64 u0xd198e7ce1b6a8344, ; 923: System.Net.Quic.dll => 72
	i64 u0xd20588bdafd1c17c, ; 924: Xamarin.Grpc.Stub.dll => 321
	i64 u0xd3144156a3727ebe, ; 925: Xamarin.Google.Guava.ListenableFuture => 309
	i64 u0xd333d0af9e423810, ; 926: System.Runtime.InteropServices => 108
	i64 u0xd33a415cb4278969, ; 927: System.Security.Cryptography.Encoding.dll => 123
	i64 u0xd3426d966bb704f5, ; 928: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 225
	i64 u0xd3651b6fc3125825, ; 929: System.Private.Uri.dll => 87
	i64 u0xd373685349b1fe8b, ; 930: Microsoft.Extensions.Logging.dll => 196
	i64 u0xd3801faafafb7698, ; 931: System.Private.DataContractSerialization.dll => 86
	i64 u0xd3e4c8d6a2d5d470, ; 932: it/Microsoft.Maui.Controls.resources => 353
	i64 u0xd3edcc1f25459a50, ; 933: System.Reflection.Emit => 93
	i64 u0xd4645626dffec99d, ; 934: lib_Microsoft.Extensions.DependencyInjection.Abstractions.dll.so => 195
	i64 u0xd4fa0abb79079ea9, ; 935: System.Security.Principal.dll => 129
	i64 u0xd5507e11a2b2839f, ; 936: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 264
	i64 u0xd5d04bef8478ea19, ; 937: Xamarin.AndroidX.Tracing.Tracing.dll => 280
	i64 u0xd60815f26a12e140, ; 938: Microsoft.Extensions.Logging.Debug.dll => 198
	i64 u0xd64f50eb4ba264b3, ; 939: lib_Google.LongRunning.dll.so => 184
	i64 u0xd65786d27a4ad960, ; 940: lib_Microsoft.Maui.Controls.HotReload.Forms.dll.so => 373
	i64 u0xd6694f8359737e4e, ; 941: Xamarin.AndroidX.SavedState => 274
	i64 u0xd6949e129339eae5, ; 942: lib_Xamarin.AndroidX.Core.Core.Ktx.dll.so => 238
	i64 u0xd6d21782156bc35b, ; 943: Xamarin.AndroidX.SwipeRefreshLayout.dll => 279
	i64 u0xd6de019f6af72435, ; 944: Xamarin.AndroidX.ConstraintLayout.Core.dll => 235
	i64 u0xd6ed09ee80649430, ; 945: lib_Xamarin.Grpc.Core.dll.so => 318
	i64 u0xd6f697a581fc6fe3, ; 946: Xamarin.Google.ErrorProne.TypeAnnotations.dll => 306
	i64 u0xd70956d1e6deefb9, ; 947: Jsr305Binding => 303
	i64 u0xd72329819cbbbc44, ; 948: lib_Microsoft.Extensions.Configuration.Abstractions.dll.so => 193
	i64 u0xd72c760af136e863, ; 949: System.Xml.XmlSerializer.dll => 163
	i64 u0xd74883ff25771958, ; 950: Xamarin.Io.PerfMark.PerfMarkApi => 323
	i64 u0xd753f071e44c2a03, ; 951: lib_System.Security.SecureString.dll.so => 130
	i64 u0xd7b3764ada9d341d, ; 952: lib_Microsoft.Extensions.Logging.Abstractions.dll.so => 197
	i64 u0xd7dfd89d34e8dd1d, ; 953: Square.OkIO => 210
	i64 u0xd7f0088bc5ad71f2, ; 954: Xamarin.AndroidX.VersionedParcelable => 284
	i64 u0xd8113d9a7e8ad136, ; 955: System.CodeDom => 212
	i64 u0xd8fb25e28ae30a12, ; 956: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 271
	i64 u0xda1dfa4c534a9251, ; 957: Microsoft.Extensions.DependencyInjection => 194
	i64 u0xdab94026b007e005, ; 958: Xamarin.GooglePlayServices.Auth.Api.Phone.dll => 311
	i64 u0xdad05a11827959a3, ; 959: System.Collections.NonGeneric.dll => 10
	i64 u0xdaefdfe71aa53cf9, ; 960: System.IO.FileSystem.Primitives => 49
	i64 u0xdaff1e02a729f3a2, ; 961: Xamarin.Grpc.Android => 315
	i64 u0xdb5383ab5865c007, ; 962: lib-vi-Microsoft.Maui.Controls.resources.dll.so => 369
	i64 u0xdb58816721c02a59, ; 963: lib_System.Reflection.Emit.ILGeneration.dll.so => 91
	i64 u0xdbeda89f832aa805, ; 964: vi/Microsoft.Maui.Controls.resources.dll => 369
	i64 u0xdbf2a779fbc3ac31, ; 965: System.Transactions.Local.dll => 150
	i64 u0xdbf9607a441b4505, ; 966: System.Linq => 62
	i64 u0xdbfc90157a0de9b0, ; 967: lib_System.Text.Encoding.dll.so => 136
	i64 u0xdc75032002d1a212, ; 968: lib_System.Transactions.Local.dll.so => 150
	i64 u0xdca8be7403f92d4f, ; 969: lib_System.Linq.Queryable.dll.so => 61
	i64 u0xdcbd21904ff0f297, ; 970: Google.Apis => 178
	i64 u0xdce2c53525640bf3, ; 971: Microsoft.Extensions.Logging => 196
	i64 u0xdd2b722d78ef5f43, ; 972: System.Runtime.dll => 117
	i64 u0xdd67031857c72f96, ; 973: lib_System.Text.Encodings.Web.dll.so => 137
	i64 u0xdd70765ad6162057, ; 974: Xamarin.JSpecify => 326
	i64 u0xdd92e229ad292030, ; 975: System.Numerics.dll => 84
	i64 u0xdde30e6b77aa6f6c, ; 976: lib-zh-Hans-Microsoft.Maui.Controls.resources.dll.so => 371
	i64 u0xde110ae80fa7c2e2, ; 977: System.Xml.XDocument.dll => 159
	i64 u0xde4726fcdf63a198, ; 978: Xamarin.AndroidX.Transition => 281
	i64 u0xde572c2b2fb32f93, ; 979: lib_System.Threading.Tasks.Extensions.dll.so => 143
	i64 u0xde8769ebda7d8647, ; 980: hr/Microsoft.Maui.Controls.resources.dll => 350
	i64 u0xdea791ecb8d4fb73, ; 981: Xamarin.Google.Android.Play.Integrity.dll => 301
	i64 u0xdee075f3477ef6be, ; 982: Xamarin.AndroidX.ExifInterface.dll => 247
	i64 u0xdf4b773de8fb1540, ; 983: System.Net.dll => 82
	i64 u0xdfa254ebb4346068, ; 984: System.Net.Ping => 70
	i64 u0xe0142572c095a480, ; 985: Xamarin.AndroidX.AppCompat.dll => 224
	i64 u0xe021eaa401792a05, ; 986: System.Text.Encoding.dll => 136
	i64 u0xe02f89350ec78051, ; 987: Xamarin.AndroidX.CoordinatorLayout.dll => 236
	i64 u0xe0496b9d65ef5474, ; 988: Xamarin.Android.Glide.DiskLruCache.dll => 217
	i64 u0xe10b760bb1462e7a, ; 989: lib_System.Security.Cryptography.Primitives.dll.so => 125
	i64 u0xe1566bbdb759c5af, ; 990: Microsoft.Maui.Controls.HotReload.Forms.dll => 373
	i64 u0xe192a588d4410686, ; 991: lib_System.IO.Pipelines.dll.so => 54
	i64 u0xe1a08bd3fa539e0d, ; 992: System.Runtime.Loader => 110
	i64 u0xe1a77eb8831f7741, ; 993: System.Security.SecureString.dll => 130
	i64 u0xe1b52f9f816c70ef, ; 994: System.Private.Xml.Linq.dll => 88
	i64 u0xe1e199c8ab02e356, ; 995: System.Data.DataSetExtensions.dll => 23
	i64 u0xe1ecfdb7fff86067, ; 996: System.Net.Security.dll => 74
	i64 u0xe2252a80fe853de4, ; 997: lib_System.Security.Principal.dll.so => 129
	i64 u0xe22fa4c9c645db62, ; 998: System.Diagnostics.TextWriterTraceListener.dll => 31
	i64 u0xe2420585aeceb728, ; 999: System.Net.Requests.dll => 73
	i64 u0xe247baa54eab2a37, ; 1000: Xamarin.Firebase.Common.Ktx.dll => 295
	i64 u0xe26692647e6bcb62, ; 1001: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 259
	i64 u0xe2799d9c4661dbbc, ; 1002: Xamarin.GoogleAndroid.Annotations => 310
	i64 u0xe29b73bc11392966, ; 1003: lib-id-Microsoft.Maui.Controls.resources.dll.so => 352
	i64 u0xe2a295b75d36df94, ; 1004: Xamarin.Firebase.Auth.dll => 292
	i64 u0xe2ad448dee50fbdf, ; 1005: System.Xml.Serialization => 158
	i64 u0xe2d920f978f5d85c, ; 1006: System.Data.DataSetExtensions => 23
	i64 u0xe2e426c7714fa0bc, ; 1007: Microsoft.Win32.Primitives.dll => 4
	i64 u0xe332bacb3eb4a806, ; 1008: Mono.Android.Export.dll => 170
	i64 u0xe3811d68d4fe8463, ; 1009: pt-BR/Microsoft.Maui.Controls.resources.dll => 360
	i64 u0xe3b7cbae5ad66c75, ; 1010: lib_System.Security.Cryptography.Encoding.dll.so => 123
	i64 u0xe4292b48f3224d5b, ; 1011: lib_Xamarin.AndroidX.Core.ViewTree.dll.so => 239
	i64 u0xe494f7ced4ecd10a, ; 1012: hu/Microsoft.Maui.Controls.resources.dll => 351
	i64 u0xe49a982a2533a332, ; 1013: lib_Google.Cloud.Location.dll.so => 183
	i64 u0xe4a9b1e40d1e8917, ; 1014: lib-fi-Microsoft.Maui.Controls.resources.dll.so => 346
	i64 u0xe4f74a0b5bf9703f, ; 1015: System.Runtime.Serialization.Primitives => 114
	i64 u0xe5434e8a119ceb69, ; 1016: lib_Mono.Android.dll.so => 172
	i64 u0xe55703b9ce5c038a, ; 1017: System.Diagnostics.Tools => 32
	i64 u0xe57013c8afc270b5, ; 1018: Microsoft.VisualBasic => 3
	i64 u0xe62913cc36bc07ec, ; 1019: System.Xml.dll => 164
	i64 u0xe6e77c648688b75b, ; 1020: Google.Api.CommonProtos.dll => 175
	i64 u0xe7b0691bcbb5a85d, ; 1021: System.Linq.Async => 213
	i64 u0xe7bea09c4900a191, ; 1022: Xamarin.AndroidX.VectorDrawable.dll => 282
	i64 u0xe7e03cc18dcdeb49, ; 1023: lib_System.Diagnostics.StackTrace.dll.so => 30
	i64 u0xe7e147ff99a7a380, ; 1024: lib_System.Configuration.dll.so => 19
	i64 u0xe83ddbccfc6aff3f, ; 1025: Xamarin.Kotlin.StdLib.Jdk7 => 328
	i64 u0xe86b0df4ba9e5db8, ; 1026: lib_Xamarin.AndroidX.Lifecycle.Runtime.Android.dll.so => 258
	i64 u0xe896622fe0902957, ; 1027: System.Reflection.Emit.dll => 93
	i64 u0xe89a2a9ef110899b, ; 1028: System.Drawing.dll => 36
	i64 u0xe8c5f8c100b5934b, ; 1029: Microsoft.Win32.Registry => 5
	i64 u0xe8efe6c2171f7cd2, ; 1030: Xamarin.Google.Guava.dll => 307
	i64 u0xe93ca41931f1f2d0, ; 1031: Xamarin.Grpc.Api.dll => 316
	i64 u0xe957c3976986ab72, ; 1032: lib_Xamarin.AndroidX.Window.Extensions.Core.Core.dll.so => 288
	i64 u0xe98163eb702ae5c5, ; 1033: Xamarin.AndroidX.Arch.Core.Runtime => 227
	i64 u0xe98b0e4b4d44e931, ; 1034: lib_Grpc.Net.Client.dll.so => 189
	i64 u0xe994f23ba4c143e5, ; 1035: Xamarin.KotlinX.Coroutines.Android => 332
	i64 u0xe9b9c8c0458fd92a, ; 1036: System.Windows => 155
	i64 u0xe9d166d87a7f2bdb, ; 1037: lib_Xamarin.AndroidX.Startup.StartupRuntime.dll.so => 278
	i64 u0xea5a4efc2ad81d1b, ; 1038: Xamarin.Google.ErrorProne.Annotations => 305
	i64 u0xeaf8e9970fc2fe69, ; 1039: System.Management => 214
	i64 u0xeb2313fe9d65b785, ; 1040: Xamarin.AndroidX.ConstraintLayout.dll => 234
	i64 u0xeb6e275e78cb8d42, ; 1041: Xamarin.AndroidX.LocalBroadcastManager.dll => 266
	i64 u0xeb9973cda26e858f, ; 1042: Xamarin.Firebase.Common.dll => 294
	i64 u0xebbcc2bc51cb4a56, ; 1043: Plugin.Firebase.Firestore.dll => 209
	i64 u0xed19c616b3fcb7eb, ; 1044: Xamarin.AndroidX.VersionedParcelable.dll => 284
	i64 u0xed60c6fa891c051a, ; 1045: lib_Microsoft.VisualStudio.DesignTools.TapContract.dll.so => 375
	i64 u0xedc4817167106c23, ; 1046: System.Net.Sockets.dll => 76
	i64 u0xedc632067fb20ff3, ; 1047: System.Memory.dll => 63
	i64 u0xedc8e4ca71a02a8b, ; 1048: Xamarin.AndroidX.Navigation.Runtime.dll => 269
	i64 u0xee81f5b3f1c4f83b, ; 1049: System.Threading.ThreadPool => 147
	i64 u0xeeb7ebb80150501b, ; 1050: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 231
	i64 u0xeefc635595ef57f0, ; 1051: System.Security.Cryptography.Cng => 121
	i64 u0xef03b1b5a04e9709, ; 1052: System.Text.Encoding.CodePages.dll => 134
	i64 u0xef602c523fe2e87a, ; 1053: lib_Xamarin.Google.Guava.ListenableFuture.dll.so => 309
	i64 u0xef72742e1bcca27a, ; 1054: Microsoft.Maui.Essentials.dll => 204
	i64 u0xefd1e0c4e5c9b371, ; 1055: System.Resources.ResourceManager.dll => 100
	i64 u0xefe8f8d5ed3c72ea, ; 1056: System.Formats.Tar.dll => 39
	i64 u0xefec0b7fdc57ec42, ; 1057: Xamarin.AndroidX.Activity => 219
	i64 u0xf008bcd238ede2c8, ; 1058: System.CodeDom.dll => 212
	i64 u0xf00c29406ea45e19, ; 1059: es/Microsoft.Maui.Controls.resources.dll => 345
	i64 u0xf09e47b6ae914f6e, ; 1060: System.Net.NameResolution => 68
	i64 u0xf0ac2b489fed2e35, ; 1061: lib_System.Diagnostics.Debug.dll.so => 26
	i64 u0xf0bb49dadd3a1fe1, ; 1062: lib_System.Net.ServicePoint.dll.so => 75
	i64 u0xf0de2537ee19c6ca, ; 1063: lib_System.Net.WebHeaderCollection.dll.so => 78
	i64 u0xf1138779fa181c68, ; 1064: lib_Xamarin.AndroidX.Lifecycle.Runtime.dll.so => 257
	i64 u0xf11b621fc87b983f, ; 1065: Microsoft.Maui.Controls.Xaml.dll => 202
	i64 u0xf161f4f3c3b7e62c, ; 1066: System.Data => 24
	i64 u0xf16eb650d5a464bc, ; 1067: System.ValueTuple => 152
	i64 u0xf1c4b4005493d871, ; 1068: System.Formats.Asn1.dll => 38
	i64 u0xf238bd79489d3a96, ; 1069: lib-nl-Microsoft.Maui.Controls.resources.dll.so => 358
	i64 u0xf2a69492c6bd46b0, ; 1070: lib_Xamarin.Kotlin.StdLib.Jdk7.dll.so => 328
	i64 u0xf2feea356ba760af, ; 1071: Xamarin.AndroidX.Arch.Core.Runtime.dll => 227
	i64 u0xf300e085f8acd238, ; 1072: lib_System.ServiceProcess.dll.so => 133
	i64 u0xf34e52b26e7e059d, ; 1073: System.Runtime.CompilerServices.VisualC.dll => 103
	i64 u0xf37221fda4ef8830, ; 1074: lib_Xamarin.Google.Android.Material.dll.so => 300
	i64 u0xf3ad9b8fb3eefd12, ; 1075: lib_System.IO.UnmanagedMemoryStream.dll.so => 57
	i64 u0xf3ddfe05336abf29, ; 1076: System => 165
	i64 u0xf408654b2a135055, ; 1077: System.Reflection.Emit.ILGeneration.dll => 91
	i64 u0xf4103170a1de5bd0, ; 1078: System.Linq.Queryable.dll => 61
	i64 u0xf42d20c23173d77c, ; 1079: lib_System.ServiceModel.Web.dll.so => 132
	i64 u0xf483be3bba89b4ff, ; 1080: lib_Xamarin.Grpc.Api.dll.so => 316
	i64 u0xf4c1dd70a5496a17, ; 1081: System.IO.Compression => 46
	i64 u0xf4ecf4b9afc64781, ; 1082: System.ServiceProcess.dll => 133
	i64 u0xf4eeeaa566e9b970, ; 1083: lib_Xamarin.AndroidX.CustomView.PoolingContainer.dll.so => 242
	i64 u0xf518f63ead11fcd1, ; 1084: System.Threading.Tasks => 145
	i64 u0xf5fc7602fe27b333, ; 1085: System.Net.WebHeaderCollection => 78
	i64 u0xf6077741019d7428, ; 1086: Xamarin.AndroidX.CoordinatorLayout => 236
	i64 u0xf6742cbf457c450b, ; 1087: Xamarin.AndroidX.Lifecycle.Runtime.Android.dll => 258
	i64 u0xf70c0a7bf8ccf5af, ; 1088: System.Web => 154
	i64 u0xf73b506af603bfe1, ; 1089: lib_Square.OkIO.dll.so => 210
	i64 u0xf77b20923f07c667, ; 1090: de/Microsoft.Maui.Controls.resources.dll => 343
	i64 u0xf7e2cac4c45067b3, ; 1091: lib_System.Numerics.Vectors.dll.so => 83
	i64 u0xf7e74930e0e3d214, ; 1092: zh-HK/Microsoft.Maui.Controls.resources.dll => 370
	i64 u0xf7fa0bf77fe677cc, ; 1093: Newtonsoft.Json.dll => 206
	i64 u0xf84773b5c81e3cef, ; 1094: lib-uk-Microsoft.Maui.Controls.resources.dll.so => 368
	i64 u0xf8aac5ea82de1348, ; 1095: System.Linq.Queryable => 61
	i64 u0xf8b77539b362d3ba, ; 1096: lib_System.Reflection.Primitives.dll.so => 96
	i64 u0xf8dacc6dd9573437, ; 1097: Square.OkIO.dll => 210
	i64 u0xf8e045dc345b2ea3, ; 1098: lib_Xamarin.AndroidX.RecyclerView.dll.so => 272
	i64 u0xf915dc29808193a1, ; 1099: System.Web.HttpUtility.dll => 153
	i64 u0xf96c777a2a0686f4, ; 1100: hi/Microsoft.Maui.Controls.resources.dll => 349
	i64 u0xf9be54c8bcf8ff3b, ; 1101: System.Security.AccessControl.dll => 118
	i64 u0xf9eec5bb3a6aedc6, ; 1102: Microsoft.Extensions.Options => 199
	i64 u0xfa0e82300e67f913, ; 1103: lib_System.AppContext.dll.so => 6
	i64 u0xfa2fdb27e8a2c8e8, ; 1104: System.ComponentModel.EventBasedAsync => 15
	i64 u0xfa3f278f288b0e84, ; 1105: lib_System.Net.Security.dll.so => 74
	i64 u0xfa5ed7226d978949, ; 1106: lib-ar-Microsoft.Maui.Controls.resources.dll.so => 339
	i64 u0xfa645d91e9fc4cba, ; 1107: System.Threading.Thread => 146
	i64 u0xfad4d2c770e827f9, ; 1108: lib_System.IO.IsolatedStorage.dll.so => 52
	i64 u0xfb06dd2338e6f7c4, ; 1109: System.Net.Ping.dll => 70
	i64 u0xfb087abe5365e3b7, ; 1110: lib_System.Data.DataSetExtensions.dll.so => 23
	i64 u0xfb846e949baff5ea, ; 1111: System.Xml.Serialization.dll => 158
	i64 u0xfbad3e4ce4b98145, ; 1112: System.Security.Cryptography.X509Certificates => 126
	i64 u0xfbf0a31c9fc34bc4, ; 1113: lib_System.Net.Http.dll.so => 65
	i64 u0xfc61ddcf78dd1f54, ; 1114: Xamarin.AndroidX.LocalBroadcastManager => 266
	i64 u0xfc6b7527cc280b3f, ; 1115: lib_System.Runtime.Serialization.Formatters.dll.so => 112
	i64 u0xfc719aec26adf9d9, ; 1116: Xamarin.AndroidX.Navigation.Fragment.dll => 268
	i64 u0xfc82690c2fe2735c, ; 1117: Xamarin.AndroidX.Lifecycle.Process.dll => 256
	i64 u0xfc93fc307d279893, ; 1118: System.IO.Pipes.AccessControl.dll => 55
	i64 u0xfcd302092ada6328, ; 1119: System.IO.MemoryMappedFiles.dll => 53
	i64 u0xfd22f00870e40ae0, ; 1120: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 243
	i64 u0xfd3ce7bc9232d417, ; 1121: Xamarin.Firebase.Firestore.dll => 298
	i64 u0xfd49b3c1a76e2748, ; 1122: System.Runtime.InteropServices.RuntimeInformation => 107
	i64 u0xfd536c702f64dc47, ; 1123: System.Text.Encoding.Extensions => 135
	i64 u0xfd583f7657b6a1cb, ; 1124: Xamarin.AndroidX.Fragment => 248
	i64 u0xfd8dd91a2c26bd5d, ; 1125: Xamarin.AndroidX.Lifecycle.Runtime => 257
	i64 u0xfda36abccf05cf5c, ; 1126: System.Net.WebSockets.Client => 80
	i64 u0xfddbe9695626a7f5, ; 1127: Xamarin.AndroidX.Lifecycle.Common => 251
	i64 u0xfeae9952cf03b8cb, ; 1128: tr/Microsoft.Maui.Controls.resources => 367
	i64 u0xfebe1950717515f9, ; 1129: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 255
	i64 u0xff270a55858bac8d, ; 1130: System.Security.Principal => 129
	i64 u0xff9b54613e0d2cc8, ; 1131: System.Net.Http.Json => 64
	i64 u0xffb5607c2db1b7e8, ; 1132: Xamarin.Kotlin.StdLib.Jdk8 => 329
	i64 u0xffdb7a971be4ec73 ; 1133: System.ValueTuple.dll => 152
], align 8

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [1134 x i32] [
	i32 182, i32 42, i32 333, i32 279, i32 13, i32 189, i32 269, i32 105,
	i32 171, i32 48, i32 224, i32 7, i32 86, i32 363, i32 341, i32 369,
	i32 244, i32 317, i32 71, i32 314, i32 272, i32 12, i32 203, i32 102,
	i32 370, i32 156, i32 19, i32 249, i32 231, i32 161, i32 246, i32 282,
	i32 167, i32 363, i32 10, i32 198, i32 283, i32 96, i32 242, i32 243,
	i32 13, i32 199, i32 10, i32 313, i32 127, i32 296, i32 95, i32 140,
	i32 39, i32 364, i32 337, i32 285, i32 212, i32 360, i32 172, i32 218,
	i32 5, i32 204, i32 67, i32 276, i32 130, i32 191, i32 188, i32 275,
	i32 245, i32 68, i32 232, i32 66, i32 57, i32 191, i32 241, i32 52,
	i32 43, i32 125, i32 67, i32 81, i32 259, i32 375, i32 158, i32 92,
	i32 99, i32 272, i32 141, i32 0, i32 182, i32 151, i32 228, i32 347,
	i32 162, i32 169, i32 348, i32 290, i32 289, i32 195, i32 81, i32 302,
	i32 375, i32 326, i32 232, i32 4, i32 5, i32 51, i32 101, i32 321,
	i32 56, i32 120, i32 98, i32 168, i32 118, i32 333, i32 21, i32 351,
	i32 137, i32 97, i32 337, i32 77, i32 357, i32 278, i32 119, i32 213,
	i32 8, i32 165, i32 366, i32 70, i32 217, i32 260, i32 273, i32 310,
	i32 171, i32 179, i32 145, i32 40, i32 289, i32 276, i32 47, i32 185,
	i32 30, i32 308, i32 270, i32 355, i32 144, i32 199, i32 163, i32 28,
	i32 84, i32 280, i32 77, i32 43, i32 176, i32 29, i32 42, i32 103,
	i32 0, i32 117, i32 320, i32 222, i32 45, i32 91, i32 366, i32 56,
	i32 148, i32 374, i32 313, i32 146, i32 100, i32 49, i32 176, i32 208,
	i32 20, i32 237, i32 114, i32 297, i32 187, i32 179, i32 215, i32 347,
	i32 304, i32 301, i32 208, i32 327, i32 200, i32 298, i32 94, i32 58,
	i32 352, i32 350, i32 81, i32 304, i32 169, i32 26, i32 315, i32 329,
	i32 71, i32 271, i32 247, i32 373, i32 368, i32 69, i32 33, i32 346,
	i32 14, i32 139, i32 38, i32 372, i32 233, i32 359, i32 134, i32 92,
	i32 88, i32 149, i32 308, i32 365, i32 24, i32 297, i32 138, i32 57,
	i32 51, i32 344, i32 29, i32 157, i32 34, i32 164, i32 297, i32 248,
	i32 52, i32 377, i32 287, i32 90, i32 306, i32 229, i32 35, i32 347,
	i32 157, i32 9, i32 345, i32 183, i32 76, i32 318, i32 55, i32 203,
	i32 341, i32 201, i32 13, i32 292, i32 286, i32 192, i32 226, i32 109,
	i32 320, i32 263, i32 32, i32 296, i32 104, i32 84, i32 92, i32 53,
	i32 96, i32 325, i32 177, i32 58, i32 9, i32 102, i32 241, i32 68,
	i32 285, i32 340, i32 206, i32 125, i32 273, i32 116, i32 135, i32 126,
	i32 106, i32 184, i32 327, i32 131, i32 319, i32 293, i32 228, i32 309,
	i32 147, i32 156, i32 249, i32 237, i32 185, i32 244, i32 184, i32 174,
	i32 273, i32 97, i32 24, i32 277, i32 143, i32 267, i32 3, i32 177,
	i32 167, i32 225, i32 100, i32 161, i32 181, i32 99, i32 175, i32 239,
	i32 25, i32 214, i32 93, i32 168, i32 172, i32 220, i32 3, i32 359,
	i32 246, i32 1, i32 114, i32 327, i32 249, i32 256, i32 178, i32 33,
	i32 6, i32 363, i32 291, i32 156, i32 190, i32 335, i32 361, i32 53,
	i32 311, i32 85, i32 294, i32 284, i32 270, i32 44, i32 255, i32 104,
	i32 47, i32 138, i32 211, i32 64, i32 265, i32 69, i32 80, i32 59,
	i32 89, i32 154, i32 226, i32 133, i32 110, i32 353, i32 265, i32 271,
	i32 171, i32 134, i32 140, i32 40, i32 340, i32 315, i32 201, i32 190,
	i32 60, i32 262, i32 293, i32 79, i32 25, i32 36, i32 302, i32 99,
	i32 259, i32 71, i32 190, i32 22, i32 237, i32 205, i32 364, i32 121,
	i32 69, i32 107, i32 370, i32 266, i32 119, i32 117, i32 251, i32 180,
	i32 252, i32 11, i32 2, i32 124, i32 115, i32 142, i32 301, i32 41,
	i32 87, i32 308, i32 221, i32 173, i32 27, i32 148, i32 354, i32 194,
	i32 305, i32 220, i32 1, i32 187, i32 222, i32 322, i32 319, i32 44,
	i32 236, i32 149, i32 18, i32 307, i32 86, i32 342, i32 312, i32 41,
	i32 255, i32 230, i32 329, i32 260, i32 94, i32 196, i32 28, i32 41,
	i32 78, i32 245, i32 233, i32 144, i32 108, i32 231, i32 11, i32 105,
	i32 137, i32 16, i32 122, i32 66, i32 157, i32 22, i32 344, i32 336,
	i32 102, i32 194, i32 334, i32 63, i32 58, i32 202, i32 343, i32 110,
	i32 173, i32 293, i32 307, i32 376, i32 332, i32 9, i32 300, i32 120,
	i32 98, i32 105, i32 335, i32 263, i32 201, i32 111, i32 223, i32 49,
	i32 20, i32 262, i32 240, i32 72, i32 235, i32 155, i32 39, i32 342,
	i32 35, i32 330, i32 38, i32 348, i32 288, i32 108, i32 311, i32 357,
	i32 21, i32 178, i32 325, i32 261, i32 211, i32 205, i32 15, i32 200,
	i32 79, i32 79, i32 240, i32 200, i32 268, i32 275, i32 152, i32 21,
	i32 203, i32 341, i32 50, i32 51, i32 318, i32 367, i32 357, i32 94,
	i32 216, i32 338, i32 353, i32 16, i32 207, i32 239, i32 123, i32 350,
	i32 160, i32 45, i32 305, i32 186, i32 116, i32 63, i32 166, i32 192,
	i32 14, i32 274, i32 111, i32 223, i32 60, i32 331, i32 121, i32 356,
	i32 2, i32 366, i32 294, i32 248, i32 261, i32 330, i32 326, i32 261,
	i32 6, i32 230, i32 346, i32 244, i32 209, i32 289, i32 17, i32 364,
	i32 343, i32 77, i32 0, i32 234, i32 313, i32 131, i32 325, i32 356,
	i32 83, i32 198, i32 12, i32 34, i32 119, i32 336, i32 256, i32 319,
	i32 246, i32 85, i32 215, i32 314, i32 18, i32 285, i32 193, i32 254,
	i32 72, i32 374, i32 95, i32 165, i32 250, i32 182, i32 82, i32 372,
	i32 291, i32 224, i32 229, i32 331, i32 154, i32 36, i32 151, i32 368,
	i32 371, i32 144, i32 56, i32 113, i32 230, i32 282, i32 317, i32 281,
	i32 302, i32 37, i32 372, i32 192, i32 115, i32 222, i32 14, i32 216,
	i32 320, i32 299, i32 146, i32 43, i32 207, i32 189, i32 204, i32 220,
	i32 290, i32 98, i32 334, i32 168, i32 16, i32 48, i32 107, i32 97,
	i32 181, i32 265, i32 27, i32 128, i32 29, i32 348, i32 299, i32 275,
	i32 128, i32 44, i32 240, i32 245, i32 149, i32 8, i32 188, i32 206,
	i32 267, i32 349, i32 362, i32 314, i32 361, i32 132, i32 360, i32 174,
	i32 42, i32 336, i32 317, i32 338, i32 33, i32 377, i32 46, i32 143,
	i32 262, i32 202, i32 253, i32 292, i32 241, i32 138, i32 62, i32 132,
	i32 340, i32 48, i32 160, i32 296, i32 227, i32 253, i32 216, i32 251,
	i32 356, i32 281, i32 46, i32 164, i32 250, i32 345, i32 247, i32 180,
	i32 352, i32 323, i32 205, i32 312, i32 18, i32 8, i32 290, i32 186,
	i32 238, i32 124, i32 59, i32 141, i32 269, i32 355, i32 257, i32 303,
	i32 208, i32 287, i32 150, i32 142, i32 338, i32 333, i32 330, i32 126,
	i32 332, i32 160, i32 162, i32 242, i32 219, i32 193, i32 358, i32 26,
	i32 267, i32 254, i32 82, i32 287, i32 127, i32 304, i32 101, i32 322,
	i32 148, i32 300, i32 270, i32 54, i32 323, i32 162, i32 167, i32 131,
	i32 37, i32 283, i32 355, i32 22, i32 112, i32 90, i32 50, i32 60,
	i32 122, i32 83, i32 127, i32 213, i32 163, i32 303, i32 166, i32 274,
	i32 276, i32 243, i32 215, i32 258, i32 4, i32 252, i32 351, i32 170,
	i32 2, i32 312, i32 263, i32 116, i32 376, i32 221, i32 310, i32 175,
	i32 19, i32 197, i32 295, i32 89, i32 65, i32 207, i32 30, i32 195,
	i32 344, i32 181, i32 235, i32 59, i32 111, i32 254, i32 32, i32 185,
	i32 128, i32 159, i32 362, i32 233, i32 140, i32 358, i32 153, i32 17,
	i32 232, i32 218, i32 75, i32 74, i32 15, i32 169, i32 85, i32 324,
	i32 331, i32 322, i32 124, i32 253, i32 264, i32 234, i32 365, i32 260,
	i32 34, i32 118, i32 139, i32 122, i32 106, i32 342, i32 324, i32 374,
	i32 283, i32 176, i32 229, i32 299, i32 183, i32 349, i32 339, i32 209,
	i32 54, i32 47, i32 291, i32 28, i32 145, i32 197, i32 328, i32 147,
	i32 35, i32 365, i32 173, i32 214, i32 288, i32 75, i32 161, i32 188,
	i32 1, i32 174, i32 335, i32 277, i32 361, i32 354, i32 159, i32 12,
	i32 155, i32 151, i32 177, i32 76, i32 103, i32 112, i32 316, i32 226,
	i32 211, i32 65, i32 66, i32 286, i32 45, i32 228, i32 109, i32 7,
	i32 225, i32 55, i32 221, i32 64, i32 339, i32 238, i32 20, i32 109,
	i32 101, i32 62, i32 187, i32 142, i32 219, i32 321, i32 7, i32 180,
	i32 354, i32 170, i32 50, i32 286, i32 115, i32 141, i32 186, i32 166,
	i32 80, i32 113, i32 264, i32 17, i32 73, i32 268, i32 89, i32 217,
	i32 87, i32 120, i32 179, i32 280, i32 298, i32 223, i32 135, i32 153,
	i32 106, i32 11, i32 90, i32 31, i32 367, i32 136, i32 359, i32 306,
	i32 362, i32 278, i32 218, i32 40, i32 377, i32 277, i32 191, i32 139,
	i32 334, i32 337, i32 25, i32 371, i32 73, i32 250, i32 279, i32 376,
	i32 295, i32 324, i32 27, i32 67, i32 88, i32 95, i32 113, i32 31,
	i32 104, i32 252, i32 37, i32 72, i32 321, i32 309, i32 108, i32 123,
	i32 225, i32 87, i32 196, i32 86, i32 353, i32 93, i32 195, i32 129,
	i32 264, i32 280, i32 198, i32 184, i32 373, i32 274, i32 238, i32 279,
	i32 235, i32 318, i32 306, i32 303, i32 193, i32 163, i32 323, i32 130,
	i32 197, i32 210, i32 284, i32 212, i32 271, i32 194, i32 311, i32 10,
	i32 49, i32 315, i32 369, i32 91, i32 369, i32 150, i32 62, i32 136,
	i32 150, i32 61, i32 178, i32 196, i32 117, i32 137, i32 326, i32 84,
	i32 371, i32 159, i32 281, i32 143, i32 350, i32 301, i32 247, i32 82,
	i32 70, i32 224, i32 136, i32 236, i32 217, i32 125, i32 373, i32 54,
	i32 110, i32 130, i32 88, i32 23, i32 74, i32 129, i32 31, i32 73,
	i32 295, i32 259, i32 310, i32 352, i32 292, i32 158, i32 23, i32 4,
	i32 170, i32 360, i32 123, i32 239, i32 351, i32 183, i32 346, i32 114,
	i32 172, i32 32, i32 3, i32 164, i32 175, i32 213, i32 282, i32 30,
	i32 19, i32 328, i32 258, i32 93, i32 36, i32 5, i32 307, i32 316,
	i32 288, i32 227, i32 189, i32 332, i32 155, i32 278, i32 305, i32 214,
	i32 234, i32 266, i32 294, i32 209, i32 284, i32 375, i32 76, i32 63,
	i32 269, i32 147, i32 231, i32 121, i32 134, i32 309, i32 204, i32 100,
	i32 39, i32 219, i32 212, i32 345, i32 68, i32 26, i32 75, i32 78,
	i32 257, i32 202, i32 24, i32 152, i32 38, i32 358, i32 328, i32 227,
	i32 133, i32 103, i32 300, i32 57, i32 165, i32 91, i32 61, i32 132,
	i32 316, i32 46, i32 133, i32 242, i32 145, i32 78, i32 236, i32 258,
	i32 154, i32 210, i32 343, i32 83, i32 370, i32 206, i32 368, i32 61,
	i32 96, i32 210, i32 272, i32 153, i32 349, i32 118, i32 199, i32 6,
	i32 15, i32 74, i32 339, i32 146, i32 52, i32 70, i32 23, i32 158,
	i32 126, i32 65, i32 266, i32 112, i32 268, i32 256, i32 55, i32 53,
	i32 243, i32 298, i32 107, i32 135, i32 248, i32 257, i32 80, i32 251,
	i32 367, i32 255, i32 129, i32 64, i32 329, i32 152
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 u0x0000000000000000, ; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

; Functions

; Function attributes: memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" }

; Metadata
!llvm.module.flags = !{!0, !1, !7, !8, !9, !10}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!".NET for Android remotes/origin/release/9.0.1xx @ 9abff7703206541fdb83ffa80fe2c2753ad1997b"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"branch-target-enforcement", i32 0}
!8 = !{i32 1, !"sign-return-address", i32 0}
!9 = !{i32 1, !"sign-return-address-all", i32 0}
!10 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
