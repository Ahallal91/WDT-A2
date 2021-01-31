using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Model.EditedModels
{
    public class AccountDto
    {
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
        [Required]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]*(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal Balance { get; set; }
        [Required]
        public DateTime ModifyDate { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }
}