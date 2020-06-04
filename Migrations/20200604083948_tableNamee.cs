using Microsoft.EntityFrameworkCore.Migrations;

namespace Factor.Migrations
{
    public partial class tableNamee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_PreFactors_PreFactorId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_PreFactors_SubmittedFactor_SubmittedFactorId",
                table: "PreFactors");

            migrationBuilder.DropForeignKey(
                name: "FK_PreFactors_Users_UserId",
                table: "PreFactors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreFactors",
                table: "PreFactors");

            migrationBuilder.RenameTable(
                name: "PreFactors",
                newName: "PreFactor");

            migrationBuilder.RenameIndex(
                name: "IX_PreFactors_UserId",
                table: "PreFactor",
                newName: "IX_PreFactor_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PreFactors_SubmittedFactorId",
                table: "PreFactor",
                newName: "IX_PreFactor_SubmittedFactorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreFactor",
                table: "PreFactor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_PreFactor_PreFactorId",
                table: "Image",
                column: "PreFactorId",
                principalTable: "PreFactor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreFactor_SubmittedFactor_SubmittedFactorId",
                table: "PreFactor",
                column: "SubmittedFactorId",
                principalTable: "SubmittedFactor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreFactor_Users_UserId",
                table: "PreFactor",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_PreFactor_PreFactorId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_PreFactor_SubmittedFactor_SubmittedFactorId",
                table: "PreFactor");

            migrationBuilder.DropForeignKey(
                name: "FK_PreFactor_Users_UserId",
                table: "PreFactor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreFactor",
                table: "PreFactor");

            migrationBuilder.RenameTable(
                name: "PreFactor",
                newName: "PreFactors");

            migrationBuilder.RenameIndex(
                name: "IX_PreFactor_UserId",
                table: "PreFactors",
                newName: "IX_PreFactors_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PreFactor_SubmittedFactorId",
                table: "PreFactors",
                newName: "IX_PreFactors_SubmittedFactorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreFactors",
                table: "PreFactors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_PreFactors_PreFactorId",
                table: "Image",
                column: "PreFactorId",
                principalTable: "PreFactors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreFactors_SubmittedFactor_SubmittedFactorId",
                table: "PreFactors",
                column: "SubmittedFactorId",
                principalTable: "SubmittedFactor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreFactors_Users_UserId",
                table: "PreFactors",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
