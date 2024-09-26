using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public ActionResult Register(RegisterDto userForRegisterDto)
        {
            var userExistsByMail = _authService.UserExists(userForRegisterDto.Email);
            var userExistsByUsername = _authService.UserExistsByUsername(userForRegisterDto.Username);
            if (!userExistsByMail.Success && !userExistsByUsername.Success)
            {
                return BadRequest("Email or Username is already used by another user");
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("login")]
        public ActionResult Login(LoginDto loginDTO)
        {
            var userToLogin = _authService.Login(loginDTO);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }
            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
                return BadRequest(result.Message);
        }
    }
}
