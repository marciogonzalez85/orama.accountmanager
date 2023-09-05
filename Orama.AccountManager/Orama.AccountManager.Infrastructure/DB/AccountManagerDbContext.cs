using Microsoft.EntityFrameworkCore;
using Orama.AccountManager.Infrastructure.DB.Configurations;
using Orama.AccountManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Infrastructure.DB
{
    public class AccountManagerDbContext : DbContext
    {
        public AccountManagerDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<FinancialAsset> FinancialAssets { get; set; }
        public DbSet<FinancialTransaction> FinancialTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinancialTransactionsConfiguration).Assembly);

            #region Seed
            modelBuilder.Entity<Cliente>()
                .HasData(new Cliente
                {
                    Id = 1,
                    Name = "Cliente A",
                    Email = "cliente_a@email.com",
                    Password = "password",
                    WebhookUrl = "http://localhost:1234/notification/"
                });

            modelBuilder.Entity<BankAccount>()
                .HasData(new BankAccount
                {
                    Id = 1,
                    ClienteId = 1,
                    Balance = 1000
                });

            modelBuilder.Entity<FinancialAsset>()
                .HasData(new FinancialAsset
                {
                    Id = 1,
                    Name = "AXZ Stock",
                    Price = 15
                });
            #endregion
        }
    }
}
