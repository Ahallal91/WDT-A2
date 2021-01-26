using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Model.Repository;

namespace WebAPI.Model.Manager
{
    public class TransactionsManager : IDataRepository<Transaction, int>
    {
        private readonly s3811836_a2Context _context;

        public TransactionsManager(s3811836_a2Context context)
        {
            _context = context;
        }
        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }

        public IEnumerable<Transaction> GetAllByID(int id)
        {
            return _context.Transactions.ToList().Where(x => x.TransactionId == id);
        }

        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;
        }
    }
}
