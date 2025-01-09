using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "9a74d96b-d258-4a70-8fb2-c72849c57129", "AQAAAAIAAYagAAAAEC0vyluj8TgqoNcSNrO0H9827ETgfWy4ezPsoQqYZsCfkDaTfuCtSC4CaJ677Iwlcg==", new DateTime(2025, 1, 9, 20, 40, 38, 450, DateTimeKind.Local).AddTicks(7467), "88ad9a25-33d1-471f-8f25-b8ff67c61555" });

            migrationBuilder.UpdateData(
                table: "DealTransactionNumberRanges",
                keyColumn: "Id",
                keyValue: 1,
                column: "Year",
                value: 2025);

            migrationBuilder.UpdateData(
                table: "DealTransactionNumberRanges",
                keyColumn: "Id",
                keyValue: 2,
                column: "Year",
                value: 2026);

            migrationBuilder.UpdateData(
                table: "DealTransactionNumberRanges",
                keyColumn: "Id",
                keyValue: 3,
                column: "Year",
                value: 2027);

            migrationBuilder.UpdateData(
                table: "DealTransactionNumberRanges",
                keyColumn: "Id",
                keyValue: 4,
                column: "Year",
                value: 2028);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "4b952d11-bec3-49b1-8f53-6f67658cb877", "AQAAAAIAAYagAAAAEKdQIge6HeKlQMUiRLvoYl3uYFPyW/tHxkMlB+/djp+VDwehc/M8aC7JBEhJ8F5kjw==", new DateTime(2024, 10, 8, 20, 58, 11, 475, DateTimeKind.Local).AddTicks(3864), "91014eab-5b96-4067-af9d-11d79a62d9e9" });

            migrationBuilder.UpdateData(
                table: "DealTransactionNumberRanges",
                keyColumn: "Id",
                keyValue: 1,
                column: "Year",
                value: 2024);

            migrationBuilder.UpdateData(
                table: "DealTransactionNumberRanges",
                keyColumn: "Id",
                keyValue: 2,
                column: "Year",
                value: 2025);

            migrationBuilder.UpdateData(
                table: "DealTransactionNumberRanges",
                keyColumn: "Id",
                keyValue: 3,
                column: "Year",
                value: 2026);

            migrationBuilder.UpdateData(
                table: "DealTransactionNumberRanges",
                keyColumn: "Id",
                keyValue: 4,
                column: "Year",
                value: 2027);
        }
    }
}
