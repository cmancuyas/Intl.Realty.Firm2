using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.DataAccess.Config
{
    public class SaleCoopConfig : IEntityTypeConfiguration<SaleCoop>
    {
        public void Configure(EntityTypeBuilder<SaleCoop> builder)
        {
            builder.ToTable("SaleCoops");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.TransactionTypeId).IsRequired();
            builder.Property(n => n.IRFDealId).IsRequired();
            builder.Property(n => n.DealStatusId).IsRequired();
            builder.Property(n => n.IsActive).IsRequired();
            builder.Property(n => n.IsDeleted).IsRequired();
            builder.Property(n => n.CreatedBy).IsRequired();
            builder.Property(n => n.CreatedAt).IsRequired();

            builder.HasOne(n => n.TransactionType)
                   .WithMany()
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.IRFDeal)
                   .WithMany()
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.DealStatus)
                   .WithMany()
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
