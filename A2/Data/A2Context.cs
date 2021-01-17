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

        public DbSet<BillPay> BillPay { get; set; }
        public DbSet<Payee> Payee { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BillPay>().
                HasOne(x => x.Account).WithMany(x => x.BillPay).HasForeignKey(x => x.AccountNumber);

            builder.Entity<BillPay>().HasOne(x => x.PayeeIDNo).WithMany(x => x.BillPay).HasForeignKey(x => x.PayeeIDNo);

            builder.Entity<Transaction>().
                HasOne(x => x.Account).WithMany(x => x.Transaction).HasForeignKey(x => x.AccountNumber);
            builder.Entity<BillPay>().HasCheckConstraint("CH_BillPay_Amount", "Amount > 0");
            builder.Entity<Transaction>().HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");
        }
    }
}
