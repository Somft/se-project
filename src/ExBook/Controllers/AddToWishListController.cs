using ExBook.Extensions;
using ExBook.Models.AddToWhishList;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace ExBook.Controllers
{
    public class AddToWishListController : Controller
    {
        private readonly AddToWishListService addToWishListService;

        public AddToWishListController(AddToWishListService addToWishListService)
        {
            this.addToWishListService = addToWishListService;
        }

        [HttpGet]
        [Route("/addtowishlist")]
    
        public IActionResult Index()
        {
           return this.View(new AddToWishListViewModel() );
        }

        public async Task<IActionResult> Index(AddToWishListViewModel input)
        {
            await this.addToWishListService.AddBook(input);
            return this.Redirect("/search");
        }

        [HttpGet]
        [Route("/register-success")]
        [AllowAnonymous]
        public IActionResult Success()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.RedirectToHome() as IActionResult
                : this.View();
        }
    }
}