using Real_time_Chat_Application.Data.Entity;
using Real_time_Chat_Application.Models;

namespace Real_time_Chat_Application.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO?> GetUserByUserNameAsync(string userName);
        Task<UserDTO?> GetUserByEmailAsync(string email);
        Task<int> GetIdByUserNameAsync(string userName);
        Task SetUserAuthorizedAsync(string userName);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task DeleteUserByUserNameAsync(string userName);
        Task<string[]> FindUsersByUserNamesAsync(string userName);
        Task<bool> UserExistsAsync(UserDTO userDTO);
    }
}
