using Microsoft.AspNetCore.Mvc;

namespace Pazuru.Presentation.Web.BackEnd.Controller
{
    [Route("api/[controller]/[action]")]
    public class PuzzleController : ControllerBase
    {
        [HttpGet]
        public IActionResult Generate()
        {
            return new JsonResult("{ message \"ayyy\" }");
        }
    }
}
