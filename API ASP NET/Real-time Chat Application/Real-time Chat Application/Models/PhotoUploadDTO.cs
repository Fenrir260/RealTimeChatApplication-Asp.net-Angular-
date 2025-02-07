namespace Real_time_Chat_Application.Models
{
    public class PhotoUploadDTO
    {
        public string Id { get; set; }
        public string userName { get; set; }
        public IFormFile Photo { get; set; }
    }
}
