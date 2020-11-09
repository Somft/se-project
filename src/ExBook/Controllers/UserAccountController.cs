using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models.UserAccount;
using ExBook.Services;

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

        [HttpGet]
        [Route("/useraccount")]
        [AllowAnonymous]
        public IActionResult AccountInfo()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.View(new UserAccountViewModel()
                {
                    CurrentUser = this.applicationDbContext.Users.First(i => i.Id == this.HttpContext.User.getID)
                })
                : this.RedirectToHome() as IActionResult; 
        }
    }
}
