using desafio.Context;
using desafio.DataTransferObject.Generics;
using desafio.DataTransferObject.Login;
using desafio.Entities;
using desafio.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace desafio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TokenService _tokenService;

        public LoginController (ApplicationContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _tokenService = new TokenService(_context);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Email);

                if (user == null)
                {
                    return Unauthorized(new ApiResponse<string>(null, new List<string> { "Email ou senha inválidos" }, 409));
                }

                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, login.Senha);
                if (result == PasswordVerificationResult.Failed)
                {
                    return Unauthorized(new ApiResponse<string>(null, new List<string> { "Email ou senha inválidos" }, 409));
                }

                var token = await _tokenService.GenerateToken(user);

                var wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == user.Id);
                var response = new ResponseLoginDto
                {
                    Id = user.Id,
                    Nome = user.Name,
                    Email = user.Email,
                    TipoUsuario = user.UserType,
                    SaldoEmCarteira = wallet?.Balance,
                    Token = token
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(null, new List<string> { ex.Message }, 400));
            }
        }
    }
}
