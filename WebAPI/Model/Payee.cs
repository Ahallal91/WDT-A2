using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI
{
    public partial class Payee
    {
        public Payee()
        {
            BillPays = new HashSet<BillPay>();
        }

        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<BillPay> BillPays { get; set; }
    }
}
