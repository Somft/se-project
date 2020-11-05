using ExBook.Extensions;
using ExBook.Models;
using ExBook.Models.Authentication;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace ExBook.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService searchService;

        public SearchController(SearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet]
        [Route("/search")]
        [AllowAnonymous]
        public IActionResult Index(string searchString)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                return this.HttpContext.User.Identity.IsAuthenticated
                 ? Search(searchString)
                 : this.RedirectToHome() as IActionResult;
            }
            else
            {
                return this.HttpContext.User.Identity.IsAuthenticated
                    ? this.View(new UsersViewModel()
                    {
                        Users = searchService.GetAllUsers()
                    })
                    : this.RedirectToHome() as IActionResult;
            }

        }

        private IActionResult Search(string searchString)
        {
            ViewBag.Message = searchString;
            return this.View("Index", new UsersViewModel()
            {
                Users = searchService.GetUsersByName(searchString)
            });
        }
    }
}
