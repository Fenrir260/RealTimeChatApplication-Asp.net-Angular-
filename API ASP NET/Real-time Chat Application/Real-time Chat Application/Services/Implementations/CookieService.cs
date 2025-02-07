using Azure.Core;
using Microsoft.AspNetCore.Http;
using Real_time_Chat_Application.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Real_time_Chat_Application.Services.Implementations
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCookieToken(string key)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies[key];

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("JWT token not found in cookies or is empty.");
            }

            if (!token.Contains("."))
            {
                throw new Exception("Invalid JWT format: Token does not contain dots.");
            }

            return token;
        }

        public Task SetCookieToken(string key, string token, int expires)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(expires),
                IsEssential = true,
                Path = "/",
                HttpOnly = false,
                SameSite = SameSiteMode.None,
                Secure = true,
            };


            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, token, options);

            return Task.CompletedTask;
        }
    }
}
