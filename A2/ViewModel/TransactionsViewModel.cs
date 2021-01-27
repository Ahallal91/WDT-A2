using A2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace A2.ViewModel
{
    public class TransactionsViewModel
    {
        public Customer Customer { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Account numbers must be 4 digits")]
        public int AccountNumber { get; set; }
        public IPagedList<Transaction> Transactions { get; set; }
    }
}
