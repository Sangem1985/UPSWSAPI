namespace UPSWSIntegrationINWAPIs.BusniessLayer.Interfaces
{
    public interface IAuthService
    {
        public string GenerateToken(string username);
    }
}
