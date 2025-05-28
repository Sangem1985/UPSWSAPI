using Microsoft.AspNetCore.Mvc;

namespace UPSWSIntegrationINWAPIs.BusniessLayer.Interfaces
{
    public interface ILoginService
    {
        public Task<string> GetToken(string userId, string userPassword);
        public Task<string> GetSecureData([FromQuery] string token);
        public Task<string> GetSecureDataAsOutPut();

    }
}
