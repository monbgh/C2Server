using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TeamServer.Models;
using TeamServer.Services;
using TeamServer.Models.Listeners;
using ApiModels.Requests;

namespace TeamServer.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ListenersController : ControllerBase

    {
        private readonly IListenerService _listeners;

        private readonly IAgentService _agentService;

        public ListenersController(IListenerService listeners)
        {
            _listeners = listeners;
        }

        [HttpGet]
        public ActionResult GetListeners()
        {
            var listeners = _listeners.GetListener;
            return Ok(listeners);
        }
        [HttpGet("{name}")]
        public IActionResult GetListener(string name)
        {
            var listeners = _listeners.GetListener(name);
            if (listeners is null) return NotFound();
            return Ok(listeners);
        }

        [HttpPost]
        public IActionResult StartListener([FromBody] StartHttpListenerRequest request)
        {

            var listener = new HttpListener(request.Name, request.BindPort);

            listener.Init();
            listener.Start();

            _listeners.AddListener(listener);

            var root = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            var path = $"{root}/{listener.Name}";
            return Created(path, listener);
        }

    }
}