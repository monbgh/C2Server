﻿using Agent.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Commands
{
     public class PrintWorkingDirectory : AgentsCommand 
    {
        public override string Name => "pwd";

        public override string Execute(AgentTask task)
        {
            return Directory.GetCurrentDirectory();


        }


    }
}
