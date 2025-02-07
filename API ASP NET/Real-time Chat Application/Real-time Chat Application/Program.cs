using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Real_time_Chat_Application.Data;
using Real_time_Chat_Application.Extentions;
using Real_time_Chat_Application.Middleware;
using Real_time_Chat_Application.Repositories;
using System.Text;

namespace Real_time_Chat_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORS", builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Set-Cookie");
                });
            });

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            //builder.Services.AddHttpContextAccessor();

            builder.Services.AddIdentityServices(builder.Configuration);

            var app = builder.Build();

            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors("CORS");

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMiddleware<TokenValidationMiddleware>(); //для глобальної перевірки запитів, тобто усі запити, що надходять, окрім винятків

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();
          
            app.Run();
        }
    }
}

