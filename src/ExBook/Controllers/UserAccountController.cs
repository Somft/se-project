using System;
using System.Linq;
using System.Threading.Tasks;

using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExBook.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly UserAccountService userAccountService;

        public UserAccountController(UserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        
        [HttpGet]
        [Route("/show")]
        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.View("Show",new UserAccountViewModel()
                {
                    ModifiedUser = await userAccountService.GetUser(this.HttpContext.User.GetId())
                })
                : this.RedirectToHome() as IActionResult;
        }


        [HttpGet]
        [Route("/edit")]
        public async Task<IActionResult> Edit()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.View("Edit", new UserAccountViewModel()
                {
                    ModifiedUser = await userAccountService.GetUser(this.HttpContext.User.GetId())
                })
                : this.RedirectToHome() as IActionResult;
        }

        [HttpGet]
        [Route("/editAdmin")]
        public async Task<IActionResult> EditAdmin(Guid ModifiedUserId)
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.View("Edit", new UserAccountViewModel()
                {
                    ModifiedUser = await userAccountService.GetUser(ModifiedUserId),
                    CurrentUser = await userAccountService.GetUser(this.HttpContext.User.GetId())
                })
                : this.RedirectToHome() as IActionResult;
        }


        [HttpPost]
        [Route("/show")]
        public async Task<IActionResult> Update(User sentUserData)
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.View("Show", new UserAccountViewModel()
                {
                    ModifiedUser = await userAccountService.UpdateData(this.HttpContext.User.GetId(),sentUserData)
                                        
                })
                : this.RedirectToHome() as IActionResult;
        }


    }
}
