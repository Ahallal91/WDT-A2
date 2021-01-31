﻿using Microsoft.EntityFrameworkCore;
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
                    var date = DateTime.Parse(transact.ModifyDate.ToString());
                    if (date.CompareTo(startDate) >= 0 && date.CompareTo(endDate) <= 0)
                    {
                        returnTransactions.Add(transact);
                    }
                }

            }
            return returnTransactions;
        }

        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;
        }
    }
}
