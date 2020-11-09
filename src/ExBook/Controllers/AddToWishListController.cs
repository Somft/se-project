using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models.AddToWhishList;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Controllers
{
    [Authorize]
    public class AddToWishListController : Controller
    {
        private readonly AddToWishListService addToWishListService;
        private ApplicationDbContext dbContext;
        public AddToWishListController(AddToWishListService addToWishListService, ApplicationDbContext dbContext)
        {
            this.addToWishListService = addToWishListService;
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("/addtowishlist")]
    
        public IActionResult Index()
        {
           return this.View(new AddToWishListViewModel() );
        }

        public async Task<IActionResult> Index(AddToWishListViewModel input)
        {
            string login = this.HttpContext.User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            Guid num = dbContext.Users.Where(user => user.Login == login).Single().Id;

            if( !await this.addToWishListService.AddBook(input, num))
            {
                input.Message = "Book exists on your wish list";
                return this.View(input);
            }

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