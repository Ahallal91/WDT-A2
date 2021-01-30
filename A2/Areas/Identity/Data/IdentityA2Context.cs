using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.Areas.Identity.Data;
using A2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace A2.Data
{
    public class IdentityA2Context : IdentityDbContext<A2User>
    {
        public IdentityA2Context(DbContextOptions<IdentityA2Context> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<BillPay> BillPay { get; set; }
        public DbSet<Payee> Payee { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public object Customers { get; internal set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            // constraints

            // old context
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
