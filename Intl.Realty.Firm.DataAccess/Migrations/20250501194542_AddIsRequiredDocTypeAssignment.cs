using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsRequiredDocTypeAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "DocumentTypeAssignments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "5e16c607-b60e-49c5-b891-e673ef7d2862", "AQAAAAIAAYagAAAAEPUbszbTp/8t89kcO0SLrGo8qRt9Z0UOsSklbHwwy+/gWY0T/kbud9gmnJ1GSF8LkQ==", new DateTime(2025, 5, 2, 3, 45, 39, 689, DateTimeKind.Local).AddTicks(6786), "8f22c68e-46f0-4eed-af62-db771f2b6778" });

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 11,
                column: "IsRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "DocumentTypeAssignments",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsRequired",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "DocumentTypeAssignments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "bd36cffe-6a74-4add-922d-c0ba713bfeef", "AQAAAAIAAYagAAAAEDvzYyeFYAlwq3YyYPOGQkP5hL52hOldB4OIcEeAs4Qa7wsnlhQdorOO+61sVtD+NA==", new DateTime(2025, 4, 20, 3, 30, 11, 271, DateTimeKind.Local).AddTicks(684), "ebf7dab4-1583-48fe-9acf-cff24293ea89" });
        }
    }
}
