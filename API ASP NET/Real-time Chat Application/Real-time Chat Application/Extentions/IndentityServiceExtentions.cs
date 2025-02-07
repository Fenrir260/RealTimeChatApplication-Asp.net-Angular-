
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Real_time_Chat_Application.Extentions
{
    public static class IndentityServiceExtentions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            Console.WriteLine("IndentityServiceExtentions");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("TokenKey") ?? throw new ArgumentNullException("TokenKey is not configured in appsettings."))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true, 
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Token validation failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token successfully validated.");
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
