using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public string TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int? DestinationAccount { get; set; }
        public decimal? Amount { get; set; }
        public string Comment { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual Account AccountNumberNavigation { get; set; }
        public virtual Account DestinationAccountNavigation { get; set; }
    }
}
