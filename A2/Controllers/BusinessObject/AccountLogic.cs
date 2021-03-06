using A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Controllers.BusinessObject
{
    /// <summary>
    /// AccountLogic class provides all logical operations for the CustomerController which deals specifically with accounts.
    /// </summary>
    public class AccountLogic
    {
        /// <summary>
        /// Deposit method: Adds the deposit value to the passed in accounts balance as long as its
        /// greater than zero, and creates a deposit transaction on the transaction list of the account.
        /// </summary>
        /// <param name="value">is the amount to deposit.</param>
        /// <param name="account">account to deposit to</param>
        public Account Deposit(decimal value, Account account)
        {
            const string depositTransaction = "D";
            if (value > 0)
            {
                account.Balance += value;
                account.Transactions.Add(new Transaction
                {
                    TransactionType = depositTransaction,
                    AccountNumber = account.AccountNumber,
                    Amount = value,
                    Comment = "Deposit",
                    ModifyDate = DateTime.UtcNow
                });
            }
            return account;
        }
        /// <summary>
        /// Withdraw method: Removes the Withdraw value to the passed in accounts balance as long as its
        /// greater than zero, and creates a Withdraw transaction on the transaction list of the account.
        /// If account has used its freetransactions, the additional service charge will apply.
        /// </summary>
        /// <param name="value">is the amount to withdraw.</param>
        /// <param name="account">account to withdraw from</param>
        /// <returns> account which withdrew, if null is returned the transaction failed due to insufficient balance</returns>
        public Account Withdraw(decimal value, Account account)
        {
            AccountConstraints constraints = new AccountConstraints(account.AccountType);
            const string withdrawTransaction = "W";
            if (value > 0)
            {
                if (ComputeTransation(value, account, constraints, constraints.withdrawCharge))
                {
                    account.Transactions.Add(new Transaction
                    {
                        TransactionType = withdrawTransaction,
                        AccountNumber = account.AccountNumber,
                        Amount = value,
                        Comment = "Withdraw",
                        ModifyDate = DateTime.UtcNow
                    });
                }
                else
                {
                    return null;
                }
            }
            return account;
        }
        /// <summary>
        /// Transfer method: Removes the Transfer value to the passed in accounts balance as long as its
        /// greater than zero, and creates a Transfer transaction on the transaction list of the account.
        /// The amount is then added to the account being transferred to and they also have a transaction added.
        /// If account has used its freetransactions, the additional service charge will apply to the
        /// transferring account only.
        /// </summary>
        /// <param name="value">is the amount to transfer.</param>
        /// <param name="account">account to transfer from</param>
        /// <param name="toAccountNumber">account to transfer to</param>
        /// <param name="comment">comment for the transfer transaction</param>
        /// <returns> the account, if null is returned the transaction failed due to insufficient balance.</returns>
        public Account Transfer(decimal value, Account account, Account toAccountNumber, string comment)
        {
            AccountConstraints constraints = new AccountConstraints(account.AccountType);
            const string transferTransaction = "T";
            if (value > 0)
            {
                if (ComputeTransation(value, account, constraints, constraints.transferCharge))
                {
                    toAccountNumber.Balance += value;
                    account.Transactions.Add(new Transaction
                    {
                        TransactionType = transferTransaction,
                        AccountNumber = account.AccountNumber,
                        DestinationAccount = toAccountNumber.AccountNumber,
                        Amount = value,
                        Comment = comment,
                        ModifyDate = DateTime.UtcNow
                    });
                    toAccountNumber.Transactions.Add(new Transaction
                    {
                        TransactionType = transferTransaction,
                        AccountNumber = toAccountNumber.AccountNumber,
                        Amount = value,
                        Comment = comment,
                        ModifyDate = DateTime.UtcNow
                    });
                }
                else
                {
                    return null;
                }
            }
            return account;
        }
        /// <summary>
        /// As long as account has previous transactions, this method loads in a list of transactions.
        /// They must match the account number of the account. Withdraw and Sent Transfers are added to FreeTransactionNumber.
        /// </summary>
        /// <returns>The amount of freetransactions used</returns>
        private static int InsertPreviousTransactions(Account account)
        {
            int freeTransactions = 0;
            if (account.Transactions.Count != 0)
            {
                foreach (var transaction in account.Transactions)
                {
                    if (transaction.TransactionType == "W")
                    {
                        freeTransactions++;
                    }
                    else if (transaction.TransactionType == "T" && transaction.DestinationAccount > 0)
                    {
                        freeTransactions++;
                    }
                }
            }
            return freeTransactions;
        }
        /// <summary>
        /// computes transactions on account and returns true if balance was successfully deduced.
        /// false returns mean balance is insufficient for transation.This method is useable where
        /// deductions on balance are required such as transfer and withdraw. Fees are only charged
        /// if freetransactionNumber exceeds the value specified in AccountConstraints class.
        /// Contract: It is expected that exception handling with be used outside of this method to handle false cases.
        /// </summary>
        /// <param name="value">amount to deduced from the Balance.</param>
        /// <param name="minimumAmount">the minimum amount the account can be set to. Eg Savings $0 </param>
        /// <param name="charge">The type of transaction charge to be applied. 
        /// Eg Withdraw transactions have a withdraw charge</param>
        /// <returns> <c>true</c> if Transaction was successful, otherwise <c>false</c>.</returns>
        private bool ComputeTransation(decimal value, Account account, AccountConstraints constraints, decimal charge)
        {
            bool retValue = false;
            if ((account.Balance - value) >= constraints.MinBalance && InsertPreviousTransactions(account) < constraints.freeTransactionLimit)
            {
                account.Balance -= value;
                retValue = true;
            }
            else if (InsertPreviousTransactions(account) >= constraints.freeTransactionLimit
                && (account.Balance - (value + charge)) >= constraints.MinBalance)
            {
                account.Balance -= (value + charge);
                ServiceCharge(charge, "Service Charge", ref account);
                retValue = true;
            }
            return retValue;
        }
        /// <summary>
        /// ServiceCharge method records a service charge transaction and adds it to the Transactions list.
        /// </summary>
        /// <param name="charge">The amount being charged.</param>
        /// <param name="comment">The type of service charge being applied.</param>
        /// <param name="account">The account where the service charge is added</param>
        private void ServiceCharge(decimal charge, string comment, ref Account account)
        {
            const string serviceChargeTransaction = "S";
            account.Transactions.Add(new Transaction
            {
                TransactionType = serviceChargeTransaction,
                AccountNumber = account.AccountNumber,
                Amount = charge,
                Comment = comment,
                ModifyDate = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Compute bill pay takes in a value and an account, calculates if the bill pay transaction can proceed
        /// (checks balance), if it can it minuses the value of the billpay from the account and adds a B transaction
        /// to the accounts transaction list, it calls CalculateNextBillPay to calculate billpays of future payments,
        /// and set current payments to complete. If insufficient balance on the account, then billpay is set to rejected status.
        /// </summary>
        /// <param name="billPay">The billpay to compute</param>
        /// <param name="account">The account that owns the bill pay </param>
        /// Eg Withdraw transactions have a withdraw charge</param>
        /// <returns>account that computed the transaction</returns>
        public Account ComputeBillPay(BillPay billPay, Account account)
        {
            AccountConstraints constraints = new AccountConstraints(account.AccountType);
            const string billPayTransaction = "B";

            if ((account.Balance - billPay.Amount) >= constraints.MinBalance)
            {
                account.Balance -= billPay.Amount;
                account.Transactions.Add(new Transaction
                {
                    TransactionType = billPayTransaction,
                    AccountNumber = account.AccountNumber,
                    Amount = billPay.Amount,
                    Comment = "BillPay",
                    ModifyDate = DateTime.UtcNow
                });
                CalculateNextBillPay(billPay, ref account);
                return account;
            }
            account.BillPay.Find(x => x.BillPayID == billPay.BillPayID).Status = StatusType.Failed;
            return account;
        }
        /// <summary>
        /// CalculateNextBillPay sets the current bill pay to complete, if the billpay is monthly or quarterly it 
        /// adds a new bill pay of the same data to the account for the specified time.
        /// </summary>
        /// <param name="billPay">is the amount to transfer.</param>
        /// <param name="account">account to transfer from</param>
        private void CalculateNextBillPay(BillPay billPay, ref Account account)
        {
            const string oneTime = "S";
            const string monthly = "M";
            const string quarterly = "Q";
            if (billPay.Period == oneTime)
            {
                account.BillPay.Find(x => x.BillPayID == billPay.BillPayID).Status = StatusType.Complete;
            }
            else if (billPay.Period == monthly)
            {
                account.BillPay.Find(x => x.BillPayID == billPay.BillPayID).Status = StatusType.Complete;
                account.BillPay.Add(new BillPay()
                {
                    AccountNumber = billPay.AccountNumber,
                    PayeeID = billPay.PayeeID,
                    Amount = billPay.Amount,
                    ScheduleDate = billPay.ScheduleDate.AddMonths(1),
                    Period = billPay.Period,
                    ModifyDate = DateTime.UtcNow,
                    Status = StatusType.Awaiting
                });
            }
            else if (billPay.Period == quarterly)
            {
                account.BillPay.Find(x => x.BillPayID == billPay.BillPayID).Status = StatusType.Complete;
                account.BillPay.Add(new BillPay()
                {
                    AccountNumber = billPay.AccountNumber,
                    PayeeID = billPay.PayeeID,
                    Amount = billPay.Amount,
                    ScheduleDate = billPay.ScheduleDate.AddMonths(3),
                    Period = billPay.Period,
                    ModifyDate = DateTime.UtcNow,
                    Status = StatusType.Awaiting
                });
            }
        }
    }
}
