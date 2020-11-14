using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExBook.Services;
using ExBook.Data;
using ExBook.Models.InitializeTransaction;
using ExBook.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExBook.Controllers
{
    public class InitializeTransactionController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly SearchService searchService;
        private readonly WishListController wishListController;
        public InitializeTransactionController(ApplicationDbContext applicationDbContext,SearchService searchService)
        {
            this.applicationDbContext = applicationDbContext;
            this.searchService = searchService;
            this.wishListController = wishListController;
        }

        [HttpPost]
        [Route("/initializetransaction")]
        public async Task<IActionResult> Index()
        {
            Guid userID = this.GetUserID();
            return this.View(new InitializeTransactionViewModel()
            { CurrentUserBooks = await this.searchService.GetBookShelfBooksById(userID) });
        }

        public void AddBookToTransaction()
        {

        }



        private Guid GetUserID()
        {
            string login = this.HttpContext.User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            return this.applicationDbContext.Users.Where(user => user.Login == login).Single().Id;
        }



    }
}
