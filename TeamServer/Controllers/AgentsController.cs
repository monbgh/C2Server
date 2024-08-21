using ApiModels.Requests;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using TeamServer.Models.Agents;
using TeamServer.Models.Listeners;
using TeamServer.Services;

namespace TeamServer.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]

    public class AgentsController : ControllerBase
    {
        public readonly IAgentService _agents;
        public AgentsController(IAgentService agents)
        {

            _agents = agents;
        }
        [HttpGet]
        public IActionResult GetAgents()
        {
            var agents = _agents.GetAgents();
            return Ok(agents);
        }
        [HttpGet("{name}")]
        public IActionResult GetAgent(string name)
        {
            var agent = _agents.GetAgent(name);
            if (agent is null) return NotFound();

            return Ok(agent);

        }
        [HttpPost("{agentId}")]
        public IActionResult TaskAgent(string agentId, [FromBody] TaskAgentRequest request)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null) return NotFound();
            var task = new AgentTask()
            {
                Id = Guid.NewGuid().ToString(),
                Command = request.Command,
                Arguments = request.Arguments,
                File = request.File,

            };

            agent.QueueTask(task);

            var root = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            var path = $"{root}/{listener.Name}";
        }
    }
}
