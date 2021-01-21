using A2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A2.ViewModel
{
    public class PayBillViewModel
    {
        public Customer Customer { get; set; }
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "PayeeID must be only numbers.")]
        public string PayeeID { get; set; }
        public string ToAccountNumber { get; set; }
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]?[0-9]?(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal Amount { get; set; }
        [RegularExpression(@"^(?-i:M|Q|S)?$", ErrorMessage = "Enter a valid State.")]
        public string Period { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}
