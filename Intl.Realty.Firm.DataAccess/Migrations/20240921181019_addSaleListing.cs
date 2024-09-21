using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addSaleListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "DocumentTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "IRFDeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PropertyAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinalSalePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    FinalClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DepositDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuyerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LandLordName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListingCommissionPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BuyingCommissionPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ListingAgentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListingBrokerage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListingBrokerageFax = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerAgentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerBrokerage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyerBrokerageFax = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellersLawyer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellersLawyerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellersPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyersLawyer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyersLawyerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyersPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IRFDeals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaseCoops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    IRFDealId = table.Column<int>(type: "int", nullable: false),
                    DealStatusId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseCoops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaseCoops_DealStatuses_DealStatusId",
                        column: x => x.DealStatusId,
                        principalTable: "DealStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaseCoops_IRFDeals_IRFDealId",
                        column: x => x.IRFDealId,
                        principalTable: "IRFDeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaseCoops_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaseListings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    IRFDealId = table.Column<int>(type: "int", nullable: false),
                    DealStatusId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaseListings_DealStatuses_DealStatusId",
                        column: x => x.DealStatusId,
                        principalTable: "DealStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaseListings_IRFDeals_IRFDealId",
                        column: x => x.IRFDealId,
                        principalTable: "IRFDeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaseListings_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleCoops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    IRFDealId = table.Column<int>(type: "int", nullable: false),
                    DealStatusId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleCoops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleCoops_DealStatuses_DealStatusId",
                        column: x => x.DealStatusId,
                        principalTable: "DealStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleCoops_IRFDeals_IRFDealId",
                        column: x => x.IRFDealId,
                        principalTable: "IRFDeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleCoops_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleListings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    IRFDealId = table.Column<int>(type: "int", nullable: false),
                    DealStatusId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleListings_DealStatuses_DealStatusId",
                        column: x => x.DealStatusId,
                        principalTable: "DealStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleListings_IRFDeals_IRFDealId",
                        column: x => x.IRFDealId,
                        principalTable: "IRFDeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleListings_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Directory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaleListingId = table.Column<int>(type: "int", nullable: true),
                    SaleCoopId = table.Column<int>(type: "int", nullable: true),
                    LeaseListingId = table.Column<int>(type: "int", nullable: true),
                    LeaseCoopId = table.Column<int>(type: "int", nullable: true),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUploads_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileUploads_LeaseCoops_LeaseCoopId",
                        column: x => x.LeaseCoopId,
                        principalTable: "LeaseCoops",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileUploads_LeaseListings_LeaseListingId",
                        column: x => x.LeaseListingId,
                        principalTable: "LeaseListings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileUploads_SaleCoops_SaleCoopId",
                        column: x => x.SaleCoopId,
                        principalTable: "SaleCoops",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileUploads_SaleListings_SaleListingId",
                        column: x => x.SaleListingId,
                        principalTable: "SaleListings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileUploads_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypeAssignments_DocumentTypeId",
                table: "DocumentTypeAssignments",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypeAssignments_TransactionTypeId",
                table: "DocumentTypeAssignments",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_DocumentTypeId",
                table: "FileUploads",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_LeaseCoopId",
                table: "FileUploads",
                column: "LeaseCoopId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_LeaseListingId",
                table: "FileUploads",
                column: "LeaseListingId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_SaleCoopId",
                table: "FileUploads",
                column: "SaleCoopId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_SaleListingId",
                table: "FileUploads",
                column: "SaleListingId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_TransactionTypeId",
                table: "FileUploads",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseCoops_DealStatusId",
                table: "LeaseCoops",
                column: "DealStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseCoops_IRFDealId",
                table: "LeaseCoops",
                column: "IRFDealId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseCoops_TransactionTypeId",
                table: "LeaseCoops",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseListings_DealStatusId",
                table: "LeaseListings",
                column: "DealStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseListings_IRFDealId",
                table: "LeaseListings",
                column: "IRFDealId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseListings_TransactionTypeId",
                table: "LeaseListings",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleCoops_DealStatusId",
                table: "SaleCoops",
                column: "DealStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleCoops_IRFDealId",
                table: "SaleCoops",
                column: "IRFDealId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleCoops_TransactionTypeId",
                table: "SaleCoops",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleListings_DealStatusId",
                table: "SaleListings",
                column: "DealStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleListings_IRFDealId",
                table: "SaleListings",
                column: "IRFDealId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleListings_TransactionTypeId",
                table: "SaleListings",
                column: "TransactionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTypeAssignments_DocumentTypes_DocumentTypeId",
                table: "DocumentTypeAssignments",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTypeAssignments_TransactionTypes_TransactionTypeId",
                table: "DocumentTypeAssignments",
                column: "TransactionTypeId",
                principalTable: "TransactionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTypeAssignments_DocumentTypes_DocumentTypeId",
                table: "DocumentTypeAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTypeAssignments_TransactionTypes_TransactionTypeId",
                table: "DocumentTypeAssignments");

            migrationBuilder.DropTable(
                name: "FileUploads");

            migrationBuilder.DropTable(
                name: "LeaseCoops");

            migrationBuilder.DropTable(
                name: "LeaseListings");

            migrationBuilder.DropTable(
                name: "SaleCoops");

            migrationBuilder.DropTable(
                name: "SaleListings");

            migrationBuilder.DropTable(
                name: "IRFDeals");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTypeAssignments_DocumentTypeId",
                table: "DocumentTypeAssignments");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTypeAssignments_TransactionTypeId",
                table: "DocumentTypeAssignments");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "DocumentTypes");
        }
    }
}
