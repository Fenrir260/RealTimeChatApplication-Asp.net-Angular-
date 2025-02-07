using Microsoft.EntityFrameworkCore;
using Real_time_Chat_Application.Data.ContextDb;
using Real_time_Chat_Application.Repositories.Implementations;
using Real_time_Chat_Application.Repositories.Interfaces;
using Real_time_Chat_Application.Services.Implementations;
using Real_time_Chat_Application.Services.Interfaces;

namespace Real_time_Chat_Application.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ContextDB>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHashService, PasswordHashService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IFriendshipService, FriendshipService>();

            return services;
        }

    }
}
