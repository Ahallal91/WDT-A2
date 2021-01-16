using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A2.Models
{
    public enum Period
    {
        Monthly = 'M',
        Quarterly = 'Q',
        OnceOff = 'S'
    };
    public class BillPay
    {
        [Required]
        public int BillPayID { get; set; }
        [Required]
        [ForeignKey("Account")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }
        [Required]
        [ForeignKey("PayeeID")]
        public int PayeeID { get; set; }
        public virtual Payee PayeeIDNo { get; set; }
        [Required]
        public DateTime ScheduleDate { get; set; }
        [Required]
        [StringLength(1)]
        public Period Period { get; set; }
        [Required]
        public DateTime ModifyDate { get; set; }
    }
}
