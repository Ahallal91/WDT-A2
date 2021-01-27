using A2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A2.ViewModel
{
    public class BillPaysViewModel
    {
        private DateTime scheduledDate;
        public int BillPayID { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Account numbers must be 4 digits")]
        public int AccountNumber { get; set; }
        public int PayeeID { get; set; }
        public string PayeeName { get; set; }
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]*(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal Amount { get; set; }
        [RegularExpression(@"^[MQS]$", ErrorMessage = "Invalid payment period.")]
        public string Period { get; set; }
        public DateTime ScheduleDate
        {
            get
            {
                return this.scheduledDate;
            }
            set
            {
                this.scheduledDate = value.ToLocalTime();
            }
        }
        public StatusType Status { get; set; }
    }
}
