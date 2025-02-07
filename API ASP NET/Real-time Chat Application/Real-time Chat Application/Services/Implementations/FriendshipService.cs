using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Real_time_Chat_Application.Data.ContextDb;
using Real_time_Chat_Application.Data.Entity;
using Real_time_Chat_Application.Models;
using Real_time_Chat_Application.Services.Interfaces;
using System.Data;

namespace Real_time_Chat_Application.Services.Implementations
{
    public class FriendshipService : IFriendshipService
    {
        private readonly ContextDB _contextDb;

        private readonly ICookieService _cookieService;

        private readonly ITokenService _tokenService;

        public FriendshipService(ContextDB contextDb, ICookieService cookieService, ITokenService tokenService)
        {
            _contextDb = contextDb;
            _cookieService = cookieService;
            _tokenService = tokenService;
        }

        public async Task SendFriendRequestAsync(UserDTO toUser)
        {
            try
            {
                var _fromUser = await GetUserFromCookieAsync("jwtToken");

                if (_fromUser == null)
                {
                    throw new Exception("Sender user not found.");
                }

                var _toUser = await _contextDb.UsersDB.SingleOrDefaultAsync(u => u.UserName == toUser.UserName);

                if (_toUser == null)
                {
                    throw new Exception("Recipient user not found.");
                }

                if (!_fromUser.Authorized || !_toUser.Authorized)
                {
                    throw new Exception("One of the users is not authorized.");
                }

                if(await IsFriendRequestExistingAsync(_fromUser.Id, _toUser.Id))
                {
                    throw new Exception("Friend request have already exist.");
                }

                var friendshipRequest = new Friendship
                {
                    UserId = _fromUser.Id,
                    User = _fromUser,
                    FriendId = _toUser.Id,
                    Friend = _toUser,
                    CreatedAt = DateTime.UtcNow,
                    IsAccepted = false,
                };

                await _contextDb.FriendshipsDB.AddAsync(friendshipRequest);
                await _contextDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task DeclineFriendRequestAsync(UserDTO toUser)
        {
            try
            {
                var _fromUser = await GetUserFromCookieAsync("jwtToken");

                if (_fromUser == null)
                {
                    throw new Exception("Sender user not found.");
                }

                var _toUser = await _contextDb.UsersDB.SingleOrDefaultAsync(u => u.UserName == toUser.UserName);

                if (_toUser == null)
                {
                    throw new Exception("Recipient user not found.");
                }

                if (!await IsFriendRequestExistingAsync(_fromUser.Id, _toUser.Id))
                {
                    throw new Exception("Friend request is not exist.");
                }

                var friendship = await _contextDb.FriendshipsDB.SingleOrDefaultAsync(u => u.UserId == _toUser.Id && u.FriendId == _fromUser.Id);

                if(friendship == null)
                {
                    throw new Exception("There is no friendship request.");
                }

                _contextDb.FriendshipsDB.Remove(friendship);
                await _contextDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        private async Task<bool> IsFriendRequestExistingAsync(int fromUserId, int toUserId)
        {
            return await _contextDb.FriendshipsDB.AnyAsync(f => (f.UserId == fromUserId && f.FriendId == toUserId) || (f.UserId == toUserId && f.FriendId == fromUserId));
        }

        public async Task AcceptFriendRequestAsync(UserDTO toUser)
        {
            try
            {
                var _fromUser = await GetUserFromCookieAsync("jwtToken");

                var _toUser = await _contextDb.UsersDB.FirstOrDefaultAsync(u => u.UserName == toUser.UserName);

                if(_fromUser == null || _toUser == null)
                {
                    throw new Exception("One of the users was not found.");
                }

                if(_fromUser == _toUser)
                {
                    throw new Exception("Users are the same.");
                }

                var friendRequest = await _contextDb.FriendshipsDB.Where(u => u.FriendId == _fromUser.Id && u.UserId == _toUser.Id).FirstOrDefaultAsync();

                if(friendRequest == null)
                {
                    throw new Exception("Friend request between users was not found.");
                }

                if(friendRequest.IsAccepted == true)
                {
                    throw new Exception("Friend request have already accepted.");
                }
                
                friendRequest.IsAccepted = true;

                var newFriend = new Friendship
                {
                    FriendId = _toUser.Id,
                    Friend = _toUser,
                    UserId = _fromUser.Id,
                    User = _fromUser,
                    CreatedAt = friendRequest.CreatedAt,
                    IsAccepted = true,
                };

                _contextDb.FriendshipsDB.Update(friendRequest);
                await _contextDb.FriendshipsDB.AddAsync(newFriend);
                await _contextDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<IEnumerable<UserDTO>?> GetMyFriendsListAsync()
        {
            try
            {
                var user = await GetUserFromCookieAsync("jwtToken");

                if (user == null)
                {
                    throw new Exception("User was not found.");
                }

                var friendsList = await _contextDb.FriendshipsDB.Where(u => u.UserId == user.Id && u.IsAccepted == true).ToListAsync();

                return await GetFriendsListByConditionAsync(friendsList, true);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<IEnumerable<UserDTO>?> GetFriendsRequestListAsync()
        {
            try
            {
                var user = await GetUserFromCookieAsync("jwtToken");

                if (user == null)
                {
                    throw new Exception("User was not found.");
                }

                var friendsList = await _contextDb.FriendshipsDB.Where(u => u.FriendId == user.Id && u.IsAccepted == false).ToListAsync();

                return await GetFriendsListByConditionAsync(friendsList, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<IEnumerable<UserDTO>?> GetSendFriendsRequestListAsync()
        {
            try
            {
                var user = await GetUserFromCookieAsync("jwtToken");

                if (user == null)
                {
                    throw new Exception("User was not found.");
                }

                var friendsList = await _contextDb.FriendshipsDB.Where(u => u.UserId == user.Id && u.IsAccepted == false).ToListAsync();

                return await GetFriendsListByConditionAsync(friendsList, true);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        private async Task<List<UserDTO>?> GetFriendsListByConditionAsync(List<Friendship>? friendsList, bool checkFriend)
        {
            List<UserDTO> result = new List<UserDTO>();

            foreach (var _friend in friendsList)
            {
                User friend;

                if(checkFriend)
                    friend = await _contextDb.UsersDB.SingleOrDefaultAsync(u => u.Id == _friend.FriendId);
                else
                    friend = await _contextDb.UsersDB.SingleOrDefaultAsync(u => u.Id == _friend.UserId);

                if (friend != null)
                {
                    result.Add(new UserDTO
                    {
                        UserName = friend.UserName,
                        Email = friend.Email,
                        Authorized = friend.Authorized,
                    });
                }
            }

            return result;
        }

        private async Task<User?> GetUserFromCookieAsync(string key)
        {
            try
            {
                var token = _cookieService.GetCookieToken(key);

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token is null or empty.");
                }

                var decodedToken = await _tokenService.DecodeToken(token);

                if (!decodedToken.ContainsKey("userName"))
                {
                    throw new Exception("Token does not contain userName.");
                }

                string userName = decodedToken["userName"]?.ToString();

                if (string.IsNullOrEmpty(userName))
                {
                    throw new Exception("Decoded userName is null or empty.");
                }

                var user = await _contextDb.UsersDB.SingleOrDefaultAsync(u => u.UserName == userName);

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetUserFromCookieAsync: {ex.Message}.");
            }
        }

        public async Task<IEnumerable<UserDTO>> SearchUsersByUserNameOrProfileAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("User name cannot be null or empty.", nameof(userName));
            }

            var users = await _contextDb.UsersDB
                .Where(u => EF.Functions.Like(u.UserName, $"%{userName}%"))
                .ToListAsync();

            var userProfiles = await _contextDb.UserProfilesDB
                .Where(u => EF.Functions.Like(u.FirstName, $"%{userName}%"))
                .ToListAsync();

            var result = new List<UserDTO>();

            result.AddRange(MapUsersToDTOList(users));
            result.AddRange(await MapUserProfilesToDTOListAsync(userProfiles));


            var thisUser = await GetUserFromCookieAsync("jwtToken"); 

            if (thisUser != null)
            {
                result.RemoveAll(u => u.UserName == thisUser.UserName);
            }

            return result.DistinctBy(u => u.UserName).ToList();
        }

        private List<UserDTO> MapUsersToDTOList(List<User> users)
        {
            return users.Select(user => new UserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                Authorized = user.Authorized
            }).ToList();
        }

        private async Task<List<UserDTO>> MapUserProfilesToDTOListAsync(List<UserProfile> userProfiles)
        {
            var userNames = userProfiles.Select(u => u.UserName).ToList();
            var matchedUsers = await _contextDb.UsersDB
                .Where(u => userNames.Contains(u.UserName))
                .ToDictionaryAsync(u => u.UserName);

            return userProfiles.Select(profile =>
            {
                var user = matchedUsers.GetValueOrDefault(profile.UserName);
                return new UserDTO
                {
                    UserName = profile.UserName,
                    Email = user?.Email ?? string.Empty,
                    Authorized = user?.Authorized ?? false
                };
            }).ToList();
        }

        public async Task CancelFriendRequestAsync(UserDTO toUser)
        {
            try
            {
                var _fromUser = await GetUserFromCookieAsync("jwtToken");
                
                if(_fromUser == null )
                {
                    throw new Exception("Sender user not found.");
                }

                var friendship = await _contextDb.FriendshipsDB.Where(u => u.Friend.UserName == toUser.UserName && u.UserId == _fromUser.Id).SingleOrDefaultAsync();

                if (friendship == null) 
                {
                    throw new Exception("Friendship request was not found.");
                }
                
                _contextDb.FriendshipsDB.Remove(friendship);
                await _contextDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
