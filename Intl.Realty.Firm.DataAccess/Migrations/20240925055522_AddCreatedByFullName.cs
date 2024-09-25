using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByFullName",
                table: "SaleListings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletedByFullName",
                table: "SaleListings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByFullName",
                table: "SaleListings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByFullName",
                table: "SaleCoops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletedByFullName",
                table: "SaleCoops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByFullName",
                table: "SaleCoops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByFullName",
                table: "LeaseListings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletedByFullName",
                table: "LeaseListings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByFullName",
                table: "LeaseListings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByFullName",
                table: "LeaseCoops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletedByFullName",
                table: "LeaseCoops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByFullName",
                table: "LeaseCoops",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByFullName",
                table: "SaleListings");

            migrationBuilder.DropColumn(
                name: "DeletedByFullName",
                table: "SaleListings");

            migrationBuilder.DropColumn(
                name: "UpdatedByFullName",
                table: "SaleListings");

            migrationBuilder.DropColumn(
                name: "CreatedByFullName",
                table: "SaleCoops");

            migrationBuilder.DropColumn(
                name: "DeletedByFullName",
                table: "SaleCoops");

            migrationBuilder.DropColumn(
                name: "UpdatedByFullName",
                table: "SaleCoops");

            migrationBuilder.DropColumn(
                name: "CreatedByFullName",
                table: "LeaseListings");

            migrationBuilder.DropColumn(
                name: "DeletedByFullName",
                table: "LeaseListings");

            migrationBuilder.DropColumn(
                name: "UpdatedByFullName",
                table: "LeaseListings");

            migrationBuilder.DropColumn(
                name: "CreatedByFullName",
                table: "LeaseCoops");

            migrationBuilder.DropColumn(
                name: "DeletedByFullName",
                table: "LeaseCoops");

            migrationBuilder.DropColumn(
                name: "UpdatedByFullName",
                table: "LeaseCoops");
        }
    }
}
