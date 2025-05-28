using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using UPSWSIntegrationINWAPIs.Models;

namespace UPSWSIntegrationINWAPIs.BusniessLayer.Interfaces
{ 
    public interface IJWTAuthTokenService
    {
        public string GenerateToken(string username);
        public Task<string> GetTokenAsync(string userId, string userPassword);

        public void SaveTokenData(string userId, TokenData tokenData);

        public TokenData GetStoredTokenData(string userId);
        public bool ValidateToken(string incomingToken, TokenData storedTokenData);
        public Task<string> GetSecureDataAsync(string token);

        public Task<string> GetSecureDataAsOutPut();

    }
}
