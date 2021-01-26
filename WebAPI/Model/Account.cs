using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Model
{
    public partial class Account
    {
        public Account()
        {
            BillPays = new HashSet<BillPay>();
            TransactionAccountNumberNavigations = new HashSet<Transaction>();
            TransactionDestinationAccountNavigations = new HashSet<Transaction>();
        }

        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<BillPay> BillPays { get; set; }
        public virtual ICollection<Transaction> TransactionAccountNumberNavigations { get; set; }
        public virtual ICollection<Transaction> TransactionDestinationAccountNavigations { get; set; }
    }
}
