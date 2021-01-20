using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A2.Models
{
    public record Login
    {
        [Required, StringLength(8)]
        [RegularExpression(@"^\d{8}?$", ErrorMessage = "Login ID must be 8 digits.")]
        [Display(Name = "Login ID")]
        public string LoginID { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

        [Required, StringLength(20)]
        public DateTime ModifyDate { get; set; }
    }
}