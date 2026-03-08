using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.DataAccess.Config
{
    public class IRFDealConfig : IEntityTypeConfiguration<IRFDeal>
    {
        public void Configure(EntityTypeBuilder<IRFDeal> builder)
        {
            builder.ToTable("IRFDeals");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            //builder.HasOne(n => n.SaleListing)
            //   .WithOne(x=>x.IRFDeal)
            //   .HasForeignKey<IRFDeal>(s=>s.SaleListingId)
            //   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
