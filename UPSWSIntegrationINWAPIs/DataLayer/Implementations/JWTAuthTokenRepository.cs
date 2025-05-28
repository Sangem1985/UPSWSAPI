using UPSWSIntegrationINWAPIs.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using UPSWSIntegrationINWAPIs.DataLayer.Interfaces;

namespace UPSWSIntegrationINWAPIs.DataLayer.Implementations
{
    public class JWTAuthTokenRepository: IJWTAuthTokenRepository
    {

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private string? _token;
        private DateTime _tokenExpiry;
        private string? _baseUrl = "";
        private readonly IMemoryCache _memoryCache;
        public JWTAuthTokenRepository(IConfiguration configuration, HttpClient httpClient, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _baseUrl = _configuration["ApiBaseUrl:BaseUrl"];
        }
        public string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GetTokenAsync(string userId, string userPassword)
        {
            // Return cached token if still valid
            if (!string.IsNullOrEmpty(_token) && DateTime.Now < _tokenExpiry)
            {
                return _token;
            }
            // Request a new token
            var loginData = new
            {
                userId,
                userPassword
            };
            var response = await _httpClient.PostAsync(_baseUrl + "auth/Authenticate/",
                new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve token.");
            }

            var result = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(result);

            _token = json.RootElement.GetProperty("token").GetString();
            _tokenExpiry = DateTime.Now.AddHours(1);
            var tokenDataToSave = new TokenData { UserId = userId, _Token = _token!, _Expiry = _tokenExpiry };
            SaveTokenData(userId, tokenDataToSave);
            return _token!;
        }
        public void SaveTokenData(string userId, TokenData tokenData)
        {
            _memoryCache.Set(userId, tokenData, (DateTimeOffset)tokenData._Expiry);
        }
        public TokenData GetStoredTokenData(string userId)
        {
            // Retrieve TokenData from the cache
            _memoryCache.TryGetValue(userId, out TokenData tokenData);
            return tokenData;
        }


        public bool ValidateToken(string incomingToken, TokenData storedTokenData)
        {
            if (storedTokenData is null)
            {
                return false;
            }
            if (storedTokenData._Token != incomingToken)
            {
                Console.WriteLine("Token mismatch.");
                return false;
            }
            if (DateTime.UtcNow > storedTokenData._Expiry)
            {
                Console.WriteLine("Token has expired.");
                return false;
            }
            return true;
        }
        public async Task<string> GetSecureDataAsync(string token)
        {
            try
            {
                var tokenData = GetStoredTokenData("admin");
                if (!ValidateToken(token, tokenData))
                {
                    return "401";
                }
                if (tokenData is null || !tokenData._Token.Equals(token))
                {
                    return "401";
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(_baseUrl + "Login/GetSecureDataAsOutPut");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return "401";// "Unauthorized request.";
                }



            }
            catch (Exception ex)
            {
                return "400";// Unauthorized request.";
            }


        }

        public async Task<string> GetSecureDataAsOutPut()
        {

            var myUsers = new LoginRequest()
            {
                UserId = "pr",
                UserPassword = "chauhan"
            };
            var data = JsonSerializer.Serialize(myUsers);
            return await Task.FromResult(data);

        }
    }
}
