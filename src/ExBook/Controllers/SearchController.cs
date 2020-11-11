using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models;
using ExBook.Models.Authentication;
using ExBook.Models.Search;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Linq;
using System.Security.Policy;
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
        public async Task<IActionResult> IndexAsync(string filterTitle, string filterAuthor, bool filterAvailable, string filterSubject)
        {
            if (!string.IsNullOrEmpty(filterTitle) || !string.IsNullOrEmpty(filterAuthor) || filterAvailable|| !string.IsNullOrEmpty(filterSubject))
            {
                return this.HttpContext.User.Identity.IsAuthenticated
                 ? await this.SearchBooks(filterTitle, filterAuthor, filterAvailable, filterSubject)
                 : this.RedirectToHome() as IActionResult;
            }
            else
            {
                return this.HttpContext.User.Identity.IsAuthenticated
                    ? this.View(new SearchBookViewModel()
                    {
                        Books = await searchService.GetAllAvailableBooks(),
                        Subjects = new SelectList(await searchService.GetAllSubjectsNames()),
                        FilterTitle = null,
                        FilterAuthor = null,
                        FilterAvailable = false,
                        FilterSubject = null,
                        UserId = this.HttpContext.User.GetId().Value
                    })
                    : this.RedirectToHome() as IActionResult;
            }

        }

        private async Task<IActionResult> SearchBooks(string filterTitle, string filterAuthor, bool filterAvailable, string filterSubject)
        {
            return this.View("Index", new SearchBookViewModel()
            {
                Books = await searchService.GetBooksFiltered(filterTitle, filterAuthor, filterAvailable, filterSubject),
                Subjects = new SelectList(await searchService.GetAllSubjectsNames()),
                FilterTitle = filterTitle,
                FilterAuthor = filterAuthor,
                FilterAvailable = filterAvailable,
                UserId = this.HttpContext.User.GetId().Value
            });
        }


        [HttpPost]
        [Route("/searchShelves")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookshelvesForBookAsync(Guid Id)
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.View("Bookshelves", new SearchBookShelfBookViewModel()
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
                    ? this.View("Bookshelves", new SearchBookShelfBookViewModel()
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
            return this.View("Bookshelves", new SearchBookShelfBookViewModel()
            {
                BookShelfBooks = await searchService.GetBookShelfBooksFiltered(filterTitle, filterLogin),
                FilterTitle = filterTitle,
                FilterLogin = filterLogin
            });
        }

        [HttpPost]
        public async Task<JsonResult> AddToWishlistAsync(string Id)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                Guid bookId;
                if (Guid.TryParse(Id, out bookId))
                {
                    await searchService.AddToWishList(Guid.Parse(Id), this.HttpContext.User.GetId().Value);
                    return Json(true);
                }
            }
            return Json(false);
        }

        /// <summary>
        /// todo 
        /// </summary>
        /// <param name="CoverUrl"></param>
        /// <returns></returns>
        public ActionResult FullSizeCover(string CoverUrl)
        {
            var model = "https://covers.openlibrary.org/b/id/9271451-M.jpg";
            return PartialView("FullSizeCover", model);
        }


    }
}
