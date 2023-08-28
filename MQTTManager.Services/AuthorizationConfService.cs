using Microsoft.Extensions.Configuration;
using MQTTManager.Services;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MQTTManager.Services
{
    public class AuthorizationConfService : IAuthorizationConfService
    {
        //private readonly IConfiguration _configuration;

        //public AuthorizationConfService(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        public AuthorizationConfService()
        {
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Konwertuj string do tablicy bajtów i oblicz hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Przekształć tablicę bajtów na string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}