using A2.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*
 * Reference McbaExampleWithLogin Login.cs week 6
 */

namespace A2.Models
{
    /// <summary>
    /// Login Record - Please see Readme for full justification of the login model as a record. 
    /// To ensure login information is immutable in the system unless specifically specified (such as updated password)
    /// a record form of Login Model is more secure and prevents the classes attributes changing unnecessarily.
    /// </summary>
    /// 
    public enum ActiveType
    {
        Unblocked = 1,
        Blocked = 2
    }
    public record Login
    {
        [Required]
        [StringLength(8)]
        [RegularExpression(@"^\d{8}?$", ErrorMessage = "Login ID must be 8 digits.")]
        [Display(Name = "Login ID")]
        public string LoginID { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Required]
        [StringLength(64)]
        [ValidPasswordLength]
        public string PasswordHash { get; set; }

        [Required, StringLength(20)]
        public DateTime ModifyDate { get; set; }
        public ActiveType Status { get; set; }
    }
}