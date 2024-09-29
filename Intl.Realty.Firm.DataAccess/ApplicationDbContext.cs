using Intl.Realty.Firm.DataAccess.Config;
using Intl.Realty.Firm.Model.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Intl.Realty.Firm.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<DealTransactionNumberRange> DealTransactionNumberRanges { get; set; }
        public DbSet<InternalEmail> InternalEmails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<EmploymentStatus> EmploymentStatuses { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DealStatus> DealStatuses { get; set; }
        public DbSet<DocumentTypeAssignment> DocumentTypeAssignments { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<IRFDeal> IRFDeals { get; set; }
        public DbSet<SaleListing> SaleListings { get; set; }
        public DbSet<SaleCoop> SaleCoops { get; set; }
        public DbSet<LeaseListing> LeaseListings { get; set; }
        public DbSet<LeaseCoop> LeaseCoops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Location Module
            modelBuilder.ApplyConfiguration(new SaleListingConfig());
            modelBuilder.ApplyConfiguration(new SaleCoopConfig());
            modelBuilder.ApplyConfiguration(new LeaseListingConfig());
            modelBuilder.ApplyConfiguration(new LeaseCoopConfig());
            modelBuilder.ApplyConfiguration(new IRFDealConfig());

            modelBuilder.Entity<EmploymentStatus>().HasData(
            new EmploymentStatus { Id = 1, Code = "1", Description = "Active" },
            new EmploymentStatus { Id = 2, Code = "2", Description = "Inactive" },
            new EmploymentStatus { Id = 3, Code = "3", Description = "Blacklisted" }
                );

            modelBuilder.Entity<DealStatus>().HasData(
            new DealStatus { Id = 1, Code = "1", Description = "Saved" },
            new DealStatus { Id = 2, Code = "2", Description = "Submitted" }
            );

            modelBuilder.Entity<TransactionType>().HasData(
            new TransactionType { Id = 1, Code = "1", Description = "SaleListing" },
            new TransactionType { Id = 2, Code = "2", Description = "SaleCoop" },
            new TransactionType { Id = 3, Code = "3", Description = "LeaseListing" },
            new TransactionType { Id = 4, Code = "4", Description = "LeaseCoop" }
            );

            modelBuilder.Entity<DocumentType>().HasData(
            new DocumentType { Id = 1, Code = "1", Description = "Agreement of Purchase Sale" },
            new DocumentType { Id = 2, Code = "2", Description = "Schedule A" },
            new DocumentType { Id = 3, Code = "3", Description = "Schedule B" }
            );
            modelBuilder.Entity<DocumentTypeAssignment>().HasData(
            new DocumentTypeAssignment { Id = 1, TransactionTypeId = 1, DocumentTypeId = 1 },
            new DocumentTypeAssignment { Id = 2, TransactionTypeId = 1, DocumentTypeId = 2 },
            new DocumentTypeAssignment { Id = 3, TransactionTypeId = 1, DocumentTypeId = 3 }
);
        }
    }

}
