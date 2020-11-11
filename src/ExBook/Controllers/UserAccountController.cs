using System.Linq;

using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExBook.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserAccountController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.View(new UserAccountViewModel()
                {
                    CurrentUser = this.applicationDbContext.Users.FirstOrDefault(i => i.Id == this.HttpContext.User.GetId())
                })
                : this.RedirectToHome() as IActionResult;
        }
    }
}
