using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A2.Models
{
    public class Payee
    {
        [Required]
        public int PayeeID { get; set; }
        [Required]
        [StringLength(50)]
        public string PayeeName { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        [StringLength(40)]
        public string City { get; set; }
        [RegularExpression(@"^(?-i:NSW|QLD|SA|TAS|VIC)$", ErrorMessage = "Enter a valid State")]
        [StringLength(20)]
        public string State { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Enter a valid 4 digit Postcode")]
        [StringLength(10)]
        public string PostCode { get; set; }
        [Required]
        [RegularExpression(@"^[+]?(61)\s\d{4}\s\d{4}$", ErrorMessage = "Enter a valid phone number.")]
        [StringLength(15)]
        public string Phone { get; set; }
    }
}
