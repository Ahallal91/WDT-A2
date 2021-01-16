using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Models
{
    public enum TransactionType
    {
        Deposit = 'D',
        Withdraw = 'W',
        Transfer = 'T',
        ServiceCharge = 'S',
        BillPay = 'B'
    };
    public class Transaction
    {
        [Required]
        public int TransactionID { get; set; }
        [Required]
        [StringLength(1)]
        public TransactionType TransactionType { get; set; }
        [Required]
        [ForeignKey("Account")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }
        [ForeignKey("DestinationAccount")]
        public int? DestAccount { get; set; }
        public virtual Account DestAccount { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [StringLength(255)]
        public string Comment { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
