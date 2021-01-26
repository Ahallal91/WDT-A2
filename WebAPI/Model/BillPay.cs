using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Model
{
    public partial class BillPay
    {
        public int BillPayId { get; set; }
        public int AccountNumber { get; set; }
        public int PayeeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string Period { get; set; }
        public DateTime ModifyDate { get; set; }
        public int Status { get; set; }

        public virtual Account AccountNumberNavigation { get; set; }
        public virtual Payee Payee { get; set; }
    }
}
