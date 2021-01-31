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
        public List<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }

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

        public List<Account> GetAllByIDWithDate(int id, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public int Update(int id, Account account)
        {
            _context.Update(account);
            _context.SaveChanges();

            return id;
        }
    }
}
