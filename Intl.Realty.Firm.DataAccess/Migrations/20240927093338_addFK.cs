using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionTypes_IRFDeals_IRFDealId",
                table: "TransactionTypes");

            migrationBuilder.DropIndex(
                name: "IX_TransactionTypes_IRFDealId",
                table: "TransactionTypes");

            migrationBuilder.DropColumn(
                name: "IRFDealId",
                table: "TransactionTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IRFDealId",
                table: "TransactionTypes",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "IRFDealId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "IRFDealId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "IRFDealId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TransactionTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "IRFDealId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypes_IRFDealId",
                table: "TransactionTypes",
                column: "IRFDealId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionTypes_IRFDeals_IRFDealId",
                table: "TransactionTypes",
                column: "IRFDealId",
                principalTable: "IRFDeals",
                principalColumn: "Id");
        }
    }
}
