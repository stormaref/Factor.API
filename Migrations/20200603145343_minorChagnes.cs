using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Factor.Migrations
{
    public partial class minorChagnes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                table: "SubmittedFactor",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsClear = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedFactor_StateId",
                table: "SubmittedFactor",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedFactor_State_StateId",
                table: "SubmittedFactor",
                column: "StateId",
                principalTable: "State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedFactor_State_StateId",
                table: "SubmittedFactor");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedFactor_StateId",
                table: "SubmittedFactor");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "SubmittedFactor");
        }
    }
}
