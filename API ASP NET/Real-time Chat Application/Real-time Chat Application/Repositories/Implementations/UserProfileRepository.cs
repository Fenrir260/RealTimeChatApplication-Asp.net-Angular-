using Microsoft.EntityFrameworkCore;
using Real_time_Chat_Application.Data.ContextDb;
using Real_time_Chat_Application.Data.Entity;
using Real_time_Chat_Application.Models;
using Real_time_Chat_Application.Repositories.Interfaces;

namespace Real_time_Chat_Application.Repositories.Implementations
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ContextDB _contextDB;
        
        private readonly string folderPath = @$"C:\Programs\Course (Azure)\UserProfilePhotos";

        private readonly string defaultAvatarPath = @"C:\Programs\Course (Azure)\UserProfilePhotos\defaultAvatar.jpg";

        public UserProfileRepository(ContextDB contextDB)
        {
            _contextDB = contextDB;
        }

        public async Task SaveUserProfileSettingsAsync(UserProfileDTO userSettings)
        {
            try
            {
                var _userSettings = await _contextDB.UserProfilesDB.AsNoTracking()
                                           .FirstOrDefaultAsync(u => u.UserId == userSettings.UserId);

                if (_userSettings == null)
                {
                    _userSettings = new UserProfile
                    {
                        UserId = userSettings.UserId,
                        FirstName = userSettings.FirstName,
                        LastName = userSettings.LastName,
                        AboutMe = userSettings.AboutMe,
                        Age = userSettings.Age,
                        Gender = userSettings.Gender,
                        InterestedIn = userSettings.InterestedIn,
                        Instagram = userSettings.Instagram,
                        ShowInstagram = userSettings.ShowInstagram,
                        Telegram = userSettings.Telegram,
                        ShowTelegram = userSettings.ShowTelegram,
                        PhotoPath = defaultAvatarPath,
                        UserName = userSettings.UserName,
                        Id = userSettings.UserId
                    };

                    await _contextDB.UserProfilesDB.AddAsync(_userSettings);
                }
                else
                {
                    var existingEntity = _contextDB.ChangeTracker.Entries<UserProfile>()
                                                  .FirstOrDefault(e => e.Entity.UserId == userSettings.UserId);

                    if (existingEntity != null)
                    {
                        _contextDB.Entry(existingEntity.Entity).State = EntityState.Detached;
                    }

                    _userSettings.FirstName = userSettings.FirstName;
                    _userSettings.LastName = userSettings.LastName;
                    _userSettings.AboutMe = userSettings.AboutMe;
                    _userSettings.Age = userSettings.Age;
                    _userSettings.Gender = userSettings.Gender;
                    _userSettings.InterestedIn = userSettings.InterestedIn;
                    _userSettings.Instagram = userSettings.Instagram;
                    _userSettings.ShowInstagram = userSettings.ShowInstagram;
                    _userSettings.Telegram = userSettings.Telegram;
                    _userSettings.ShowTelegram = userSettings.ShowTelegram;

                    _contextDB.UserProfilesDB.Update(_userSettings);
                }

                await _contextDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }



        public async Task SaveUserProfilePhotoAsync(PhotoUploadDTO photo)
        {
            var usersSettings = await _contextDB.UserProfilesDB.FirstOrDefaultAsync(u => u.Id == int.Parse(photo.Id));

            if (usersSettings != null)
            {
                usersSettings.PhotoPath = UploadPhoto(photo.Photo, photo.userName);
            }
            else
            {
                throw new Exception("There is no user to save photo.");
            }

            _contextDB.UserProfilesDB.Update(usersSettings);
            await _contextDB.SaveChangesAsync();
        }
        private string UploadPhoto(IFormFile photo, string userName)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new ArgumentException("The file is empty or not provided.");
            }

            string userFolderPath = Path.Combine(folderPath, userName);
            if (!Directory.Exists(userFolderPath))
            {
                Directory.CreateDirectory(userFolderPath);
            }

            string fileName = $"{userName}{Path.GetExtension(photo.FileName)}"; 
            string fullPath = Path.Combine(userFolderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            return fullPath;
        }

        public async Task<FormFile> GetUserProfilePhotoAsync(string userName)
        {
            try
            {
                var userPhotoPath = await _contextDB.UserProfilesDB.Where(u => u.UserName == userName).Select(u => u.PhotoPath).SingleOrDefaultAsync();

                return await CreateFormFile(userPhotoPath);
            }
            catch (Exception ex)
            {
                return await GetDefaultPhotoAsync(defaultAvatarPath);
            }
        }

        private async Task<FormFile> GetDefaultPhotoAsync(string defaultAvatarPath)
        {
            if (!File.Exists(defaultAvatarPath))
            {
                throw new FileNotFoundException("Photo by default is not find.");
            }

            return await CreateFormFile(defaultAvatarPath); 
        }

        private async Task<FormFile> CreateFormFile(string filePath)
        {
            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var memoryStream = new MemoryStream(fileBytes);

            return new FormFile(memoryStream, 0, fileBytes.Length, "photo", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = GetMimeType(Path.GetExtension(filePath))
            };
        }

        private string GetMimeType(string extension)
        {
            return extension.ToLower() switch
            {
                ".png" => "image/png",
                ".jpeg" => "image/jpeg",
                ".jpg" => "image/jpeg",
                _ => "application/octet-stream"
            };
        }

        public async Task<UserProfileDTO> GetUserProfileDataAsync(string userName)
        {
            var _user = await _contextDB.UserProfilesDB.SingleOrDefaultAsync(u => u.UserName == userName);

            if (_user == null)
            {
                throw new Exception($"User Profile data, with UserName {userName} is not exist.");
            }

            return new UserProfileDTO
            {
                UserId = _user.Id,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                AboutMe = _user.AboutMe,
                Age = _user.Age,
                Gender = _user.Gender,
                InterestedIn = _user.InterestedIn, 
                Instagram = _user.Instagram,
                ShowInstagram = _user.ShowInstagram,    
                Telegram = _user.Telegram,
                ShowTelegram = _user.ShowTelegram,

                UserName = _user.UserName,
            };
        }
    }
}
