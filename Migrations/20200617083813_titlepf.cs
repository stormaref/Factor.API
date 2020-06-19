using Microsoft.EntityFrameworkCore.Migrations;

namespace Factor.Migrations
{
    public partial class titlepf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PreFactor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "PreFactor");
        }
    }
}
