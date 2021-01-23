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
        private DateTime scheduledDate;
        public Customer Customer { get; set; }
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "PayeeID must be only numbers.")]
        public string PayeeID { get; set; }
        public List<Payee> Payee { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]*(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal Amount { get; set; }
        [RegularExpression(@"^[MQS]$", ErrorMessage = "Invalid payment period.")]

        public string Period { get; set; }
        public DateTime ScheduledDate
        {
            get
            {
                return this.scheduledDate;
            }
            set
            {
                this.scheduledDate = value;
            }
        }
    }
}
