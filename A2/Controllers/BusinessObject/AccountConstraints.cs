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

        /// <summary>
        /// AcccountConstraints class deals with specific charges on transactions, freetransactionlimits and minbalance and openingbalance
        /// of individual account types. This allows correct charges to be applied to the right accounts.
        /// </summary>
        /// <param name="accountType">The type of account you want to get parameters for.</param>
        public AccountConstraints(string accountType)
        {
            SetAccount(accountType);
        }

        public decimal OpeningBalance { get; private set; }
        public decimal MinBalance { get; private set; }
        /// <summary>
        /// Sets the openingBalance and minBalance constraints of an accountType which can be used for future checks.
        /// </summary>
        /// <param name="accountType">The type of account you want to get parameters for.</param>
        private void SetAccount(string accountType)
        {
            const string savings = "S";
            const string checking = "C";
            switch (accountType)
            {
                case savings:
                    OpeningBalance = 100;
                    MinBalance = 0;
                    break;
                case checking:
                    OpeningBalance = 500;
                    MinBalance = 200;
                    break;
                default:
                    break;
            }
        }
    }
}
