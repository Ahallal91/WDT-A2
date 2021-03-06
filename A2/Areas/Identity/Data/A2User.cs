using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using A2.Models;
using Microsoft.AspNetCore.Identity;

namespace A2.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the A2User class
    public enum ActiveType
    {
        Unlocked = 1,
        Locked = 2
    }
    public class A2User : IdentityUser
    {
        [ForeignKey("Customer")]
        public int? CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        // the last time the account was modified - including status.
        [Required]
        public DateTime ModifyDate { get; set; }
        [Required]
        public ActiveType Status { get; set; }
    }
}
