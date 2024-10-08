﻿using ApiModels.Requests;
using TeamServer.Models;
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






        [HttpGet("{agentId}/tasks")]
        public IActionResult GetTaskResults(string agentId)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null) return NotFound("Agent not found ");

            var results = agent.GetTaskResults();
            return Ok(results);

        }





        [HttpGet("{agentId}/tasks/{taskId}")]
        public IActionResult GetTaskResult(string agentId , string taskId)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null) return NotFound("Agent not found ");

            var result =agent.GetTaskResult(taskId);
            if (result is null) return NotFound("Task Not found");
            return Ok(result);
 
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
                File = request.File

            };

            agent.QueueTask(task);

            var root = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            var path = $"{root}/tasks/{task.Id}";

            return Created(path, path);
        }
    }
}
