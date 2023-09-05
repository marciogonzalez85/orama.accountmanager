using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orama.AccountManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Infrastructure.DB.Configurations
{
    public class ClientesConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(k => k.Id);

            builder
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .HasMany(m => m.BankAccounts)
                .WithOne(o => o.Cliente)
                .HasForeignKey(o => o.ClienteId);
        }
    }
}
