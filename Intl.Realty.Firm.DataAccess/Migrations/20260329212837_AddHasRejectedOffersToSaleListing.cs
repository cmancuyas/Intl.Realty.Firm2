using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    public partial class AddHasRejectedOffersToSaleListing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasRejectedOffers",
                table: "SaleListings",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasRejectedOffers",
                table: "SaleListings");
        }
    }
}