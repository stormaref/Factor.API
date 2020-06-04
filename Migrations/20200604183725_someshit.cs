using Microsoft.EntityFrameworkCore.Migrations;

namespace Factor.Migrations
{
    public partial class someshit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FactorItem_ProductId",
                table: "FactorItem");

            migrationBuilder.CreateIndex(
                name: "IX_FactorItem_ProductId",
                table: "FactorItem",
                column: "ProductId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FactorItem_ProductId",
                table: "FactorItem");

            migrationBuilder.CreateIndex(
                name: "IX_FactorItem_ProductId",
                table: "FactorItem",
                column: "ProductId");
        }
    }
}
