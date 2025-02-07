using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Real_time_Chat_Application.Repositories.Interfaces;
using Real_time_Chat_Application.Models;

namespace Real_time_Chat_Application.Filters
{
    public class ValidateTokenAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("ValidateTokenAttribute");

            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userData = new UserDTO
            {
                UserName = context.HttpContext.User.Claims.FirstOrDefault(u => u.Type == "userName")?.Value,
                Email = context.HttpContext.User.Claims.FirstOrDefault(u => u.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value,

            };

            var authorizedClaim = context.HttpContext.User.Claims.FirstOrDefault(u => u.Type == "authorized")?.Value;
            if (!string.IsNullOrEmpty(authorizedClaim) && bool.TryParse(authorizedClaim, out var isAuthorized))
            {
                userData.Authorized = isAuthorized;
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            if (userRepository == null || !await userRepository.UserExistsAsync(userData))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }

}
