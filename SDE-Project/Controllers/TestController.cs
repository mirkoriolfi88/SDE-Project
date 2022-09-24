using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SDE_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet(Name = "GetTestValue")]
        public string Get()
        {
            return "Test new controller";
        }
    }
}
