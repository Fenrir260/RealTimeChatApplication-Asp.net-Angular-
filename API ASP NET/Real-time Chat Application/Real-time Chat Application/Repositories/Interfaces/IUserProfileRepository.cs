using Real_time_Chat_Application.Models;

namespace Real_time_Chat_Application.Repositories.Interfaces
{
    public interface IUserProfileRepository
    {
        Task SaveUserProfileSettingsAsync(UserProfileDTO userSettings);
        Task SaveUserProfilePhotoAsync(PhotoUploadDTO photo);
        Task<FormFile> GetUserProfilePhotoAsync(string userName);
        Task<UserProfileDTO> GetUserProfileDataAsync(string userName);
    }
}
