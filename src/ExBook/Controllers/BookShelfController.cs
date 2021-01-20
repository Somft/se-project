using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models.BookShelf;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Controllers
{
    [Authorize]
    public class BookShelfController : Controller
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
            return View (new BookShelfViewModel() {Books = result });
            
        }

        public Guid GetUserID()
        {
            string login = this.HttpContext.User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            return dbContext.Users.Where(user => user.Login == login).Single().Id;
        }

    }
    
}