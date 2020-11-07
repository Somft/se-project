using ExBook.Extensions;
using ExBook.Models.Authentication;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace ExBook.Controllers
{
    public class AddToWishListController : Controller
    {
        private readonly AddToWishListService addToWishList;

        public AddToWishListController(AddToWishListService addToWishList)
        {
            this.addToWishList = addToWishList;
        }

        [HttpGet]
        [Route("/add")]
    
        public IActionResult Index()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.View()
                : this.RedirectToHome() as IActionResult;
        }

    }
}