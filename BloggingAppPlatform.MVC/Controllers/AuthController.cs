using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using Core.Helpers.Security.JWT;
using Entities.DTOs;
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

            // Step 1: Validate user credentials
            var userToLogin = _authService.Login(loginDTO);
            if (!userToLogin.Success)
            {
                // Display specific error message
                ViewBag.ErrorMessage = userToLogin.Message; // Pass the error message to ViewBag
                return View(loginDTO);
            }

            // Step 2: Generate JWT token (for future API use if needed)
            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message; // Pass the token generation error
                return View(loginDTO);
            }

            // Step 3: Get user roles from the service or the token (if stored in claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userToLogin.Data.Username),
                new Claim(ClaimTypes.Email, userToLogin.Data.Email),
                new Claim("userId", userToLogin.Data.Id.ToString()), // User ID
            };

            var roles = _userService.GetClaims(userToLogin.Data);  // Assuming GetClaims returns roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            // Step 4: Create ClaimsIdentity and sign in with cookie authentication
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true }; // Persistent cookie

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Step 5: Redirect user to home or another page after successful login
            ViewBag.SuccessMessage = "Login successful!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Log the user out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Login");
        }
    }
}
