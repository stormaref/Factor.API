using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Factor.Migrations
{
    public partial class wtf2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreFactor_SubmittedFactor_SubmittedFactorId",
                table: "PreFactor");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedFactor_Contact_ContactId",
                table: "SubmittedFactor");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedFactor_ContactId",
                table: "SubmittedFactor");

            migrationBuilder.DropIndex(
                name: "IX_PreFactor_SubmittedFactorId",
                table: "PreFactor");

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                table: "SubmittedFactor",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubmittedFactorId",
                table: "PreFactor",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedFactor_ContactId",
                table: "SubmittedFactor",
                column: "ContactId",
                unique: true,
                filter: "[ContactId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PreFactor_SubmittedFactorId",
                table: "PreFactor",
                column: "SubmittedFactorId",
                unique: true,
                filter: "[SubmittedFactorId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PreFactor_SubmittedFactor_SubmittedFactorId",
                table: "PreFactor",
                column: "SubmittedFactorId",
                principalTable: "SubmittedFactor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedFactor_Contact_ContactId",
                table: "SubmittedFactor",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreFactor_SubmittedFactor_SubmittedFactorId",
                table: "PreFactor");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedFactor_Contact_ContactId",
                table: "SubmittedFactor");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedFactor_ContactId",
                table: "SubmittedFactor");

            migrationBuilder.DropIndex(
                name: "IX_PreFactor_SubmittedFactorId",
                table: "PreFactor");

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                table: "SubmittedFactor",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SubmittedFactorId",
                table: "PreFactor",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedFactor_ContactId",
                table: "SubmittedFactor",
                column: "ContactId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreFactor_SubmittedFactorId",
                table: "PreFactor",
                column: "SubmittedFactorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PreFactor_SubmittedFactor_SubmittedFactorId",
                table: "PreFactor",
                column: "SubmittedFactorId",
                principalTable: "SubmittedFactor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedFactor_Contact_ContactId",
                table: "SubmittedFactor",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
