using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Factor.Migrations
{
    public partial class Commit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Verifications_Users_UserId",
                table: "Verifications");

            migrationBuilder.DropIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Verifications",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Verifications_Users_UserId",
                table: "Verifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Verifications_Users_UserId",
                table: "Verifications");

            migrationBuilder.DropIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Verifications",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Verifications_Users_UserId",
                table: "Verifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
