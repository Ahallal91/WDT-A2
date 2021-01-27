using Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.ViewModel
{
    public class TransactionViewModel
    {
        [RegularExpression(@"^\d{4}$", ErrorMessage = "CustomerID must be 4 digits long.")]
        public int CustomerID { get; set; }
        public List<AccountDto> Accounts { get; set; }
        public List<TransactionDto> Transactions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
