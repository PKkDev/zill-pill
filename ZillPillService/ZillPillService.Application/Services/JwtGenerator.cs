using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZillPillService.Infrastructure.Entities;
using ZillPillService.Infrastructure.ServicesContract;

namespace ZillPillService.Application.Services
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            var key = _configuration["AuthOptions:TokenKey"];
            var issuer = _configuration["AuthOptions:Issuer"];
            var audience = _configuration["AuthOptions:Audience"];
            var lifeTime = Convert.ToInt32(_configuration["AuthOptions:LifeTime"]);

            SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(key));

            var claims = new List<Claim> {
                new Claim("UserIdentity", user.Id.ToString()),
                new Claim("UserIdentityPh", user.Phone),
            };

            if (user.Profile != null)
            {
                if (user.Profile.FirstName != null)
                    claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Profile.FirstName));
                if (user.Profile.Email != null)
                    claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Profile.Email));
            }

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.Add(TimeSpan.FromMinutes(lifeTime)),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
