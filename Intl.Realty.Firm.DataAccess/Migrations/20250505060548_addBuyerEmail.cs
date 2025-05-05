using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addBuyerEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellerEmailPhone",
                table: "IRFDeals",
                newName: "SellerEmail");

            migrationBuilder.RenameColumn(
                name: "BuyerEmailPhone",
                table: "IRFDeals",
                newName: "BuyerEmail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "fa1419e2-c0d2-4ddc-b565-00f959c9eebc", "AQAAAAIAAYagAAAAEEtRTeAc9xShdIL7wi8SWeupJ90gnFlQSI8tR6XJDd+Hl9NB98TMuwfiFKKtf4aMwg==", new DateTime(2025, 5, 5, 14, 5, 45, 783, DateTimeKind.Local).AddTicks(9680), "91ddbf6f-742d-48c7-8b25-4fc45ad7632a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellerEmail",
                table: "IRFDeals",
                newName: "SellerEmailPhone");

            migrationBuilder.RenameColumn(
                name: "BuyerEmail",
                table: "IRFDeals",
                newName: "BuyerEmailPhone");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "70b0ee93-0b2d-40ba-818d-ec27a95836b0", "AQAAAAIAAYagAAAAEH1pXQSP0TMLrIeG6u5yhJLZY8B6GscV4PEnNwfb/fBrpZFLsq5aNAYdCtSQpYUsyw==", new DateTime(2025, 5, 3, 8, 5, 43, 249, DateTimeKind.Local).AddTicks(9713), "bab8205a-7af9-4866-a179-9e0d05c4d0ec" });
        }
    }
}
