using Real_time_Chat_Application.Models;

namespace Real_time_Chat_Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendVerificationMessageAsync(UserRegisterDTO data);
    }
}
