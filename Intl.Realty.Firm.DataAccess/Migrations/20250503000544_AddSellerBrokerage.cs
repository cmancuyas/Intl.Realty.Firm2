using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerBrokerage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SellerBrokerage",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "70b0ee93-0b2d-40ba-818d-ec27a95836b0", "AQAAAAIAAYagAAAAEH1pXQSP0TMLrIeG6u5yhJLZY8B6GscV4PEnNwfb/fBrpZFLsq5aNAYdCtSQpYUsyw==", new DateTime(2025, 5, 3, 8, 5, 43, 249, DateTimeKind.Local).AddTicks(9713), "bab8205a-7af9-4866-a179-9e0d05c4d0ec" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerBrokerage",
                table: "IRFDeals");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "7d37611a-33f9-45cb-981d-7ec38bc07236", "AQAAAAIAAYagAAAAELBSkuXjlPDupDmidw7hrD/UnEFvvHaaAReX6/GjTDot1e2goUnx/zDQo9nAbHMGMA==", new DateTime(2025, 5, 3, 6, 15, 56, 2, DateTimeKind.Local).AddTicks(4756), "d3638f76-b4b2-4834-96df-92aa15cbcb52" });
        }
    }
}
