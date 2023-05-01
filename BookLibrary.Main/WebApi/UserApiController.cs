using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Main.WebApi
{
    [Route("api/user")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        [HttpGet("name")]
        public ActionResult<string?> Get()
        {
            return Ok(User.Identity?.Name);
        }
    }
}
