using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models.Authentication;
using ExBook.Models.WhishList;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Controllers
{
    public class BookShelfController
    {
        private readonly BookShelfService bookShelfService;
        private ApplicationDbContext dbContext;
        public BookShelfController(BookShelfService bookShelfService, ApplicationDbContext dbContext)
        {
            this.bookShelfService = bookShelfService;
            this.dbContext = dbContext;
        }
        
        [HttpGet]
        [Route("/bookshelf")]
        public async Task<IActionResult> Index()
        {
            Guid userID = GetUserID();
            var result = await bookShelfService.GetUserBook(userID);
            return View (new WishListViewModel() {Books = result });
          
        }
       
        [HttpPost]
        [Route("/addToList")]
        public IActionResult Add()
        {
            //this.HttpContext.Response.Cookies.Delete("Authentication");
            return this.RedirectToAddToWishList();
        }

        public Guid GetUserID()
        {
            string login = this.HttpContext.User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            return dbContext.Users.Where(user => user.Login == login).Single().Id;
        }

    }
    
}