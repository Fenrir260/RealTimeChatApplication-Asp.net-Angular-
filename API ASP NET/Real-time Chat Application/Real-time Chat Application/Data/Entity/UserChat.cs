using Microsoft.EntityFrameworkCore;

namespace Real_time_Chat_Application.Data.Entity
{
    [Keyless]
    public class UserChat
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
