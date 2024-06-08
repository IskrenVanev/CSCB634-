using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StohlDivan.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SupplierClassFixProblemWithRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_SupplierId",
                table: "OrderHeaders");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_SupplierId",
                table: "OrderHeaders",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_SupplierId",
                table: "OrderHeaders");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_SupplierId",
                table: "OrderHeaders",
                column: "SupplierId",
                unique: true,
                filter: "[SupplierId] IS NOT NULL");
        }
    }
}
