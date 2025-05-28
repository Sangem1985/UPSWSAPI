namespace UPSWSWebAPIs.BusniessLayer.Interfaces
{
    public interface IAuthService
    {
        public string GenerateToken(string username);
    }
}
