using A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Util
{
    public static class Utilities
    {
        public static Account ValidateAccount(Customer customer, int accountNumber)
        {
            Account account = null;
            foreach (var acc in customer.Accounts)
            {
                if (acc.AccountNumber == accountNumber)
                {
                    account = acc;
                }
            }

            return account;
        }
    }
}
