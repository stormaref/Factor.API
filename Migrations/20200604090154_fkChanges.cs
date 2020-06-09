using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Factor.Migrations
{
    public partial class fkChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_SubmittedFactor_SubmitedFactorId",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_SubmitedFactorId",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "SubmitedFactorId",
                table: "Contact");

            migrationBuilder.AddColumn<Guid>(
                name: "ContactId",
                table: "SubmittedFactor",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedFactor_ContactId",
                table: "SubmittedFactor",
                column: "ContactId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedFactor_Contact_ContactId",
                table: "SubmittedFactor",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedFactor_Contact_ContactId",
                table: "SubmittedFactor");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedFactor_ContactId",
                table: "SubmittedFactor");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "SubmittedFactor");

            migrationBuilder.AddColumn<Guid>(
                name: "SubmitedFactorId",
                table: "Contact",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Contact_SubmitedFactorId",
                table: "Contact",
                column: "SubmitedFactorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_SubmittedFactor_SubmitedFactorId",
                table: "Contact",
                column: "SubmitedFactorId",
                principalTable: "SubmittedFactor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
