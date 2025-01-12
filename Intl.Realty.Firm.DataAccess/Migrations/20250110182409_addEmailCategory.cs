using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addEmailCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_EmailCategories", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "8bb5173a-87ea-4e5f-8465-444d1efe4f37", "AQAAAAIAAYagAAAAEJmBm8Fp9K/SHY/qYXHieTJsx3UccCutZuFlZRN5+gJ5KUKuJlMB95BZcT8hduV74g==", new DateTime(2025, 1, 11, 2, 24, 7, 104, DateTimeKind.Local).AddTicks(9126), "65a0db44-a861-48f2-96e2-2a5e089f1461" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailCategories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "9a74d96b-d258-4a70-8fb2-c72849c57129", "AQAAAAIAAYagAAAAEC0vyluj8TgqoNcSNrO0H9827ETgfWy4ezPsoQqYZsCfkDaTfuCtSC4CaJ677Iwlcg==", new DateTime(2025, 1, 9, 20, 40, 38, 450, DateTimeKind.Local).AddTicks(7467), "88ad9a25-33d1-471f-8f25-b8ff67c61555" });
        }
    }
}
