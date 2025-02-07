namespace Real_time_Chat_Application.Services.Interfaces
{
    public interface ICookieService
    {
        Task SetCookieToken(string key, string token, int expires);
        string GetCookieToken(string key);
    }
}
