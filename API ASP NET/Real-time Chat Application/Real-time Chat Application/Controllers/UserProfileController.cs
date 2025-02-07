using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Real_time_Chat_Application.Filters;
using Real_time_Chat_Application.Models;
using Real_time_Chat_Application.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Real_time_Chat_Application.Controllers
{
    [ApiController]
    [ValidateToken]
    [Authorize]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileController(IWebHostEnvironment env, IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        [HttpGet("getUserProfileData/{userName}")]
        public async Task<ActionResult> GetUserProfileData([FromRoute] string userName)
        {
            var data = await _userProfileRepository.GetUserProfileDataAsync(userName);
            return Ok(data);
        }
        
        [HttpPut("saveUserProfile")]
        public async Task<ActionResult> SaveUserProfile([FromBody] UserProfileDTO userProfile)
        {
            try
            {
                if (userProfile == null)
                {
                    return BadRequest("user profile is null");
                }
                await _userProfileRepository.SaveUserProfileSettingsAsync(userProfile);
                return Ok(new { message = "Profile saved successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("uploadPhoto")]
        public async Task<ActionResult> UploadPhoto([FromForm] PhotoUploadDTO photo)
        {
            try
            {
                await _userProfileRepository.SaveUserProfilePhotoAsync(photo);
                return Ok(new { message = "photo uploaded successful" });
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message); 
            }
        }

        [HttpGet("getPhotoByUserName/{userName}")]
        public async Task<IActionResult> GetPhotoByUserName(string userName)
        {
            try
            {
                var photo = await _userProfileRepository.GetUserProfilePhotoAsync(userName);

                string mimeType = "image/jpeg"; 
                if (!string.IsNullOrEmpty(photo.FileName))
                {
                    mimeType = photo.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ? "image/png" : "image/jpeg";
                }

                var memoryStream = new MemoryStream();
                await photo.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                return File(memoryStream, mimeType, photo.FileName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Сталася помилка.", details = ex.Message });
            }
        }


    }
}
