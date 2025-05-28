using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using UPSWSWebAPIs.BusniessLayer.Implementations;
using UPSWSWebAPIs.BusniessLayer.Interfaces;
using UPSWSWebAPIs.Models;
namespace UPSWSWebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {


        private readonly ILoginService _loginService; 
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] LoginRequest loginRequest)
        {

            return Ok(await _loginService.GetToken(loginRequest.UserId, loginRequest.UserPassword));

        }

        [HttpGet("GetSecureData")]
        public async Task<IActionResult> GetSecureData([FromQuery] string token)
        {
            return  Ok(await _loginService.GetSecureData(token));
        }

        [HttpGet("GetSecureDataAsOutPut")]
        public  async Task<string> GetSecureDataAsOutPut()
        {
            var data  = await _loginService.GetSecureDataAsOutPut();

            return data;

        }



    }




    public class MyUsers
    {
       public string? UserId { get; set; }
       public string? Password { get; set; }

        public MyUsers() { }

        public MyUsers(string userId, string password)
        {
            UserId = userId;
            Password = password;
        }
    }
}
