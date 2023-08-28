
namespace MQTTManager.Services
{
    public interface IAuthorizationConfService
    {
        string HashPassword(string password);
    }
}
