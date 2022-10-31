using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUser _IUser;
        public AuthController(IUser IUser)
        {
            _IUser = IUser;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(user _user)
        {
            user user = _IUser.UserLogin(_user);
            if (user == null)
            {
                return BadRequest("Invalid Credentials");
            }

            var claims = new List<Claim>
    {
        new Claim("userid", user.id.ToString()),
        new Claim(ClaimTypes.Email, user.login)
    };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Ok("success");
        }

        [HttpGet("userprofile")]
        public async Task<IActionResult> UserProfileAsync(int id)
        {

            int userId = HttpContext.User.Claims
            .Where(_ => _.Type == "userid")
            .Select(_ => Convert.ToInt32(_.Value))
            .First();

            var userProfile = _IUser.GetUserData(userId);

            return Ok(userProfile);



        }
    }
}
