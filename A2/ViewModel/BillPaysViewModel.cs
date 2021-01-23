using A2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A2.ViewModel
{
    public class BillPaysViewModel
    {
        private DateTime scheduledDate;
        public int BillPayID { get; set; }
        public int AccountNumber { get; set; }
        public int PayeeID { get; set; }
        public string PayeeName { get; set; }
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        public string Period { get; set; }
        public DateTime ScheduleDate
        {
            get
            {
                return this.scheduledDate;
            }
            set
            {
                this.scheduledDate = value.ToLocalTime();
            }
        }
        public StatusType Status { get; set; }
    }
}
