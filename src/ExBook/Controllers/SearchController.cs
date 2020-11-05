using ExBook.Extensions;
using ExBook.Models;
using ExBook.Models.Authentication;
using ExBook.Models.Search;
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
        public async Task<IActionResult> IndexAsync(string filterTitle, string filterLogin)
        {
            if (!string.IsNullOrEmpty(filterTitle) || !string.IsNullOrEmpty(filterLogin))
            {
                return this.HttpContext.User.Identity.IsAuthenticated
                 ? await this.Search(filterTitle, filterLogin)
                 : this.RedirectToHome() as IActionResult;
            }
            else
            {
                return this.HttpContext.User.Identity.IsAuthenticated
                    ? this.View(new SearchBookShelfBookViewModel()
                    {
                        BookShelfBooks = await searchService.GetAllBookShelfBooks(),
                        FilterTitle = null
                    })
                    : this.RedirectToHome() as IActionResult;
            }

        }

        private async Task<IActionResult> Search(string filterTitle, string filterLogin)
        {
            return this.View("Index", new SearchBookShelfBookViewModel()
            {
                BookShelfBooks = await searchService.GetBookShelfBooksFiltered(filterTitle, filterLogin),
                FilterTitle = filterTitle,
                FilterLogin = filterLogin
            });
        }
    }
}
