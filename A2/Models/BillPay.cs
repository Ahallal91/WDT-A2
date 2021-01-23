using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A2.Models
{
    public enum StatusType
    {
        Awaiting = 1,
        Failed = 2,
        Complete = 3
    }
    public class BillPay
    {
        [Required]
        public int BillPayID { get; set; }
        [Required]
        [ForeignKey("Account")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }
        [Required]
        [ForeignKey("PayeeID")]
        public int PayeeID { get; set; }
        public virtual Payee Payee { get; set; }
        [Required]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]*(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal Amount { get; set; }
        [Required]
        public DateTime ScheduleDate { get; set; }
        [Required]
        [StringLength(1)]
        [RegularExpression(@"^[MQS]$", ErrorMessage = "Invalid payment period.")]
        public string Period { get; set; }
        [Required]
        public DateTime ModifyDate { get; set; }
        public StatusType Status { get; set; }
    }
}
