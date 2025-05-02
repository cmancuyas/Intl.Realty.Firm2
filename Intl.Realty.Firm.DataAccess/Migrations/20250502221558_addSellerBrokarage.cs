using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addSellerBrokarage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyersPhoneNumber",
                table: "IRFDeals");

            migrationBuilder.RenameColumn(
                name: "SellersPhoneNumber",
                table: "IRFDeals",
                newName: "SellerName");

            migrationBuilder.RenameColumn(
                name: "SellersLawyerAddress",
                table: "IRFDeals",
                newName: "SellerLawyerAddress");

            migrationBuilder.RenameColumn(
                name: "SellersLawyer",
                table: "IRFDeals",
                newName: "SellerLawyer");

            migrationBuilder.RenameColumn(
                name: "BuyersLawyerAddress",
                table: "IRFDeals",
                newName: "SellerEmailPhone");

            migrationBuilder.RenameColumn(
                name: "BuyersLawyer",
                table: "IRFDeals",
                newName: "SellerBrokeragePhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "BuyerBrokerageEmail",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuyerBrokeragePhoneNumber",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuyerEmailPhone",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuyerLawyer",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuyerLawyerAddress",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellerAgentName",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellerBrokerageEmail",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellerBrokerageFax",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "7d37611a-33f9-45cb-981d-7ec38bc07236", "AQAAAAIAAYagAAAAELBSkuXjlPDupDmidw7hrD/UnEFvvHaaAReX6/GjTDot1e2goUnx/zDQo9nAbHMGMA==", new DateTime(2025, 5, 3, 6, 15, 56, 2, DateTimeKind.Local).AddTicks(4756), "d3638f76-b4b2-4834-96df-92aa15cbcb52" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerBrokerageEmail",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "BuyerBrokeragePhoneNumber",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "BuyerEmailPhone",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "BuyerLawyer",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "BuyerLawyerAddress",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "SellerAgentName",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "SellerBrokerageEmail",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "SellerBrokerageFax",
                table: "IRFDeals");

            migrationBuilder.RenameColumn(
                name: "SellerName",
                table: "IRFDeals",
                newName: "SellersPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "SellerLawyerAddress",
                table: "IRFDeals",
                newName: "SellersLawyerAddress");

            migrationBuilder.RenameColumn(
                name: "SellerLawyer",
                table: "IRFDeals",
                newName: "SellersLawyer");

            migrationBuilder.RenameColumn(
                name: "SellerEmailPhone",
                table: "IRFDeals",
                newName: "BuyersLawyerAddress");

            migrationBuilder.RenameColumn(
                name: "SellerBrokeragePhoneNumber",
                table: "IRFDeals",
                newName: "BuyersLawyer");

            migrationBuilder.AddColumn<string>(
                name: "BuyersPhoneNumber",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "5e16c607-b60e-49c5-b891-e673ef7d2862", "AQAAAAIAAYagAAAAEPUbszbTp/8t89kcO0SLrGo8qRt9Z0UOsSklbHwwy+/gWY0T/kbud9gmnJ1GSF8LkQ==", new DateTime(2025, 5, 2, 3, 45, 39, 689, DateTimeKind.Local).AddTicks(6786), "8f22c68e-46f0-4eed-af62-db771f2b6778" });
        }
    }
}
