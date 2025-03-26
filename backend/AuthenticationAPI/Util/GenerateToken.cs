using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationAPI.util
{
    public abstract class GenerateToken
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        public GenerateToken(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public string GenerateTokenWithClaim(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII
                .GetBytes(_configuration.GetSection("JWTSetting").GetSection("securityKey").Value!); 

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims = new List<Claim>
            {
                 new (JwtRegisteredClaimNames.Email,user.Email ?? ""),
                new(JwtRegisteredClaimNames.Name,user.FirstName ?? ""),
                new(JwtRegisteredClaimNames.Name,user.LastName ?? ""),
                new(JwtRegisteredClaimNames.NameId,user.Id ?? ""),
                new(JwtRegisteredClaimNames.Aud,_configuration.GetSection("JWTSetting").GetSection("ValidAudience").Value!),
                new(JwtRegisteredClaimNames.Iss,_configuration.GetSection("JWTSetting").GetSection("ValidIssuer").Value!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class TokenGenerator : GenerateToken
    {
        public TokenGenerator(IConfiguration configuration, UserManager<AppUser> userManager) 
            : base(configuration, userManager)
        {
        }
    }
}
