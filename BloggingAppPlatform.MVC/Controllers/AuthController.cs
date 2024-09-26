using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BloggingAppPlatform.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: Register view
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto userForRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please check the input fields.";
                return View(userForRegisterDto);
            }

            var userExistsByMail = _authService.UserExists(userForRegisterDto.Email);
            var userExistsByUsername = _authService.UserExistsByUsername(userForRegisterDto.Username);

            if (!userExistsByMail.Success || !userExistsByUsername.Success)
            {
                ViewBag.ErrorMessage = "Email or Username is already used by another user.";
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
                // Save the JWT token in session
                HttpContext.Session.SetString("JWTToken", result.Data.Token);

                // Sign the user in using cookie authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, registerResult.Data.Username),
                    new Claim(ClaimTypes.Email, registerResult.Data.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                ViewBag.SuccessMessage = "Registration successful!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = result.Message;
            return View(userForRegisterDto);
        }

        // GET: Login view
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please provide valid credentials.";
                return View(loginDTO);
            }

            var userToLogin = _authService.Login(loginDTO);
            if (!userToLogin.Success)
            {
                ViewBag.ErrorMessage = userToLogin.Message;
                return View(loginDTO);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                // Save the JWT token in session
                HttpContext.Session.SetString("JWTToken", result.Data.Token);

                // Sign the user in using cookie authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userToLogin.Data.Username),
                    new Claim(ClaimTypes.Email, userToLogin.Data.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                ViewBag.SuccessMessage = "Login successful!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = result.Message;
            return View(loginDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign the user out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("JWTToken"); // Clear JWT token from session
            return RedirectToAction("Login", "Auth");
        }
    }
}
