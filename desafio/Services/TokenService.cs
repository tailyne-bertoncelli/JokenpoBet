using desafio.Context;
using desafio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace desafio.Services
{
    public class TokenService
    {
        private readonly ApplicationContext _context;
        public TokenService(ApplicationContext context) {
            _context = context;
        }

        public async Task<string> GenerateToken(User user)
        {
            var wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == user.Id);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                    ),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserType.ToString())
                })
            };

            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
