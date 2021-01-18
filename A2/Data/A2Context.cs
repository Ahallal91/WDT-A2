using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.Models;
using Microsoft.EntityFrameworkCore;

namespace A2.Data
{
    public class A2Context : DbContext
    {
        public A2Context(DbContextOptions<A2Context> options) : base(options)
        { }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<BillPay> BillPay { get; set; }
        public DbSet<Payee> Payee { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // constraints
            builder.Entity<Login>().HasCheckConstraint("CH_Login_LoginID", "len(LoginID) = 8").
                HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 64");
            builder.Entity<Account>().HasCheckConstraint("CH_Account_Balance", "Balance >= 0");
            builder.Entity<BillPay>().HasCheckConstraint("CH_BillPay_Amount", "Amount > 0");
            builder.Entity<Transaction>().HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");
            // relations
            builder.Entity<Account>().
                 HasOne(x => x.Customer).WithMany(x => x.Accounts).HasForeignKey(x => x.CustomerID);
            builder.Entity<BillPay>().
                HasOne(x => x.Account).WithMany(x => x.BillPay).HasForeignKey(x => x.AccountNumber);
            builder.Entity<BillPay>().HasOne(x => x.Payee).WithMany(x => x.BillPay).HasForeignKey(x => x.PayeeID);
            builder.Entity<Transaction>().
                HasOne(x => x.Account).WithMany(x => x.Transactions).HasForeignKey(x => x.AccountNumber);
        }
    }
}
