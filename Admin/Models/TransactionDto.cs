using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class TransactionDto
    {
        [Required]
        public int TransactionID { get; set; }
        [Required]
        [StringLength(1)]
        [RegularExpression(@"^[DWTSB]$", ErrorMessage = "Enter a valid transaction type")]
        public string TransactionType { get; set; }
        [Required]
        public int AccountNumber { get; set; }
        [RegularExpression(@"^\d{4}?$", ErrorMessage = "Account numbers must be 4 digits")]
        public int? DestinationAccount { get; set; }
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]*(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal? Amount { get; set; }
        [StringLength(255)]
        public string Comment { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
