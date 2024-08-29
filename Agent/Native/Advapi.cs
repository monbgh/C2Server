using System;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Principal;


namespace Agent.Native
{
    public static  class Advapi
    {
        [DllImport("advapi32.dll")]
        public static extern bool LogonUserA(
        string IpszUsername,
        string IpszDomain,
        string IpszPassword,
        LegonProvider dwLogonType,
        LogonUserProvider dwLogonProvider,
        ref IntPtr phToken);



        [DllImport("advapi32.dll")]
        public static extern bool OpenProcessToken(
            IntPtr ProcessHandle,
            DesiredAccess desiredAccess,
            out IntPtr TokenHandle);



        [DllImport("advapi32.dll")]
        public extern static bool DuplicateTokenEx(
            IntPtr hExistingToken,
            TokenAccessLevels dwDesiredAcces,
            ref SECURITY_ATTRIBUTES IpTokenAttributes,
            SecurityImpersonationLevel ImpersonationLevel,
            TokenBindingType TokenType,
            out IntPtr phNewtoken);





        [DllImport("advapi32.dll ", SetLastError = true)]


        public static extern bool ImpersonateLoggedOnUser(IntPtr hTokern);

        [DllImport("advapi32.dll ", SetLastError = true)]
        public static extern bool RevetToSelf();

        public enum LegonProvider
        {
            LOGON32_LOGON_BATCH =4,
            LOGON32_LOGON_INTERACTIVE =2,
            LOGON32_LOGON_NETWORK=3,
            LOGON32_LOGON_NETWORK_CLEARTEXT=8,
            LOGON32_LOGON_NEW_CREDENTIALS=9,
            LOGON32_LOGON_SERVICE=5,
            LOGON32_LOGON_UNLOCK=7

        }

        public enum LogonUserProvider
        {
            LOGON32_PROVIDER_DEFAULT=0,
            LOGON32_PROVIDER_WINNT35 = 1,
            LOGON32_PROVIDER_WINNT50 =3,
            LOGON32_PROVIDER_WINNT40 =2,
            LOGON32_PROVIDER_VIRTUAL =4

        }
    }
}
