using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
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
        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }

        public IEnumerable<Account> GetAllByID(int id)
        {
            return _context.Accounts.ToList().Where(x => x.AccountNumber == id);
        }

        public int Update(int id, Account account)
        {
            _context.Update(account);
            _context.SaveChanges();

            return id;
        }
    }
}
