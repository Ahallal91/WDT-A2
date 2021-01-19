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
        public string TransactionType { get; set; }
        [Required]
        [ForeignKey("Account")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }
        [ForeignKey("DestAccount")]
        public int? DestinationAccount { get; set; }
        public virtual Account DestAccount { get; set; }
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal? Amount { get; set; }
        [StringLength(255)]
        public string Comment { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
