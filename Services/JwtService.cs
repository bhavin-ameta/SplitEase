using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SplitEase.Helpers;
using SplitEase.Model;

namespace SplitEase.Services
{

    public class JwtService : IJwtService
    {
        private readonly JWTSettings _settings;

        public JwtService(IOptions<JWTSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateToken(Usermodel user)
        {
            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
                };
            // add role claim here if needed: new Claim(ClaimTypes.Role, "User")
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer!,
                audience: _settings.Audience!,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_settings.ExpiresInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string GeneratePasswordResetToken(Usermodel user)
        {
            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


