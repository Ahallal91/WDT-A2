using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Models
{
    public class Transaction
    {
        [Required]
        public int TransactionID { get; set; }
        [Required]
        [StringLength(1)]
        [RegularExpression(@"^(?-i:D|W|T|S|B)?$", ErrorMessage = "Enter a valid transaction type")]
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
        [RegularExpression(@"^([0-9]*)(.[[0-9]+]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal? Amount { get; set; }
        [StringLength(255)]
        public string Comment { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
