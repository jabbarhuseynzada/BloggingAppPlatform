using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BloggingAppPlatform.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDto userForRegisterDto)
        {
            var userExistsByMail = _authService.UserExists(userForRegisterDto.Email);
            var userExistsByUsername = _authService.UserExistsByUsername(userForRegisterDto.Username);

            if (!userExistsByMail.Success || !userExistsByUsername.Success)
            {
                ViewBag.ErrorMessage = "Email or Username is already used by another user";
                return View(userForRegisterDto);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);

            if (!registerResult.Success)
            {
                ViewBag.ErrorMessage = registerResult.Message;
                return View(userForRegisterDto);
            }

            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                HttpContext.Session.SetString("JWTToken", result.Data.Token);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = result.Message;
            return View(userForRegisterDto);
        }

        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            var userToLogin = _authService.Login(loginDto);
            if (!userToLogin.Success)
            {
                ViewBag.ErrorMessage = userToLogin.Message;
                return View();
            }

            // Step 2: Generate JWT token
            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                // If successful, store JWT token in session (for example)
                HttpContext.Session.SetString("JWTToken", "Bearer " + result.Data.Token);

                // Alternatively, you can return it to the client-side where it will be stored in localStorage

                // Redirect to some protected area after successful login
                return RedirectToAction("Index", "Home"); // Redirect to Home after login
            }
            else
            {
                // Return error if token creation fails
                ViewBag.ErrorMessage = result.Message;
                return View();
            }
        }
        [HttpPost]
        public IActionResult Logout()
        {
            // Sign the user out
            HttpContext.SignOutAsync();

            // Redirect to the home page or any desired page
            return RedirectToAction("Index", "Home");
        }
    }
}
