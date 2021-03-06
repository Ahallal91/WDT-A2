using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
/*
 * Reference McbaExampleWithLogin Account.cs week 6
 */
namespace A2.Models
{
    public class Account
    {
        private DateTime modifyDate;
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Number")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Account numbers must be 4 digits")]
        public int AccountNumber { get; set; }
        [Required]
        [StringLength(1)]
        [Display(Name = "Type")]
        [RegularExpression(@"^[SC]$", ErrorMessage = "Enter a valid account type")]
        public string AccountType { get; set; }
        [Required]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        [Required]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]*(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal Balance { get; set; }
        [Required, StringLength(20)]
        public DateTime ModifyDate
        {
            get
            {
                return this.modifyDate;
            }
            set
            {
                this.modifyDate = value.ToUniversalTime();
            }
        }
        public virtual List<Transaction> Transactions { get; set; }
        public virtual List<BillPay> BillPay { get; set; }
    }
}