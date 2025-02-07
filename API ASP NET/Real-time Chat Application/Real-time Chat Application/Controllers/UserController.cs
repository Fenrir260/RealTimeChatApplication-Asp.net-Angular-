using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Real_time_Chat_Application.Data.Entity;
using Real_time_Chat_Application.Filters;
using Real_time_Chat_Application.Models;
using Real_time_Chat_Application.Repositories.Interfaces;
using Real_time_Chat_Application.Services.Interfaces;

namespace Real_time_Chat_Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IEmailService _emailService;

        private readonly IUserRepository _userRepository;

        private readonly ITokenService _tokenService;

        private readonly ICookieService _cookieService;

        private readonly IUserService _userService;

        public UserController(IUserRepository userRepository, IEmailService emailService, IUserService userService, ITokenService tokenService, ICookieService cookieService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _userService = userService;
            _tokenService = tokenService;
            _cookieService = cookieService;
        }

        [HttpPost("registerUser")]
        public async Task<ActionResult<string>> RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            
            try
            {
                await _userService.AddUserAsync(userRegisterDTO);

                string token = _tokenService.CreateToken(userRegisterDTO.UserName, userRegisterDTO.Email, false, 7);  
                
                await _emailService.SendVerificationMessageAsync(userRegisterDTO);

                await _cookieService.SetCookieToken("jwtToken", token, 7);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("loginUser")]
        public async Task<ActionResult> LoginUser(UserLoginDTO userLoginDTO)
        {
            try
            {
                var userLoginned = await _userService.UserLoginAsync(userLoginDTO);

                var token = _tokenService.CreateToken(userLoginned.UserName, userLoginned.Email, userLoginned.Authorized, 7);

                await _cookieService.SetCookieToken("jwtToken", token, 7);
                
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getIdByUserName/{userName}")]
        [Authorize]
        public async Task<ActionResult<int>> GetIdByUserName([FromRoute] string userName)
        {
            try 
            {
                int id = await _userRepository.GetIdByUserNameAsync(userName);
                return Ok(id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpGet("getUsers")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("getUserByUserName/{userName}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUserByUserName([FromRoute] string userName)
        {
            var user = await _userRepository.GetUserByUserNameAsync(userName);
            if(user == null)
            {
                return BadRequest("User is not exist");
            }
            return Ok(user);
        }

        [HttpPost("setUserAuthorize")]
        [Authorize]
        public async Task<ActionResult> SetUserAuthorize([FromBody] string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return BadRequest(new
                    {
                        message = "userName is required",
                        status = 400,
                        errors = new { userName = "userName cannot be null or empty" }
                    });
                }

                await _userRepository.SetUserAuthorizedAsync(userName);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    status = 400,
                    errors = ex.StackTrace
                });
            }

            return Ok("The user has been authorized");
        }

        [Authorize]
        [HttpGet("getUserByEmail/{email}")]
        public async Task<ActionResult<UserDTO?>> GetUserByEmail([FromRoute] string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(error: ex.Message);
            }
        }


        [HttpDelete("deleteUserByUserName")]
        [Authorize]
        public async Task<ActionResult> DeleteUserByUserName(string userName)
        {
            try
            {
                await _userRepository.DeleteUserByUserNameAsync(userName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
    }
}
