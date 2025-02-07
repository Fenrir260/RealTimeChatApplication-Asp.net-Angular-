namespace Real_time_Chat_Application.Data.Entity
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AboutMe { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string InterestedIn { get; set; }
        public string Instagram { get; set; }
        public bool ShowInstagram { get; set; }
        public string Telegram { get; set; }
        public bool ShowTelegram { get; set; }
        public string PhotoPath { get; set; }

        public int Id { get; set; }
        public string UserName { get; set; }    
        public User User { get; set; }
    }
}
