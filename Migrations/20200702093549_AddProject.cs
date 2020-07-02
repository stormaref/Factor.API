using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Factor.Migrations
{
    public partial class AddProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "PreFactor",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreFactor_ProjectId",
                table: "PreFactor",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreFactor_Project_ProjectId",
                table: "PreFactor",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreFactor_Project_ProjectId",
                table: "PreFactor");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropIndex(
                name: "IX_PreFactor_ProjectId",
                table: "PreFactor");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "PreFactor");
        }
    }
}
