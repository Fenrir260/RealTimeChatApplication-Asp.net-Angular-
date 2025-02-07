using Real_time_Chat_Application.Data.Entity;
using Real_time_Chat_Application.Models;

namespace Real_time_Chat_Application.Services.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(UserRegisterDTO userRegisterDTO);
        Task<UserDTO> UserLoginAsync(UserLoginDTO userLoginDTO);
    }
}
