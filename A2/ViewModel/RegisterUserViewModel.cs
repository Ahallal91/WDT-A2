using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace A2.ViewModel
{
    public class RegisterUserViewModel
    {

        [Required]
        [StringLength(8)]
        [RegularExpression(@"^\d{8}?$", ErrorMessage = "Login ID must be 8 digits.")]
        [Display(Name = "Login ID")]
        public string LoginID { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Ente a valid name")]
        [StringLength(50)]
        public string CustomerName { get; set; }
        [Required]
        [RegularExpression(@"^[+]?(61)\s\d{4}\s\d{4}$", ErrorMessage = "Enter a valid phone number.")]
        [StringLength(15)]
        public string Phone { get; set; }
        [Required]
        [StringLength(1)]
        [Display(Name = "Type")]
        [RegularExpression(@"^[SC]$", ErrorMessage = "Enter a valid account type")]
        public string AccountType { get; set; }
        [Required]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^[0-9]*(\.[0-9][0-9]?)?$", ErrorMessage = "Currency must be greater than zero and to two decimal places.")]
        public decimal Balance { get; set; }

    }
}
