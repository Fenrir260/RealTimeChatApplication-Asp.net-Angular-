namespace Real_time_Chat_Application.Data.Entity
{
    public class Friendship
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int FriendId { get; set; }
        public User Friend { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsAccepted { get; set; } 
    }
}
