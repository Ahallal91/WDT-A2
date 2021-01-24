using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
/*
 * Reference McbaExampleWithLogin Transaction.cs week 6
 */
namespace A2.Models
{
    /// <summary>
    /// Transaction Record - Please see Readme for full justification of the Transaction model as a record. 
    /// When transactions are recorded they should remain in a immutable state to ensure that once a transaction is recorded
    /// it can be uploaded into the database with the exact correct data. There is also an advantage for a banking application to
    /// use a record for transactions as they would usually have many of them recorded. Using a record uses less resources when accessing
    /// transactions in this manner.
    /// </summary>
    public record Transaction
    {
        [Required]
        public int TransactionID { get; set; }
        [Required]
        [StringLength(1)]
        [RegularExpression(@"^[DWTSB]$", ErrorMessage = "Enter a valid transaction type")]
        public string TransactionType { get; set; }
        [Required]
        [ForeignKey("Account")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }
        [ForeignKey("DestAccount")]
        [RegularExpression(@"^\d{4}?$", ErrorMessage = "Account numbers must be 4 digits")]
        public int? DestinationAccount { get; set; }
        public virtual Account DestAccount { get; set; }
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]*(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal? Amount { get; set; }
        [StringLength(255)]
        public string Comment { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
