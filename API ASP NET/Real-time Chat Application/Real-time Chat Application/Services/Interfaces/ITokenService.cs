using Real_time_Chat_Application.Models;

namespace Real_time_Chat_Application.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(string userName, string email, bool authorized, int expires);
        Task <Dictionary<string, object>> DecodeToken(string token);
    }
}
