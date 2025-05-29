using UPSWSWebAPIs.Models;

namespace UPSWSWebAPIs.DataLayer.Interfaces
{
    public interface IJWTAuthTokenRepository
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
