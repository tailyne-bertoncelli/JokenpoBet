using desafio.Context;
using desafio.DataTransferObject.Generics;
using desafio.DataTransferObject.User;
using desafio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace desafio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ApplicationContext _context;

        public UserController(UserService userService, ApplicationContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> InsertUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                var verifyEmail = await _userService.VerifyUserByEmail(userDto.Email);
                if (verifyEmail == true)
                {
                    return Conflict(new ApiResponse<string>(null, new List<string> { "Email já cadastrado!" }, 409));
                }
                var request = await _userService.Create(userDto);

                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(null, new List<string> { ex.Message }, 400));
            }
        }

        [HttpGet]
        [Route("allUsers")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(new ApiResponse<List<UsersDto>>(users, null, 200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(null, new List<string> { ex.Message }, 400));
            }
        }

        [HttpPut]
        [Route("updatePassword")]
        [Authorize(Roles = "PLAYER,ADMIN")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto passwordDto)
        {
            try
            {
                var verifyUser = await _userService.VerifyUserByEmail(passwordDto.Email);
                if (verifyUser == false)
                {
                    return Conflict(new ApiResponse<string>(null, new List<string> { "Usuário não encontrado!" }, 409));
                }
                var request = await _userService.UpdatePassword(passwordDto);

                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(null, new List<string> { ex.Message }, 400));
            }
        }
    }
}
