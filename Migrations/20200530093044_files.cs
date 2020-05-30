using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Factor.Migrations
{
    public partial class files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Factors");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Bytes = table.Column<byte[]>(nullable: true),
                    FactorItemId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Factors_FactorItemId",
                        column: x => x.FactorItemId,
                        principalTable: "Factors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Image_FactorItemId",
                table: "Image",
                column: "FactorItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Factors",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
