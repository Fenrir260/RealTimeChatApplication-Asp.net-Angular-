namespace Real_time_Chat_Application.Data.Entity
{
    public class Chat
    {
        public int ChatId { get; set; }
        public string ChatName { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<UserChat> UserChats { get; set; }
    }
}
