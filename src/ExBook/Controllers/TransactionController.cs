using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models.Transactions;
using ExBook.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly InitializeTransactionService initializeTransactionService;

        public TransactionController(ApplicationDbContext applicationDbContext,
            InitializeTransactionService initializeTransactionService)
        {
            this.dbContext = applicationDbContext;
            this.initializeTransactionService = initializeTransactionService;

        }

        [HttpGet]
        [Route("/transation")]
        public async Task<IActionResult> Index(Guid id)
        {
            Guid userId = this.HttpContext.User.GetId()!.Value;

            Transaction transaction = await this.dbContext.Transactions
                .SingleAsync(t => t.Id == id);

            if (userId != transaction.InitiatorId && userId != transaction.RecipientId)
            {
                return this.BadRequest();
            }

            if (userId == transaction.RecipientId && transaction.Status == Transaction.Statuses.Initialized)
            {
                return this.BadRequest();
            }

            return this.View(new TransactionViewModel()
            {
                Transaction = transaction,

                RecipientBooks = transaction.Recipient.BookShelves
                    .SelectMany(b => b.BookShelfBooks)
                    .Where(b => !b.IsLocked && !b.IsRemoved)
                    .Where(b => !transaction.RecipientBooks.Select(b => b.Id).Contains(b.Id))
                    .ToList(),

                InitiatorBooks = transaction.Initiator.BookShelves
                    .SelectMany(b => b.BookShelfBooks)
                    .Where(b => !b.IsLocked && !b.IsRemoved)
                    .Where(b => !transaction.InitiatorBooks.Select(b => b.Id).Contains(b.Id))
                    .ToList()
            });
        }

        [HttpGet]
        [Route("/transactions")]
        public async Task<IActionResult> UserTransactions()
        {
            Guid userId = this.HttpContext.User.GetId()!.Value;

            List<Transaction> transactions = await this.dbContext.Transactions
                    .Include(t => t.Recipient)
                    .Include(t => t.Initiator)
                    .Include(t => t.InitiatorBooks)
                        .ThenInclude(t => t.Book)
                        .AsSplitQuery()
                    .Include(t => t.RecipientBooks)
                        .ThenInclude(t => t.Book)
                        .AsSplitQuery()
                    .Include(t => t.Ratings)
                    .Where(t => t.InitiatorId == userId || t.RecipientId == userId)
                    .ToListAsync();

            return this.View(new UserTransactionsViewModel()
            {
                ToReview = transactions
                    .Where(t => t.RecipientId == userId)
                    .Where(t => t.Status == Transaction.Statuses.Reviewed)
                    .ToList(),

                Waiting = transactions
                    .Where(t => t.InitiatorId == userId)
                    .Where(t => t.Status == Transaction.Statuses.Reviewed)
                    .ToList(),

                Rejected = transactions
                    .Where(t => t.InitiatorId == userId)
                    .Where(t => t.Status == Transaction.Statuses.Rejected)
                    .ToList(),

                Drafts = transactions
                    .Where(t => t.InitiatorId == userId)
                    .Where(t => t.Status == Transaction.Statuses.Initialized)
                    .ToList(),

                Accepted = transactions
                    .Where(t =>
                        (t.InitiatorId == userId && !t.Ratings.Any(r => r.Author == RatingAuthor.Initiator)) ||
                        (t.RecipientId == userId && !t.Ratings.Any(r => r.Author == RatingAuthor.Recipient)))
                    .Where(t => t.Status == Transaction.Statuses.Accepted)
                    .ToList(),
            });
        }

        [HttpPost]
        [Route("/transation/init")]
        public async Task<IActionResult> Initialize(Guid id)
        {
            BookShelfBook book = await this.initializeTransactionService.GetBookShelfBookById(id);

            User initiator = await this.initializeTransactionService.GetUserById(this.HttpContext.User.GetId()!.Value);
            User recipient = await this.initializeTransactionService.GetUserById(book.BookShelf.UserId);

            if (initiator.Id == recipient.Id)
            {
                return this.BadRequest();
            }

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Initiator = initiator,
                Recipient = recipient,
                Status = Transaction.Statuses.Initialized,
                RecipientBooks = new List<BookShelfBook> { book }
            };

            await this.dbContext.AddAsync(transaction);
            this.dbContext.SaveChanges();

            return this.RedirectToAction(nameof(Index), new
            {
                id = transaction.Id,
            });
        }

        [HttpPost]
        [Route("/transation/submit-review")]
        public async Task<IActionResult> SendToReview(Guid transactionId)
        {
            Guid userId = this.HttpContext.User.GetId()!.Value;

            Transaction transaction = await this.dbContext.Transactions
                .SingleAsync(t => t.Id == transactionId);

            if (transaction.Status != Transaction.Statuses.Initialized || userId != transaction.InitiatorId)
            {
                return this.BadRequest();
            }

            transaction.Status = Transaction.Statuses.Reviewed;

            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction("Index", new
            {
                id = transaction.Id,
            });
        }

        [HttpPost]
        [Route("/transation/accept")]
        public async Task<IActionResult> Accept(Guid transactionId)
        {
            Transaction transaction = await this.dbContext.Transactions
                .Include(t => t.RecipientBooks)
                .Include(t => t.InitiatorBooks)
                .SingleAsync(t => t.Id == transactionId);

            transaction.Status = Transaction.Statuses.Accepted;

            foreach (BookShelfBook book in transaction.RecipientBooks.Concat(transaction.InitiatorBooks))
            {
                book.IsLocked = true;
            }

            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction(nameof(Index), new
            {
                id = transaction.Id,
            });
        }

        [HttpPost]
        [Route("/transation/reject")]
        public async Task<IActionResult> Reject(Guid transactionId)
        {
            Transaction transaction = await this.dbContext.Transactions
                .SingleAsync(t => t.Id == transactionId);

            transaction.Status = Transaction.Statuses.Rejected;

            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction(nameof(UserTransactions));
        }

        [HttpPost]
        [Route("/transation/remove")]
        public async Task<IActionResult> Remove(Guid transactionId)
        {
            Transaction transaction = await this.dbContext.Transactions
                .Include(t => t.RecipientBooks)
                .Include(t => t.InitiatorBooks)
                .SingleAsync(t => t.Id == transactionId);

            if (transaction.Status == Transaction.Statuses.Initialized)
            {
                this.dbContext.Transactions.Remove(transaction);
            }
            else
            {
                transaction.Status = Transaction.Statuses.Removed;
            }


            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction(nameof(UserTransactions));
        }

        [HttpPost]
        [Route("/transation/retry")]
        public async Task<IActionResult> Retry(Guid transactionId)
        {
            Transaction transaction = await this.dbContext.Transactions
                .Include(t => t.RecipientBooks)
                .Include(t => t.InitiatorBooks)
                .SingleAsync(t => t.Id == transactionId);

            transaction.Status = Transaction.Statuses.Removed;

            Transaction newTransaction = this.dbContext.Transactions.Add(new Transaction()
            {
                Id = Guid.NewGuid(),
                InitiatorId = transaction.InitiatorId,
                RecipientId = transaction.RecipientId,
                InitiatorBooks = transaction.InitiatorBooks,
                RecipientBooks = transaction.RecipientBooks,
                Status = Transaction.Statuses.Initialized,
            }).Entity;

            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction(nameof(Index), new
            {
                id = newTransaction.Id,
            });
        }


        [HttpPost]
        [Route("/transation/books/recipient/add")]
        public async Task<IActionResult> AddRecipientBooks(Guid transactionId, Guid bookId)
        {
            Transaction transaction = await this.dbContext.Transactions
                .Include(t => t.RecipientBooks)
                .SingleAsync(t => t.Id == transactionId);


            transaction.RecipientBooks.Add(this.dbContext.BookShelfBooks.Find(bookId));

            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction(nameof(Index), new
            {
                id = transaction.Id,
            });
        }

        [HttpPost]
        [Route("/transation/books/recipient/remove")]
        public async Task<IActionResult> RemoveRecipientBooks(Guid transactionId, Guid bookId)
        {
            Transaction transaction = await this.dbContext.Transactions
                .Include(t => t.RecipientBooks)
                .SingleAsync(t => t.Id == transactionId);

            transaction.RecipientBooks.Remove(transaction.RecipientBooks.Where(b => b.Id == bookId).First());

            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction("Index", new
            {
                id = transaction.Id,
            });
        }

        [HttpPost]
        [Route("/transation/books/initiator/add")]
        public async Task<IActionResult> AddInitiatorBooks(Guid transactionId, Guid bookId)
        {
            Transaction transaction = await this.dbContext.Transactions
                .Include(t => t.InitiatorBooks)
                .SingleAsync(t => t.Id == transactionId);


            transaction.InitiatorBooks.Add(this.dbContext.BookShelfBooks.Find(bookId));

            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction("Index", new
            {
                id = transaction.Id,
            });
        }

        [HttpPost]
        [Route("/transation/books/initiator/remove")]
        public async Task<IActionResult> RemoveInitiatorBooks(Guid transactionId, Guid bookId)
        {
            Transaction transaction = await this.dbContext.Transactions
                .Include(t => t.InitiatorBooks)
                .SingleAsync(t => t.Id == transactionId);

            transaction.InitiatorBooks.Remove(transaction.InitiatorBooks.Where(b => b.Id == bookId).First());

            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction("Index", new
            {
                id = transaction.Id,
            });
        }

        [HttpGet]
        [Route("/transation/rate")]
        public async Task<IActionResult> Rating(Guid transactionId)
        {
            Transaction transaction = await this.dbContext.Transactions
              .SingleAsync(t => t.Id == transactionId);

            return this.View(new TransactionRatingViewModel()
            {
                Transaction = transaction,
            });
        }

        [HttpPost]
        [Route("/transation/rate")]
        public async Task<IActionResult> Rating(Guid transactionId, TransactionRatingViewModel request)
        {
            Guid userId = this.HttpContext.User.GetId()!.Value;

            Transaction transaction = await this.dbContext.Transactions
                .Include(t => t.Ratings)
             .SingleAsync(t => t.Id == transactionId);

            if (!this.ModelState.IsValid)
            {
                request.Transaction = transaction;
                return this.View(request);
            }

            RatingAuthor author = transaction.RecipientId == userId
                ? RatingAuthor.Recipient
                : RatingAuthor.Initiator;

            Rating rating = transaction.Ratings.FirstOrDefault(r => r.Author == author)
                ?? this.dbContext.Ratings.Add(new Rating()
                {
                    Id = Guid.NewGuid(),
                    Author = author,
                    Transaction = transaction,

                }).Entity;

            rating.Comment = request.Comments ?? "";
            rating.Value = request.Rating ?? 0;

            await this.dbContext.SaveChangesAsync();

            return this.RedirectToAction(nameof(UserTransactions));
        }
    }
}
