using A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Controllers.BusinessObject
{
    public class AccountLogic
    {
        /// <summary>
        /// Deposit method: The value passed in is the amount to be deposited and added onto the Balance of the account.
        /// Deposited values must be greater than zero.
        /// </summary>
        /// <param name="value">is the amount to deposit.</param>

        public Account Deposit(decimal value, Account account)
        {
            string depositTransaction = "D";
            if (value > 0)
            {
                account.Balance += value;
                account.Transactions.Add(new Transaction
                {
                    TransactionType = depositTransaction,
                    AccountNumber = account.AccountNumber,
                    Amount = value,
                    Comment = "Deposit",
                    ModifyDate = DateTime.Now
                });
            }
            return account;
        }
        /// <summary>
        /// Withdraw method: The value passed in is the amount to be deposited and added onto the Balance of the account.
        /// Deposited values must be greater than zero.
        /// </summary>
        /// <param name="value">is the amount to deposit.</param>

        public Account Withdraw(decimal value, Account account)
        {
            AccountConstraints constraints = new AccountConstraints(account.AccountType);
            string withdrawTransaction = "W";
            if (value > 0)
            {
                if (ComputeTransation(value, ref account, constraints, constraints.withdrawCharge))
                {
                    account.Transactions.Add(new Transaction
                    {
                        TransactionType = withdrawTransaction,
                        AccountNumber = account.AccountNumber,
                        Amount = value,
                        Comment = "Withdraw",
                        ModifyDate = DateTime.Now
                    });
                }
            }
            return account;
        }
        /// <summary>
        /// As long as account has no previous transactions, this method loads in a list of transactions.
        /// They must match the account number of the account. Withdraw and Sent Transfers are added to FreeTransactionNumber.
        /// </summary>
        private static int InsertPreviousTransactions(Account account)
        {
            int freeTransactions = 0;
            if (account.Transactions.Count != 0)
            {
                foreach (var transaction in account.Transactions)
                {
                    if (transaction.AccountNumber == account.AccountNumber)
                    {
                        if (transaction.TransactionType == "W")
                        {
                            freeTransactions++;
                        }
                        else if (transaction.TransactionType == "T" && transaction.DestAccount.AccountNumber != 0)
                        {
                            freeTransactions++;
                        }
                    }
                }
            }
            return freeTransactions;
        }
        /// <summary>
        /// computes transactions on account and returns true if balance was successfully deduced.
        /// false returns mean balance is insufficient for transation.This method is useable where
        /// deductions on balance are required such as transfer and withdraw. Fees are only charged
        /// if freetransactionNumber exceeds 4.
        /// Contract: It is expected that exception handling with be used outside of this method to handle false cases.
        /// </summary>
        /// <param name="value">amount to deduced from the Balance.</param>
        /// <param name="minimumAmount">the minimum amount the account can be set to. Eg Savings $0 </param>
        /// <param name="charge">The type of transaction charge to be applied. 
        /// Eg Withdraw transactions have a withdraw charge</param>
        /// <returns> <c>true</c> if Transaction was successful, otherwise <c>false</c>.</returns>
        private bool ComputeTransation(decimal value, ref Account account, AccountConstraints constraints, decimal charge)
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
        private void ServiceCharge(decimal charge, string comment, ref Account account)
        {
            string serviceChargeTransaction = "S";
            account.Transactions.Add(new Transaction
            {
                TransactionType = serviceChargeTransaction,
                AccountNumber = account.AccountNumber,
                Amount = charge,
                Comment = comment,
                ModifyDate = DateTime.Now
            });
        }
    }
}
