using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExBook.Data;
using ExBook.Extensions;
using ExBook.Models.FinalizeTransaction;
using ExBook.Services;

using Microsoft.AspNetCore.Mvc;

namespace ExBook.Controllers
{
    public class FinalizeTransactionController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly FinalizeTransactionService finalizeTransactionService;

        public FinalizeTransactionController(ApplicationDbContext applicationDbContext,FinalizeTransactionService finalizeTransactionService)
        {
            this.applicationDbContext = applicationDbContext;
            this.finalizeTransactionService = finalizeTransactionService;
        }

        [HttpPost]
        [Route("/finalizetransaction")]
        public async Task<IActionResult> Index(Guid id)
        {

            
                var trans = await this.finalizeTransactionService.GetTransactionById(id);
                return this.View(new FinalizeTransactionViewModel()
                {
                    transaction = trans
                }
                );
           
            
              
            
          
        }
    }
}
