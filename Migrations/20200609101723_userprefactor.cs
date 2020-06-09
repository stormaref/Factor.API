using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Factor.Migrations
{
    public partial class userprefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreFactor_Users_UserId",
                table: "PreFactor");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "PreFactor",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PreFactor_Users_UserId",
                table: "PreFactor",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreFactor_Users_UserId",
                table: "PreFactor");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "PreFactor",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_PreFactor_Users_UserId",
                table: "PreFactor",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
