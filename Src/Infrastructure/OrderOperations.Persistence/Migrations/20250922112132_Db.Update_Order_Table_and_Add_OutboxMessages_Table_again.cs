using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderOperations.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdate_Order_Table_and_Add_OutboxMessages_Table_again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_PersonId_IdempotencyKey",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PersonId_IdempotencyKey",
                table: "Orders",
                columns: new[] { "PersonId", "IdempotencyKey" },
                unique: true,
                filter: "\"IdempotencyKey\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_PersonId_IdempotencyKey",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PersonId_IdempotencyKey",
                table: "Orders",
                columns: new[] { "PersonId", "IdempotencyKey" },
                unique: true,
                filter: "[IdempotencyKey] IS NOT NULL");
        }
    }
}
