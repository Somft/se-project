
using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models.AddToBookShelf;
using ExBook.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Threading.Tasks;
namespace ExBook.Controllers
{
    [Authorize]
    public class AddToBookshelfController : Controller
    {
        
        private readonly AddToBookShelfService addToBookShelfService;
        private ApplicationDbContext dbContext;
        public AddToBookshelfController(AddToBookShelfService addToBookShelfService, ApplicationDbContext dbContext)
        {
            this.addToBookShelfService = addToBookShelfService;
            this.dbContext = dbContext;
        }
        
        
        [HttpGet]
        [Route("/addtobookshelf")]
    
        public IActionResult Index()
        {
            return this.View(new AddToBookShelfViewModel() );
        }
        
        [HttpPost]
        [Route("/addtobookshelf")]
        public async Task<IActionResult> Index(AddToBookShelfViewModel input)
        {
            string login = this.HttpContext.User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            Guid num = dbContext.Users.Where(user => user.Login == login).Single().Id;
            
            if ( !await this.addToBookShelfService.AddBookToBookshelf(input, this.HttpContext.User.GetId()))
            {
                input.Message = "Book exists on your bookshelf";
                return this.View(input);
            }
            else
            {
                input.Message = "Book added successfully!";
                input.Success = true;
                return this.View(input);
            }
        }
        
        
        public async Task<IActionResult> RemoveBookFromShelf(Guid Id)
        {
            await this.addToBookShelfService.RemoveBook(Id);
            return this.RedirectToBookShelf();
           
        }
    }
}