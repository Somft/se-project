using ExBook.Extensions;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExBook.Controllers
{
    public class WishListController : Controller
    {
        private readonly WishListService wishListService;

        public WishListController(WishListService wishListService)
        {
            this.wishListService = wishListService;
        }

        [HttpGet]
        [Route("/wishlist")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.View()
                : this.RedirectToSearch() as IActionResult;
        }

    }
}
