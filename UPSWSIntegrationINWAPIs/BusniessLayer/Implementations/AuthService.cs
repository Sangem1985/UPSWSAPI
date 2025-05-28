using UPSWSIntegrationINWAPIs.BusniessLayer.Interfaces;

namespace UPSWSIntegrationINWAPIs.BusniessLayer.Implementations
{
    public class AuthService : IAuthService
    {

        private readonly IJWTAuthTokenService _jWTAuthTokenService;
        public AuthService(IJWTAuthTokenService jWTAuthTokenService)
        {
            _jWTAuthTokenService = jWTAuthTokenService;
        }
        public string GenerateToken(string username)
        {
            return _jWTAuthTokenService.GenerateToken(username);
        }
    }
}
