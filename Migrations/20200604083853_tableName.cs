using Microsoft.EntityFrameworkCore.Migrations;

namespace Factor.Migrations
{
    public partial class tableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factors_SubmittedFactor_SubmittedFactorId",
                table: "Factors");

            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Users_UserId",
                table: "Factors");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Factors_PreFactorId",
                table: "Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Factors",
                table: "Factors");

            migrationBuilder.RenameTable(
                name: "Factors",
                newName: "PreFactors");

            migrationBuilder.RenameIndex(
                name: "IX_Factors_UserId",
                table: "PreFactors",
                newName: "IX_PreFactors_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Factors_SubmittedFactorId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "Factors");

            migrationBuilder.RenameIndex(
                name: "IX_PreFactors_UserId",
                table: "Factors",
                newName: "IX_Factors_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PreFactors_SubmittedFactorId",
                table: "Factors",
                newName: "IX_Factors_SubmittedFactorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Factors",
                table: "Factors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_SubmittedFactor_SubmittedFactorId",
                table: "Factors",
                column: "SubmittedFactorId",
                principalTable: "SubmittedFactor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_Users_UserId",
                table: "Factors",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Factors_PreFactorId",
                table: "Image",
                column: "PreFactorId",
                principalTable: "Factors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
