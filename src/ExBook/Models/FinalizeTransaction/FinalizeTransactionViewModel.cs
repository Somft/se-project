using ExBook.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Models.FinalizeTransaction
{
    public class FinalizeTransactionViewModel
    {
        public Data.Transaction transaction { get; set; }
    }
}
