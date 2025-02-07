using Microsoft.EntityFrameworkCore;
using Real_time_Chat_Application.Data.ContextDb;
using Real_time_Chat_Application.Data.Entity;
using Real_time_Chat_Application.Models;
using Real_time_Chat_Application.Repositories.Interfaces;
using Real_time_Chat_Application.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Real_time_Chat_Application.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ContextDB _contextDb;

        private readonly ITokenService _tokenService;

        private readonly IPasswordHashService _passwordHashService;

        public UserRepository(ContextDB contextDB, ITokenService tokenService, IPasswordHashService passwordHashService)
        {
            _contextDb = contextDB;
            _tokenService = tokenService;
            _passwordHashService = passwordHashService;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _contextDb.UsersDB.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _contextDb.UsersDB.FindAsync(id);
        }

        public async Task<User?> GetUserByNameAsync(string userName)
        {
            return await _contextDb.UsersDB.SingleOrDefaultAsync(u => u.UserName == userName);
        }


        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            var user = await _contextDb.UsersDB.SingleOrDefaultAsync(e => e.Email == email);

            if (user == null)
            {
                throw new Exception($"User with email: {email}, does not exist.");
            }

            return new UserDTO()
            {
                UserName = user.UserName,
                Email = user.Email,
                Authorized = user.Authorized,
            };
        }

        public async Task<UserDTO?> GetUserByUserNameAsync(string userName)
        {
            var user = await _contextDb.UsersDB.SingleOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                throw new Exception($"User with User name: {userName}, does not exist.");
            }

            return new UserDTO()
            {
                UserName = user.UserName,
                Email = user.Email,
                Authorized = user.Authorized,
            };
        }

        public async Task SetUserAuthorizedAsync(string userName)
        {
            var user = await _contextDb.UsersDB.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                throw new Exception($"User with User name: {userName}, does not exist.");
            }

            user.Authorized = true;

            _contextDb.Update(user);
            await _contextDb.SaveChangesAsync();
        }

        public async Task DeleteUserByUserNameAsync(string userName)
        {
            var user = await _contextDb.UsersDB.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                throw new Exception($"User with User Name: {userName} is not exist.");
            }

            _contextDb.UsersDB.Remove(user);
            await _contextDb.SaveChangesAsync();
        }

        public async Task<string[]> FindUsersByUserNamesAsync(string userName)
        {
            var users = await _contextDb.UsersDB.Where(u => EF.Functions.Like(u.UserName, $"%{userName}%")).ToListAsync();

            var usersNames = new string[users.Count];

            for (int i = 0; i < users.Count; i++)
            {
                usersNames[i] = users[i].UserName;
            }

            return usersNames;
        }

        public async Task<int> GetIdByUserNameAsync(string userName)
        {
            var id = await _contextDb.UsersDB.SingleAsync(u => u.UserName == userName);

            if(id == null)
            {
                throw new Exception($"There is no id with UserName: {userName}.");
            }

            return id.Id;
        }

        public async Task<bool> UserExistsAsync(UserDTO userDTO)
        {
            var user = await _contextDb.UsersDB.Where(u => u.UserName == userDTO.UserName && u.Email == userDTO.Email && userDTO.Authorized == true).FirstOrDefaultAsync();

            if(user == null)
            {
                return false;
            }

            return true;
        }
    }
}
 