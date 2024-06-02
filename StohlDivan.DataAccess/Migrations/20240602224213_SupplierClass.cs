using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StohlDivan.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SupplierClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carrier",
                table: "OrderHeaders");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "OrderHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_SupplierId",
                table: "OrderHeaders",
                column: "SupplierId",
                unique: true,
                filter: "[SupplierId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Supplier_SupplierId",
                table: "OrderHeaders",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Supplier_SupplierId",
                table: "OrderHeaders");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_SupplierId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "OrderHeaders");

            migrationBuilder.AddColumn<string>(
                name: "Carrier",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
