using ExBook.Extensions;
using ExBook.Models.Authentication;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace ExBook.Controllers
{
    [Controller]
    [Authorize]
    public class RegistrationController : Controller
    {
        private readonly RegistrationService registrationService;

        public RegistrationController(RegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        [HttpGet]
        [Route("/register")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.RedirectToHome() as IActionResult
                : this.View(new RegisterViewModel());
        }

        [HttpPost]
        [Route("/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Index(RegisterViewModel request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(request);
            }

            if (request.Password != request.PasswordConfirmation)
            {
                request.Message = "Passwords are not the same";
                return this.View(request);
            }

            if (!await this.registrationService.RegisterUser(request))
            {
                request.Message = "User with given login exists";
                return this.View(request);
            }

            return this.Redirect("/register-success");
        }

        [HttpGet]
        [Route("/register-success")]
        [AllowAnonymous]
        public IActionResult Success()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.RedirectToHome() as IActionResult
                : this.View();
        }
    }
}
