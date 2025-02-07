namespace Real_time_Chat_Application.Data.Entity
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public string? Sentiment { get; set; }
    }
}
