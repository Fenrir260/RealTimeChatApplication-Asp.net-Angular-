using Microsoft.Extensions.Configuration;
using Real_time_Chat_Application.Models;
using Real_time_Chat_Application.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Real_time_Chat_Application.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly string _email = "chepurnyi.valerii3@gmail.com";

        private readonly string _password = "jtuwjgkepzjtdwxj";

        private readonly IPasswordHashService _passwordHashService;

        private readonly ITokenService _tokenService;

        private readonly IConfiguration _config;

        public EmailService(IConfiguration config, IPasswordHashService passwordHashService, ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
            _passwordHashService = passwordHashService;
        }

        public async Task<string> SendVerificationMessageAsync(UserRegisterDTO data)
        {
            var token = _tokenService.CreateToken(data.UserName, data.Email, false, 7);

            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential(_email, _password);
                smtpClient.EnableSsl = true;



                var message = new MailMessage()
                {
                    From = new MailAddress(_email, "Account Verification"),
                    Subject = "Account Verification",
                    Body = $"Для підтвердення пошти перейдіть по даному посиланню: \n {data.ConfirmationUrl}",
                    IsBodyHtml = true
                };

                message.To.Add(data.Email);

                await smtpClient.SendMailAsync(message);
            }

            return token;
        }
    }
}
