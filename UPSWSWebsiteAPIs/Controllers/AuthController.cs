using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPSWSWebsiteAPIs.Models;
using UPSWSWebsiteAPIs.BusniessLayer.Interfaces;
namespace UPSWSWebsiteAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] LoginRequest loginRequest)
        {


            if (loginRequest.UserId == "admin" && loginRequest.UserPassword == "password")
            {
                var token = _authService.GenerateToken(loginRequest.UserId);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }

}
