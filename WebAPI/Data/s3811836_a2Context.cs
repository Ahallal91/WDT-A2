using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebAPI.Model;

#nullable disable

namespace WebAPI.Data
{
    public partial class s3811836_a2Context : DbContext
    {
        public s3811836_a2Context()
        {
        }

        public s3811836_a2Context(DbContextOptions<s3811836_a2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<BillPay> BillPays { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Payee> Payees { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountNumber);

                entity.ToTable("Account");

                entity.HasIndex(e => e.CustomerId, "IX_Account_CustomerID");

                entity.Property(e => e.AccountNumber).ValueGeneratedNever();

                entity.Property(e => e.AccountType)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CustomerId);
            });

            modelBuilder.Entity<BillPay>(entity =>
            {
                entity.ToTable("BillPay");

                entity.HasIndex(e => e.AccountNumber, "IX_BillPay_AccountNumber");

                entity.HasIndex(e => e.PayeeId, "IX_BillPay_PayeeID");

                entity.Property(e => e.BillPayId).HasColumnName("BillPayID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.PayeeId).HasColumnName("PayeeID");

                entity.Property(e => e.Period)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.BillPays)
                    .HasForeignKey(d => d.AccountNumber);

                entity.HasOne(d => d.Payee)
                    .WithMany(p => p.BillPays)
                    .HasForeignKey(d => d.PayeeId);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId)
                    .ValueGeneratedNever()
                    .HasColumnName("CustomerID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(40);

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.PostCode).HasMaxLength(10);

                entity.Property(e => e.State).HasMaxLength(20);

                entity.Property(e => e.Tfn)
                    .HasMaxLength(11)
                    .HasColumnName("TFN");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.HasIndex(e => e.CustomerId, "IX_Login_CustomerID");

                entity.Property(e => e.LoginId)
                    .HasMaxLength(8)
                    .HasColumnName("LoginID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.CustomerId);
            });

            modelBuilder.Entity<Payee>(entity =>
            {
                entity.ToTable("Payee");

                entity.Property(e => e.PayeeId).HasColumnName("PayeeID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(40);

                entity.Property(e => e.PayeeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.PostCode).HasMaxLength(10);

                entity.Property(e => e.State).HasMaxLength(20);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.HasIndex(e => e.AccountNumber, "IX_Transaction_AccountNumber");

                entity.HasIndex(e => e.DestinationAccount, "IX_Transaction_DestinationAccount");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Comment).HasMaxLength(255);

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.TransactionAccountNumberNavigations)
                    .HasForeignKey(d => d.AccountNumber);

                entity.HasOne(d => d.DestinationAccountNavigation)
                    .WithMany(p => p.TransactionDestinationAccountNavigations)
                    .HasForeignKey(d => d.DestinationAccount);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
