using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI
{
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            AspNetUsers = new HashSet<AspNetUser>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Tfn { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
