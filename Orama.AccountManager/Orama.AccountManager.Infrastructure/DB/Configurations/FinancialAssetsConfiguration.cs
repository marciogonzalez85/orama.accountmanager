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
    public class FinancialAssetsConfiguration : IEntityTypeConfiguration<FinancialAsset>
    {
        public void Configure(EntityTypeBuilder<FinancialAsset> builder)
        {
            builder.HasKey(k => k.Id);

            builder
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .HasMany(m => m.FinancialTransactions)
                .WithOne(o => o.FinancialAsset)
                .HasForeignKey(f => f.FinancialAssetId);
        }
    }
}
