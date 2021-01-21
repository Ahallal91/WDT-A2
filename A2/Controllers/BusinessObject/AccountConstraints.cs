using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Controllers.BusinessObject
{
    /// <summary>
    /// AccountConstraints class provides all constraint values for transaction charges, freeTransactionLimit, and the opening and minimum balances of 
    /// different account types.
    /// </summary>
    public class AccountConstraints
    {
        public readonly decimal withdrawCharge = 0.10M;
        public readonly decimal transferCharge = 0.20M;
        public readonly int freeTransactionLimit = 4;

        public AccountConstraints(string accountType)
        {
            SetAccount(accountType);
        }

        public decimal OpeningBalance { get; private set; }
        public decimal MinBalance { get; private set; }

        private void SetAccount(string accountType)
        {
            switch (accountType)
            {
                case "S":
                    OpeningBalance = 100;
                    MinBalance = 0;
                    break;
                case "C":
                    OpeningBalance = 500;
                    MinBalance = 200;
                    break;
                default:
                    break;
            }
        }
    }
}
