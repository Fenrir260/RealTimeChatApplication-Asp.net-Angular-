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
    [Authorize]
    [ValidateToken]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {

        private readonly IUserRepository _userRepository;

        private readonly IFriendshipService _friendshipService;

        public FriendsController(IUserRepository userRepository, IFriendshipService friendshipService)
        {
            _userRepository = userRepository;
            _friendshipService = friendshipService;
        }

        [HttpPost("sendFriendRequest")]
        public async Task<ActionResult> SendFriendRequest([FromBody] UserDTO toUser)
        {
            try
            {
                await _friendshipService.SendFriendRequestAsync(toUser);
                return Ok(new { message = "Request was send successful."});
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = $"{ex.Message}" });
            }
        }

        [HttpPut("acceptFriendRequest")]
        public async Task<ActionResult> AcceptFriendRequest([FromBody] UserDTO toUser)
        {
            try
            {
                await _friendshipService.AcceptFriendRequestAsync(toUser);
                return Ok(new {message = "Friend request accepted."});
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = $"{ex.Message}" }); ;
            }
        }

        [HttpDelete("declineFriendRequest")]
        public async Task<ActionResult> DeclineFriendRequest([FromBody] UserDTO toUser)
        {
            try
            {
                await _friendshipService.DeclineFriendRequestAsync(toUser);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = $"{ex.Message}" });
            }
        }

        [HttpGet("getMyFriendsList")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetMyFriendsList()
        {
            try
            {
                var list = await _friendshipService.GetMyFriendsListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = $"{ex.Message}" });
            }
        }

        [HttpGet("getFriendsRequestList")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetFriendsRequestList()
        {
            try
            {
                var list = await _friendshipService.GetFriendsRequestListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = $"{ex.Message}" });
            }
        }

        [HttpGet("getSendFriendsRequestList")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetSendFriendsRequestList()
        {
            try
            {
                var list = await _friendshipService.GetSendFriendsRequestListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = $"{ex.Message}" });
            }
        }

        [HttpGet("findUserByUsersName/{userName}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> FindUsersByUserName([FromRoute] string userName)
        {
            try
            {
                var users = await _friendshipService.SearchUsersByUserNameOrProfileAsync(userName);

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = $"{ex.Message}" });
            }
        }

        [HttpPost("cancelFriendRequest")]
        public async Task<ActionResult> CancelFriendRequest([FromBody] UserDTO toUser)
        {
            try
            {
                await _friendshipService.CancelFriendRequestAsync(toUser);
                return Ok(new { message = "The request was successfully canceled." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { errrorMessage = $"{ex.Message}" });
            }
        }
    }
}
