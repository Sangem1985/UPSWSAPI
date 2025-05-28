using UPSWSIntegrationINWAPIs.BusniessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UPSWSIntegrationINWAPIs.BusniessLayer.Implementations
{
    public class LoginService : ILoginService
    {
        private readonly IJWTAuthTokenService _jWTAuthTokenService;

        public LoginService(IJWTAuthTokenService jWTAuthTokenService)
        {
            _jWTAuthTokenService = jWTAuthTokenService;
        }
        public async Task<string> GetSecureData([FromQuery] string token)
        {
            return await _jWTAuthTokenService.GetSecureDataAsync(token);
        }

        public async Task<string> GetToken(string userId, string userPassword)
        {
            return await _jWTAuthTokenService.GetTokenAsync(userId, userPassword);
        }

        public async Task<string> GetSecureDataAsOutPut()
        {
            return await _jWTAuthTokenService.GetSecureDataAsOutPut();
        }


    }
}
