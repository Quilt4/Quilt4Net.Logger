using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.DataTransfer
{
    public class LoginResult : ILoginResult
    {
        internal LoginResult()
        {
        }

        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string userName { get; set; }

        /*
        
        {"access_token":"OSdATuQraBso_flOPC8Fm8zxZ5b7kt8b5pA4_fXmPFzuv8QyVd-0U5b-xc-4UWUUZRnzpn5GvBjnsXwsD7diRj3SiJfc3K4Dk8ok4Z6ewpQ82GDybg1sRtGxdM4c3AIXcdxMrNK107HjR6nvDU2hNKDxAR3kjMwrNNPuLN-17V6zqLJvhmVLkEDVpGqPW-f7xqqA2THC2cU1ObXz4FICq_nsQUwHeZbbackvwAwnMwFHkrqv1HwX0VoTvfEl3GYs_wTPreTCB0aAi3-32P4GaZd8jkl8SEdmmRxr6o7GrcMFsaZLDT4IZnigvDBAigC3IAHzCGQSzNhTcZAsmK-zMWv_sXJJ0U2h-Mv8fpPc5PYn948zM52yUPsT2cvW3nXDDJWM5JZ3_m_IS1_grDfj_mPccq243gFzaJVBPGXuoN0","token_type":"bearer","expires_in":1209599,"userName":"Daniel",".issued":"Mon, 28 Dec 2015 07:40:17 GMT",".expires":"Mon, 11 Jan 2016 07:40:17 GMT"}

        */
    }
}