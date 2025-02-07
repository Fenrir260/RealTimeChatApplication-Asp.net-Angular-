using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Real_time_Chat_Application.Data.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        [DefaultValue(false)]
        public bool Authorized { get; set; }

        public ICollection<Friendship> Friends { get; set; }
        public ICollection<Friendship> FriendOf { get; set; }

        public ICollection<UserChat> UserChats { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
