using Real_time_Chat_Application.Models;
using Real_time_Chat_Application.Repositories.Interfaces;
using static System.Net.WebRequestMethods;

namespace Real_time_Chat_Application.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly string[] publicPaths = new[] 
        { 
            "/api/user/loginUser", 
            "/api/user/register",
            "/api/Cookie/getEncodedTokenFromCookieByKey/jwtToken"
        };


        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
        {
            
            if (publicPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
            {
                await _next(context); 
                return;
            }

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var user = new UserDTO
                {
                    UserName = context.User.Claims.FirstOrDefault(u => u.Type == "userName")?.Value,
                    Email = context.User.Claims.FirstOrDefault(u => u.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value,

                };

                var authorizedClaim = context.User.Claims.FirstOrDefault(u => u.Type == "authorized")?.Value;
                if (!string.IsNullOrEmpty(authorizedClaim) && bool.TryParse(authorizedClaim, out var isAuthorized))
                {
                    user.Authorized = isAuthorized;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid token data.");
                    return;
                }

                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid token data.");
                    return;
                }

                if (!await userRepository.UserExistsAsync(user)) 
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("User not found or unauthorized.");
                    return;
                }

                await _next(context);
            }
        }
    }
}
