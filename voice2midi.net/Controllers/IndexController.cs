using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace voice2midiAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        [HttpGet]
        public IActionResult status()
        {
            string state = "running";
            return Ok(new { state });
        }
    }
}
