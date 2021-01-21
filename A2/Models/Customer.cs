using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*
 * Reference McbaExampleWithLogin Customer.cs week 6
 */
namespace A2.Models
{
    public record Customer
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "CustomerID must be 4 digits long.")]
        public int CustomerID { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Ente a valid name")]
        [StringLength(50)]
        public string CustomerName { get; set; }
        [RegularExpression("[0-9]\\d{10}", ErrorMessage = "Enter a valid 11 digit TFN.")]
        [StringLength(11)]
        public string TFN { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        [StringLength(40)]
        [RegularExpression("^[A-Z][a-z]+$", ErrorMessage = "Enter a valid city")]
        public string City { get; set; }
        [RegularExpression(@"^(?-i:NSW|QLD|SA|TAS|VIC)?$", ErrorMessage = "Enter a valid State.")]
        [StringLength(20)]
        public string State { get; set; }
        [RegularExpression(@"^\d{4}?$", ErrorMessage = "Enter a valid 4 digit Postcode.")]
        [StringLength(10)]
        public string PostCode { get; set; }
        [Required]
        [RegularExpression(@"^[+]?(61)\s\d{4}\s\d{4}$", ErrorMessage = "Enter a valid phone number.")]
        [StringLength(15)]
        public string Phone { get; set; }
        public virtual List<Account> Accounts { get; set; }
    }
}