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
using ExBook.Extensions;

namespace ExBook.Controllers
{
    public class InitializeTransactionController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly SearchService searchService;
        private readonly InitializeTransactionService initializeTransactionService;
        private Transaction transaction;
        public InitializeTransactionController(ApplicationDbContext applicationDbContext,SearchService searchService, InitializeTransactionService initializeTransactionService)
        {
            this.applicationDbContext = applicationDbContext;
            this.searchService = searchService;
            this.initializeTransactionService = initializeTransactionService;
            this.transaction = new Transaction();

        }

        [HttpPost]
        [Route("/initializetransaction")]
        public async Task<IActionResult> Index(Guid id)
        {   if (HttpContext.User.Identity.IsAuthenticated)
            {
                Guid InitiatorId = this.HttpContext.User.GetId().Value;
                BookShelfBook bsb = await this.initializeTransactionService.GetBookShelfBookById(id);
                var init = await this.initializeTransactionService.GetUserById(InitiatorId);
                var reci = await this.initializeTransactionService.GetUserById(bsb.BookShelf.UserId);
                this.transaction.Id = Guid.NewGuid();              

                    this.transaction.Initiator = init;
                this.transaction.Recipient = reci;
                this.transaction.InitiatorId = InitiatorId;
                this.transaction.RecipientId = bsb.BookShelf.UserId;
                   

                return this.View(new InitializeTransactionViewModel()
                {
                    transaction = this.transaction,
                    initialBook = bsb,
                    recipientBooks = reci.BookShelves.FirstOrDefault().BookShelfBooks,
                    initiatorBooks = init.BookShelves.FirstOrDefault().BookShelfBooks

                }) ;
            }

            else
            {
                return this.RedirectToHome() as IActionResult;
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddBookToRecipientBooksAsync(string id)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                Guid bookId;
                if(Guid.TryParse(id, out bookId))
                await this.initializeTransactionService.AddBookToRecipientBooks(Guid.Parse(id), this.transaction);
                return Json(true);   
            }
            return Json(false);
        }



    }
}
