using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace A2.Models
{
    public enum AccountType
    {
        Savings = 'S',
        Checking = 'C'
    };
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }
        [Display(Name = "Type")]
        public AccountType AccountType { get; set; }
        [Required]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        [Required]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
        [Required, StringLength(20)]
        public DateTime ModifyDate { get; set; }
        public virtual List<Transaction> Transactions { get; set; }
        public virtual List<BillPay> BillPay { get; set; }
    }
}