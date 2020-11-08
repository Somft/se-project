using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models;
using ExBook.Models.Authentication;
using ExBook.Models.Search;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace ExBook.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService searchService;
        private ApplicationDbContext dbContext;
        public SearchController(SearchService searchService, ApplicationDbContext dbContext)
        {
            this.searchService = searchService;
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("/search")]
        [AllowAnonymous]
        public async Task<IActionResult> IndexAsync(string filterTitle, string filterAuthor, bool filterAvailable)
        {
            if (!string.IsNullOrEmpty(filterTitle) || !string.IsNullOrEmpty(filterAuthor)|| filterAvailable)
            {
                return this.HttpContext.User.Identity.IsAuthenticated
                 ? await this.SearchBooks(filterTitle, filterAuthor, filterAvailable)
                 : this.RedirectToHome() as IActionResult;
            }
            else
            {
                return this.HttpContext.User.Identity.IsAuthenticated
                    ? this.View(new SearchBookViewModel()
                    {
                        Books = await searchService.GetAllAvailableBooks(),
                        FilterTitle = null,
                        FilterAuthor = null,
                        FilterAvailable = false,
                        UserId= GetUserID()
                    })
                    : this.RedirectToHome() as IActionResult;
            }

        }

        private async Task<IActionResult> SearchBooks(string filterTitle, string filterAuthor, bool filterAvailable)
        {
            return this.View("Index", new SearchBookViewModel()
            {
                Books = await searchService.GetBooksFiltered(filterTitle, filterAuthor, filterAvailable),
                FilterTitle = filterTitle,
                FilterAuthor = filterAuthor,
                FilterAvailable = filterAvailable,
                UserId = GetUserID()
            });
        }


        [HttpPost]
        [Route("/searchShelves")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookshelvesForBookAsync(Guid Id)
        {
                return this.HttpContext.User.Identity.IsAuthenticated
                    ? this.View("Bookshelves",new SearchBookShelfBookViewModel()
                    {
                        BookShelfBooks = await searchService.GetBookShelfBooksById(Id)
                    })
                    : this.RedirectToHome() as IActionResult;
        }

        [HttpGet]
        [Route("/searchShelf")]
        [AllowAnonymous]
        public async Task<IActionResult> BookshelvesAsync(string filterTitle, string filterLogin)
        {
            if (!string.IsNullOrEmpty(filterTitle) || !string.IsNullOrEmpty(filterLogin))
            {
                return this.HttpContext.User.Identity.IsAuthenticated
                 ? await this.SearchBookShelf(filterTitle, filterLogin)
                 : this.RedirectToHome() as IActionResult;
            }
            else
            {
                return this.HttpContext.User.Identity.IsAuthenticated
                    ? this.View(new SearchBookShelfBookViewModel()
                    {
                        BookShelfBooks = await searchService.GetAllBookShelfBooks(),
                        FilterTitle = null,
                        FilterLogin = null
                    })
                    : this.RedirectToHome() as IActionResult;
            }

        }

        private async Task<IActionResult> SearchBookShelf(string filterTitle, string filterLogin)
        {
            return this.View("Index", new SearchBookShelfBookViewModel()
            {
                BookShelfBooks = await searchService.GetBookShelfBooksFiltered(filterTitle, filterLogin),
                FilterTitle = filterTitle,
                FilterLogin = filterLogin
            });
        }

        public ActionResult FullSizeCover(string CoverUrl)
        {
            var model = "https://covers.openlibrary.org/b/id/9271451-M.jpg";
            return PartialView("FullSizeCover", model);
        }
        private Guid GetUserID()
        {
            string login = this.HttpContext.User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            return dbContext.Users.Where(user => user.Login == login).Single().Id;
        }

        [HttpPost]
        [Route("/AddToWishlist")]
        [AllowAnonymous]
        public async Task<IActionResult> AddToWishlist(Guid Id)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                searchService.AddToWishList(Id, GetUserID());
                return new EmptyResult();
            }
            else
            return this.RedirectToHome() as IActionResult; ;
        }

    }
}
