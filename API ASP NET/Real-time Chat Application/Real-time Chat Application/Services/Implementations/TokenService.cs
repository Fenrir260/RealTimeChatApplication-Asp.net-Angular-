using Microsoft.IdentityModel.Tokens;
using Real_time_Chat_Application.Models;
using Real_time_Chat_Application.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Real_time_Chat_Application.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("TokenKey") ?? throw new ArgumentNullException("TokenKey is not configured in appsettings.")));
        }

        public string CreateToken(string userName, string email, bool authorized, int expires)
        {
            var claims = new List<Claim>()
            {
                new Claim("userName", userName),
                new Claim("email", email),
                new Claim("authorized", authorized.ToString()),
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(expires),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<Dictionary<string, object>> DecodeToken(string token)
        {
            try
            {
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false, 
                    ValidateLifetime = true, 
                    ValidateIssuerSigningKey = true, 
                    IssuerSigningKey = _key
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    var claims = new Dictionary<string, object>();

                    foreach (var claim in jwtToken.Claims)
                    {
                        claims[claim.Type] = claim.Value;
                    }

                    return claims;
                }
                else
                {
                    throw new Exception("Invalid token format.");
                }
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>
                {
                    { "error", "Invalid token" },
                    { "message", ex.Message }
                };
            }
        }

    }
}
