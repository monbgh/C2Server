using Microsoft.AspNetCore.Mvc;
namespace TeamServer.Models
{

    [Controller]
    public class HttpListenerConroller : ControllerBase
    {
        public IActionResult HandleImplant()
        {
            return Ok("Your listener works");
        }

    }
}
