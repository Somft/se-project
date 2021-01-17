using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExBook.Data;
using ExBook.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace ExBook.Controllers
{
    public class TransactionController : Controller
    {
        
        [HttpGet]
        [Route("/transaction")]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        [Route("/Initialize")]
        public IActionResult Initialize()
        {
            return this.RedirectToInitializeTransaction();
        }
    }
}
