using ExBook.Data;
using ExBook.Extensions;
using ExBook.Views.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExBook.Controllers
{
    [Authorize]
    public class AuthenticationController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IConfiguration configuration;

        public AuthenticationController(ApplicationDbContext applicationDbContext, IConfiguration configuration)
        {
            this.applicationDbContext = applicationDbContext;
            this.configuration = configuration;
        }



        [HttpGet]
        [Route("/login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.RedirectToHome() as IActionResult
                : this.View(new LoginViewModel());
        }

        [HttpPost]
        [Route("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            User? user = await this.AuthenticateAsync(request.Login, request.Password);

            if (user != null)
            {
                this.HttpContext.Response.Cookies.Append("Authentication", this.GenerateJwt(user), new CookieOptions());

                return this.RedirectToHome();
            }

            return this.View(new LoginViewModel()
            {
                Message = "Incorrect username or password",
                Login = request.Login
            });
        }


        [HttpPost]
        [Route("/logout")]
        public IActionResult Logout()
        {
            this.HttpContext.Response.Cookies.Delete("Authentication");
            return this.RedirectToLogin();
        }

        private async Task<User?> AuthenticateAsync(string login, string password)
        {
            User? user = await this.applicationDbContext.Users.SingleOrDefaultAsync(u => u.Login == login);

            //TODO use hash!!!!!!!!!!!
            if (user != null && user.Password == password)
            {
                return user;
            }

            return null;
        }

        private string GenerateJwt(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                this.configuration["Jwt:Issuer"],
                this.configuration["Jwt:Audience"],
                new[]
                {
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Login),
                        new Claim(ClaimTypes.Email, "test"),
                },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
