using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please fill out all required fields correctly.";
                return View(registerDto);
            }

            if (registerDto.Password != registerDto.RePassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View(registerDto);
            }

            // Check if email already exists
            var userExistsByMail = _authService.UserExists(registerDto.Email);
            // Check if username already exists
            var userExistsByUsername = _authService.UserExistsByUsername(registerDto.Username);

            // If either email or username exists, display an error message
            if (!userExistsByMail.Success || !userExistsByUsername.Success)
            {
                ViewBag.ErrorMessage = "Email or Username is already used by another user.";
                return View(registerDto);
            }

            // Register the user
            var registerResult = _authService.Register(registerDto, registerDto.Password);
            if (!registerResult.Success)
            {
                ViewBag.ErrorMessage = registerResult.Message;
                return View(registerDto);
            }

            // Generate access token
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(registerDto);
            }

            // Redirect to login page upon successful registration
            TempData["SuccessMessage"] = "Registration successful! You can now log in.";
            return RedirectToAction("Login");
        }


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
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(loginDTO);
            }

            var token = result.Data; 

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token.Token);

            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp");
            if (expClaim != null)
            {
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value));

                Response.Cookies.Append("auth_token", token.Token, new CookieOptions
                {
                    HttpOnly = true, 
                    Secure = true, 
                    SameSite = SameSiteMode.Strict, 
                    Expires = expirationTime 
                });
            }

            // Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userToLogin.Data.Username),
                new Claim(ClaimTypes.Email, userToLogin.Data.Email),
                new Claim("userId", userToLogin.Data.Id.ToString()),
            };

            var roles = _userService.GetClaims(userToLogin.Data);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            // Create ClaimsIdentity and sign in
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            // Sign in user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Log the user out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Remove the auth_token cookie on logout
            Response.Cookies.Delete("auth_token");
            return RedirectToAction("Index", "Home");
        }

    }
}
