using Real_time_Chat_Application.Models;

namespace Real_time_Chat_Application.Services.Interfaces
{
    public interface IFriendshipService
    {
        Task SendFriendRequestAsync(UserDTO toUser);
        Task DeclineFriendRequestAsync(UserDTO toUser);
        Task AcceptFriendRequestAsync(UserDTO toUser);
        Task CancelFriendRequestAsync(UserDTO toUser);
        Task<IEnumerable<UserDTO>?> GetMyFriendsListAsync();
        Task<IEnumerable<UserDTO>?> GetFriendsRequestListAsync();
        Task<IEnumerable<UserDTO>?> GetSendFriendsRequestListAsync();
        Task<IEnumerable<UserDTO>> SearchUsersByUserNameOrProfileAsync(string userName);
    }
}
