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
    public class FinancialTransactionsConfiguration : IEntityTypeConfiguration<FinancialTransaction>
    {
        public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
        {
            builder.HasKey(k => k.Id);

            builder
                .Property(k => k.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
        }
    }
}
