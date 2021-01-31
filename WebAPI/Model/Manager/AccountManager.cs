using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Model.Repository;


namespace WebAPI.Model.Manager
{
    public class AccountManager : IDataRepository<Account, int>
    {
        private readonly s3811836_a2Context _context;

        public AccountManager(s3811836_a2Context context)
        {
            _context = context;
        }
        /// <summary>
        /// Returns a list of all accounts in the system.
        /// </summary>
        /// <returns>All accounts that are available are returned as a list.</returns>
        public List<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }
        /// <summary>
        /// Allows admin to get a user account by their Customer ID and return it as a list of accounts.
        /// </summary>
        /// <returns>Returns a list of accounts that match the customerID</returns>
        public List<Account> GetAllByID(int id)
        {
            var customer = _context.Customers.Include(x => x.Accounts)
                .ThenInclude(x => x.TransactionAccountNumberNavigations)
                .FirstOrDefaultAsync(x => x.CustomerId == id).Result;
            var returnAccounts = new List<Account>();
            foreach (var acc in customer.Accounts)
            {
                returnAccounts.Add(acc);
            }
            return returnAccounts;
        }
        /// <summary>
        /// Not implemented
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public List<Account> GetAllByIDWithDate(int id, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Allows admin to update a user account. Not currently used in the project.
        /// </summary>
        /// <returns>id of account that was updated.</returns>
        public int Update(int id, Account account)
        {
            _context.Update(account);
            _context.SaveChanges();

            return id;
        }
    }
}
