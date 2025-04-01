using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LandEmailAddress",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LandLordPhoneNumber",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantAgentName",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantBrokerage",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantBrokerageFax",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantName",
                table: "IRFDeals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "e9e784ab-307d-450d-b091-16b762055184", "AQAAAAIAAYagAAAAELgmPgwQr6re8M3LFpYO4tamQnyc9VOpHcDsb3VMGK0TypXF622fI6BJD4MOUvUppg==", new DateTime(2025, 3, 30, 1, 5, 32, 656, DateTimeKind.Local).AddTicks(7923), "cfcfdd12-3af7-4d04-a597-e130dff0cf38" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LandEmailAddress",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "LandLordPhoneNumber",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "TenantAgentName",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "TenantBrokerage",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "TenantBrokerageFax",
                table: "IRFDeals");

            migrationBuilder.DropColumn(
                name: "TenantName",
                table: "IRFDeals");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "54535f62-eeec-4780-9145-0840c97e698e", "AQAAAAIAAYagAAAAEKarZQbdL2baywShaDIiiATQS24lPQ7jpPAAWSxf1+ejzXHERnHr+eq7wwZYQrXRgw==", new DateTime(2025, 3, 5, 9, 22, 14, 634, DateTimeKind.Local).AddTicks(728), "d311dd10-d00f-47ce-9d21-550597299ab8" });
        }
    }
}
