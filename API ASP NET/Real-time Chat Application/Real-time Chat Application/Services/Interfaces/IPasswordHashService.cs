using Real_time_Chat_Application.Data.Entity;

namespace Real_time_Chat_Application.Services.Interfaces
{
    public interface IPasswordHashService
    {
        User HashPassword(string userName, string email, string password);
        bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
