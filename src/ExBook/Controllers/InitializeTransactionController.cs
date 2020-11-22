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
using ExBook.Models.FinalizeTransaction;

namespace ExBook.Controllers
{
    public class InitializeTransactionController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly SearchService searchService;
        private readonly InitializeTransactionService initializeTransactionService;

        public InitializeTransactionController(ApplicationDbContext applicationDbContext,SearchService searchService, InitializeTransactionService initializeTransactionService)
        {
            this.applicationDbContext = applicationDbContext;
            this.searchService = searchService;
            this.initializeTransactionService = initializeTransactionService;
            
        }

        [HttpPost]
        [Route("/initializetransaction")]
        public async Task<IActionResult> Index(Guid id)
        {   
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Guid InitiatorId = this.HttpContext.User.GetId().Value;
                BookShelfBook bsb = await this.initializeTransactionService.GetBookShelfBookById(id);
                var init = await this.initializeTransactionService.GetUserById(InitiatorId);
                var reci = await this.initializeTransactionService.GetUserById(bsb.BookShelf.UserId);

                Transaction transaction = new Transaction();
                transaction.Id = Guid.NewGuid();             
                transaction.Initiator = init;
                transaction.Recipient = reci;
                transaction.InitiatorId = InitiatorId;
                transaction.RecipientId = bsb.BookShelf.UserId;
                transaction.Status = "initialized";
                transaction.RecipientBooks.Add(bsb);

                await this.applicationDbContext.AddAsync(transaction);
                this.applicationDbContext.SaveChanges();

                return this.View(new InitializeTransactionViewModel()
                {
                    transaction = transaction,
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


      


        #region AddTransactionBooks

        [HttpPost]
        public async Task<JsonResult> AddToInitiatorBooksAsync(string idb, string idt)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                Guid bookId;
                Guid transactionID;

                if (Guid.TryParse(idb, out bookId) && Guid.TryParse(idt, out transactionID))
                {
                    await this.initializeTransactionService.AddBookToInitiatorBooks(Guid.Parse(idb), Guid.Parse(idt));
                    return Json(true);
                }
            }
            return Json(false);
        }

        [HttpPost]
        public async Task<JsonResult> AddToRecipientBooksAsync(string idb, string idt)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                Guid bookId;
                Guid transactionID;

                if(Guid.TryParse(idb, out bookId) && Guid.TryParse(idt, out transactionID))
                {
                        await this.initializeTransactionService.AddBookToRecipientBooks(Guid.Parse(idb), Guid.Parse(idt));
                        return Json(true);
                }
            }
            return Json(false);
        }

        #endregion AddTransactionBooks
    }

    
}
