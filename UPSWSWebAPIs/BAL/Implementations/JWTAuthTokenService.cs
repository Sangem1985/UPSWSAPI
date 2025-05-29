using UPSWSWebAPIs.Models;
using UPSWSWebAPIs.BusniessLayer.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using UPSWSWebAPIs.DataLayer.Interfaces;
using UPSWSWebAPIs.DataLayer.Implementations;
using Dapper;
using Npgsql;
using System.Data;

namespace UPSWSWebAPIs.BusniessLayer.Implementations
{
    
    public class JWTAuthTokenService : IJWTAuthTokenService
    {
      
        private readonly IJWTAuthTokenRepository? _jWTAuthTokenRepository;

        public JWTAuthTokenService(IJWTAuthTokenRepository? jWTAuthTokenRepository)
        {
            _jWTAuthTokenRepository = jWTAuthTokenRepository;
        }

        public string GenerateToken(string username)
        {
           return _jWTAuthTokenRepository.GenerateToken(username);
        }

        public Task<string> GetSecureDataAsync(string token)
        {
            return _jWTAuthTokenRepository.GetSecureDataAsync(token);
        }

        public TokenData GetStoredTokenData(string userId)
        {
            return _jWTAuthTokenRepository.GetStoredTokenData(userId);
        }

        public Task<string> GetTokenAsync(string userId, string userPassword)
        {
            return _jWTAuthTokenRepository.GetTokenAsync(userId, userPassword);
        }

        public void SaveTokenData(string userId, TokenData tokenData)
        {
             _jWTAuthTokenRepository.SaveTokenData(userId, tokenData);
        }

        public bool ValidateToken(string incomingToken, TokenData storedTokenData)
        {
            return _jWTAuthTokenRepository.ValidateToken(incomingToken, storedTokenData);
        }

        public async Task<string> GetSecureDataAsOutPut()
        {
            return await _jWTAuthTokenRepository.GetSecureDataAsOutPut();

        }
    }

}
