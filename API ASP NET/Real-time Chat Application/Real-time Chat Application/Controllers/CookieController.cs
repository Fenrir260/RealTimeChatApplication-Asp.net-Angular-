using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Real_time_Chat_Application.Models;
using Real_time_Chat_Application.Services.Interfaces;

namespace Real_time_Chat_Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CookieController : Controller
    {
        private readonly ICookieService _cookieService;

        private readonly ITokenService _tokenService;

        public CookieController(ICookieService cookieService, ITokenService tokenService)
        {
            _cookieService = cookieService;
            _tokenService = tokenService;
        }

        [HttpGet("getEncodedTokenFromCookieByKey/{tokenKey}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetEncodedTokenFromCookie([FromRoute] string tokenKey)
        {
            var encodedToken = _cookieService.GetCookieToken(tokenKey);

            return Ok($"{encodedToken}");
        }

        [HttpGet("getDecodedTokenFromCookieByKey/{tokenKey}")]
        [Authorize]
        public async Task<ActionResult> GetDecodedTokenFromCookie([FromRoute] string tokenKey)
        {
            
            var token = _cookieService.GetCookieToken(tokenKey);

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { error = "Token not found in cookies" });
            }

            var decodedToken = await _tokenService.DecodeToken(token);

            return Ok(decodedToken);
        }

        [HttpPost("setCookie")]
        [AllowAnonymous]
        public async Task<ActionResult> SetCookie([FromQuery] string token)
        {
            var response = _cookieService.SetCookieToken("jwtToken", token, 7);
            return Ok(response);
        }

        [HttpPost("getToken")]
        [AllowAnonymous]
        public async Task<ActionResult> GetToken([FromBody] UserDTO user)
        {
            var token = _tokenService.CreateToken(user.UserName, user.Email, user.Authorized, 7);
            return Ok(token);
        }




    }
}
