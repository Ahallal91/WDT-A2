using System;

namespace s3811836_a2.Models
{
    public class Customer
    {
        // [Required]
        // [RegularExpression(@"^\d{8}$", ErrorMessage = "Enter a valid 8 digit ID.")]
        // [StringLength(20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Ente a valid name")]
        [StringLength(50)]
        public string CustomerName { get; set; }

        [RegularExpression ("[0-9]\\d{10}", ErrorMessage = "Enter a valid 11 digit TFN.")]
        [StringLength(20)]
        public string TFN { get; set; }

        [StringLength(60)]
        public string Address { get; set; }


        [StringLength (30)]
        [RegularExpression ("^[A-Z][a-z]+$", ErrorMessage = "Enter a valid city")]
        public string City { get; set; }

        [RegularExpression(@"^(?-i:NSW|QLD|SA|TAS|VIC)$", ErrorMessage = "Enter a valid state")]
        [StringLength(20)]
        public string State { get; set; }

        [RegularExpression(@"^\d{4}$", ErrorMessage = "Enter a valid 4 digit Postcode")]
        [StringLength(10)]
        public string PostCode { get; set; }

        [Required]
        [RegularExpression(@"^[+]?(61)\s\d{4}\s\d{4}$", ErrorMessage = "Enter a valid phone number")]
        [StringLength(15)]
        public string Phone { get; set; }

        public List<Account> Accounts { get; set; }
    }
}