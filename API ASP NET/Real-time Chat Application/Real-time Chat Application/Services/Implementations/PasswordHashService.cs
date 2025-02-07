using Real_time_Chat_Application.Data.Entity;
using Real_time_Chat_Application.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Real_time_Chat_Application.Services.Implementations
{
    public class PasswordHashService : IPasswordHashService
    {
        public User HashPassword(string userName, string email, string password)
        {
            using var hmac = new HMACSHA512();

            var user = new User
            {
                UserName = userName,
                Email = email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            return user;
        }

        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);

            var computePassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computePassword.Length; i++)
            {
                if(computePassword[i] != passwordHash[i]) return false;
            }

            return true;
        }
    }
}
