using A2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/*
Reference on how to use viewmodel https://docs.microsoft.com/en-us/aspnet/core/mvc/views/overview?view=aspnetcore-5.0
 */
namespace A2.ViewModel
{
    public class ATMViewModel
    {
        public Customer Customer { get; set; }
        [RegularExpression(@"^\d{4}?$", ErrorMessage = "Account numbers must be 4 digits")]
        public string ToAccountNumber { get; set; }
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]?[0-9]?(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}
