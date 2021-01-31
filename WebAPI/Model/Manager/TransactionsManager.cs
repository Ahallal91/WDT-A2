using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Model.Repository;

namespace WebAPI.Model.Manager
{
    /*
 * Reference: Week 9 WebAPI
 * Transaction Manager implements IDatarepository so that Admin can get all transactions
 */
    public class TransactionsManager : IDataRepository<Transaction, int>
    {
        private readonly s3811836_a2Context _context;

        public TransactionsManager(s3811836_a2Context context)
        {
            _context = context;
        }
        /// <summary>
        /// Allows Admin project to get all transactions of all users to display on the transactions page.
        /// </summary>
        /// <returns>A List of Transactions of all the transactions available.</returns>
        public List<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }
        /// <summary>
        /// Allows Admin project to get all transactions of all users to display on the transactions page by transaction id
        /// This code was written in-case future use was needed for getting transactions by id.
        /// </summary>
        /// <returns>A List of Transactions that match the id passed in.</returns>
        public List<Transaction> GetAllByID(int id)
        {
            return _context.Transactions.Where(x => x.TransactionId == id).ToList();
        }
        /// <summary>
        /// Allows Admin project to get all transactions by a CustomerID and within a certain start and endDate.
        /// This allows the filtering for transactions in the admin project to occur without much logic on the admin project side
        /// as all filtering is done in the API.
        /// </summary>
        /// <returns>A List of Transactions within the specified date and that match the customer ID</returns>
        public List<Transaction> GetAllByIDWithDate(int id, DateTime startDate, DateTime endDate)
        {
            var customer = _context.Customers.Include(x => x.Accounts)
                .ThenInclude(x => x.TransactionAccountNumberNavigations)
                .FirstOrDefaultAsync(x => x.CustomerId == id).Result;

            var returnTransactions = new List<Transaction>();
            foreach (var acc in customer.Accounts)
            {
                foreach (var transact in acc.TransactionAccountNumberNavigations)
                {
                    if (DateTime.TryParse(transact.ModifyDate.ToString(), out DateTime date))
                    {
                        // add 1 day to end date to ensure that it includes dates selected as time not included in front-end.
                        if (date.ToLocalTime().CompareTo(startDate) >= 0 && date.ToLocalTime().CompareTo(endDate.AddDays(1)) <= 0)
                        {
                            returnTransactions.Add(transact);
                        }
                    }
                }

            }
            return returnTransactions;
        }
        /// <summary>
        /// Allows Admin project to update a transaction by the transaction id, not currently implemented in the project
        /// but may be used in the future.
        /// </summary>
        /// <returns>the id that was updated</returns>
        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;
        }
    }
}
