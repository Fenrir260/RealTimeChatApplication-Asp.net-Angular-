using Microsoft.EntityFrameworkCore;
using Real_time_Chat_Application.Data.ContextDb;
using Real_time_Chat_Application.Data.Entity;
using Real_time_Chat_Application.Models;
using Real_time_Chat_Application.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Real_time_Chat_Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ContextDB _contextDb;

        private readonly ITokenService _tokenService;

        private readonly IPasswordHashService _passwordHashService;

        public UserService(ContextDB ContextDb, ITokenService tokenService, IPasswordHashService passwordHashService)
        {
            _contextDb = ContextDb;
            _tokenService = tokenService;
            _passwordHashService = passwordHashService;
        }

        public async Task AddUserAsync(UserRegisterDTO userRegisterDTO)
        {
            var user = await _contextDb.UsersDB.FirstOrDefaultAsync(u => u.UserName == userRegisterDTO.UserName || u.Email == userRegisterDTO.Email);

            if(user != null)
            {
                throw new Exception($"User with User Name {user.UserName} or email {user.Email} already exist");
            }

            var newUser = _passwordHashService.HashPassword(userRegisterDTO.UserName, userRegisterDTO.Email, userRegisterDTO.Password);

            await _contextDb.UsersDB.AddAsync(newUser);
            await _contextDb.SaveChangesAsync();
        }

        public async Task<UserDTO> UserLoginAsync(UserLoginDTO userLoginDTO)
        {
            try
            {
                var user = await _contextDb.UsersDB.SingleOrDefaultAsync(u => u.Email == userLoginDTO.Email);

                if (user == null)
                {
                    throw new Exception("User is not exist in Database");
                }

                if (user.Authorized == false)
                {
                    throw new Exception("User is not confirm email");
                }

                if(!_passwordHashService.VerifyPassword(userLoginDTO.Password, user.PasswordHash, user.PasswordSalt))
                {
                    throw new Exception("Invalid password");
                }

                return new UserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Authorized = user.Authorized,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
