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

        public DbSet<Department> Departments { get; set; }
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
    }

}
