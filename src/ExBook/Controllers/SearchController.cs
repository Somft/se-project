using ExBook.Extensions;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.View()
                : this.RedirectToHome() as IActionResult;
        }

    }
}
