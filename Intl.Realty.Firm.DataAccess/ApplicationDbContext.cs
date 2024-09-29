using Intl.Realty.Firm.DataAccess.Config;
using Intl.Realty.Firm.Model.Model;
using Microsoft.AspNetCore.Identity;
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

            modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Code = "1", Description = "Information Technology" },
            new Department { Id = 2, Code = "2", Description = "Human Resources" },
            new Department { Id = 3, Code = "3", Description = "Sales" }
                );

            modelBuilder.Entity<EmploymentStatus>().HasData(
            new EmploymentStatus { Id = 1, Code = "1", Description = "Active" },
            new EmploymentStatus { Id = 2, Code = "2", Description = "Inactive" },
            new EmploymentStatus { Id = 3, Code = "3", Description = "Blacklisted" }
                );

            modelBuilder.Entity<DealStatus>().HasData(
            new DealStatus { Id = 1, Code = "1", Description = "Submitted" },
            new DealStatus { Id = 2, Code = "2", Description = "Saved" }
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

            modelBuilder.Entity<DocumentTypeAssignment>().HasData(
            new DocumentTypeAssignment { Id = 1, TransactionTypeId = 2, DocumentTypeId = 1 },
            new DocumentTypeAssignment { Id = 2, TransactionTypeId = 2, DocumentTypeId = 2 },
            new DocumentTypeAssignment { Id = 3, TransactionTypeId = 2, DocumentTypeId = 3 }
            );

            modelBuilder.Entity<DocumentTypeAssignment>().HasData(
                new DocumentTypeAssignment { Id = 1, TransactionTypeId = 3, DocumentTypeId = 1 },
                new DocumentTypeAssignment { Id = 2, TransactionTypeId = 3, DocumentTypeId = 2 },
                new DocumentTypeAssignment { Id = 3, TransactionTypeId = 3, DocumentTypeId = 3 }
                );

            modelBuilder.Entity<DocumentTypeAssignment>().HasData(
                new DocumentTypeAssignment { Id = 1, TransactionTypeId = 4, DocumentTypeId = 1 },
                new DocumentTypeAssignment { Id = 2, TransactionTypeId = 4, DocumentTypeId = 2 },
                new DocumentTypeAssignment { Id = 3, TransactionTypeId = 4, DocumentTypeId = 3 }
                );

            List<ApplicationRole> roles = new List<ApplicationRole>
            {
                new ApplicationRole
                {
                    Id = new Guid("6ca041d2-d159-4077-ae85-44a8e33b0d44"),
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                },
                new ApplicationRole
                {
                    Id = new Guid("529e4622-26e2-4403-88a0-554d6e197557"),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new ApplicationRole
                {
                    Id = new Guid("a48ea010-1a28-4bf4-b13f-3b210714d39a"),
                    Name = "DealAdmin",
                    NormalizedName = "DEALADMIN"
                },
                new ApplicationRole
                {
                    Id = new Guid("4049c51e-53d5-482d-bd3d-1edebf961e0b"),
                    Name = "Agent",
                    NormalizedName = "AGENT"
                }
            };
            modelBuilder.Entity<ApplicationRole>().HasData(roles);

            var hasher = new PasswordHasher<ApplicationUser>();
            List<ApplicationUser> users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id=new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                    UserName = "superadmin@internationalrealtyfirm.com",
                    Email = "superadmin@internationalrealtyfirm.com",
                    NormalizedUserName = "SUPERADMIN@INTERNATIONALREALTYFIRM.COM",
                    NormalizedEmail = "SUPERADMIN@INTERNATIONALREALTYFIRM.COM",
                    PasswordHash = hasher.HashPassword(null, "aMyhurLX9Ad94D!"),
                    EmailConfirmed = true,
                    EmploymentStatusId = 1,
                    FirstName = "Christopher",
                    LastName = "Mancuyas",
                    RegistrationDate = DateTime.Now,
                    SecurityStamp = Guid.NewGuid().ToString()

                },
            };
            modelBuilder.Entity<ApplicationUser>().HasData(users);

            List<IdentityUserRole<Guid>> userRoles = new List<IdentityUserRole<Guid>>
            {
                new IdentityUserRole<Guid>
                {
                    RoleId = roles.First(r => r.Name == "SuperAdmin").Id,
                    UserId = users.First(u => u.UserName == "superadmin@internationalrealtyfirm.com").Id
                },
            };
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(userRoles);
        }
    }

}
