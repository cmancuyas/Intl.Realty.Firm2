using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAgentReferral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProfileImages_ProfileImageId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileImageId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImageId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AgentReferrals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferrerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReferrerPhone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReferredAgentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_AgentReferrals", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "bd36cffe-6a74-4add-922d-c0ba713bfeef", "AQAAAAIAAYagAAAAEDvzYyeFYAlwq3YyYPOGQkP5hL52hOldB4OIcEeAs4Qa7wsnlhQdorOO+61sVtD+NA==", new DateTime(2025, 4, 20, 3, 30, 11, 271, DateTimeKind.Local).AddTicks(684), "ebf7dab4-1583-48fe-9acf-cff24293ea89" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentReferrals");

            migrationBuilder.AddColumn<int>(
                name: "ProfileImageId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfilePictureId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("74b7ddfa-9c71-4b95-84e2-9b5fcdef884c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfileImageId1", "ProfilePictureId", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "863ab1aa-7549-49ee-9b5b-cf546c4c5c13", "AQAAAAIAAYagAAAAEK0QTCe7E/V5UswcdWgUq/20KLlUnED2PgSJ2z9FZ1w2Z/j10EbsT9HSYT+vhdfD/Q==", null, null, new DateTime(2025, 4, 18, 20, 26, 20, 656, DateTimeKind.Local).AddTicks(3915), "931ceb30-dbe3-47e2-b35d-b002b0e49559" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileImageId1",
                table: "AspNetUsers",
                column: "ProfileImageId1",
                unique: true,
                filter: "[ProfileImageId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProfileImages_ProfileImageId1",
                table: "AspNetUsers",
                column: "ProfileImageId1",
                principalTable: "ProfileImages",
                principalColumn: "Id");
        }
    }
}
