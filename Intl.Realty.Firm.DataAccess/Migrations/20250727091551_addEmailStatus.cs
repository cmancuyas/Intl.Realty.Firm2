using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addEmailStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
                name: "EmailStatusId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmailStatuses",
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
                    table.PrimaryKey("PK_EmailStatuses", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "EmailStatusId", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "9b136909-faf0-4b69-b00b-72a68fc2fa61", 0, "AQAAAAIAAYagAAAAELoqS2fbtZszsAVuRywOqwl4AG1PPZb15G2xXwKaqnfB787VLLyAMPH+llH5vc5IRQ==", new DateTime(2025, 7, 27, 17, 15, 49, 24, DateTimeKind.Local).AddTicks(9252), "85f9f089-dfa0-4f38-8686-6c436d74aa53" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmailStatusId",
                table: "AspNetUsers",
                column: "EmailStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EmailStatuses_EmailStatusId",
                table: "AspNetUsers",
                column: "EmailStatusId",
                principalTable: "EmailStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EmailStatuses_EmailStatusId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EmailStatuses");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmailStatusId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailStatusId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "fa1419e2-c0d2-4ddc-b565-00f959c9eebc", "AQAAAAIAAYagAAAAEEtRTeAc9xShdIL7wi8SWeupJ90gnFlQSI8tR6XJDd+Hl9NB98TMuwfiFKKtf4aMwg==", new DateTime(2025, 5, 5, 14, 5, 45, 783, DateTimeKind.Local).AddTicks(9680), "91ddbf6f-742d-48c7-8b25-4fc45ad7632a" });
        }
    }
}
