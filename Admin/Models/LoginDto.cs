using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class LoginDto
    {
        public enum ActiveType
        {
            Unblocked = 1,
            Blocked = 2
        }
        public class Login
        {
            [Required]
            [StringLength(8)]
            [RegularExpression(@"^\d{8}?$", ErrorMessage = "Login ID must be 8 digits.")]
            [Display(Name = "Login ID")]
            public string LoginID { get; set; }

            [Required]
            public int CustomerID { get; set; }

            [Required]
            [StringLength(64)]
            public string PasswordHash { get; set; }

            [Required, StringLength(20)]
            public DateTime ModifyDate { get; set; }
            [Required]
            public ActiveType Status { get; set; }
        }
    }
}
