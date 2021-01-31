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
        public List<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }

        public List<Transaction> GetAllByID(int id)
        {
            return _context.Transactions.Where(x => x.TransactionId == id).ToList();
        }

        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;
        }
    }
}
