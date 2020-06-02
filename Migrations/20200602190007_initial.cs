using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Factor.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubmittedFactor",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TotalPrice = table.Column<long>(nullable: false),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmittedFactor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SubmitedFactorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contact_SubmittedFactor_SubmitedFactorId",
                        column: x => x.SubmitedFactorId,
                        principalTable: "SubmittedFactor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactorItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    TotalPrice = table.Column<long>(nullable: false),
                    SubmittedFactorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactorItem_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FactorItem_SubmittedFactor_SubmittedFactorId",
                        column: x => x.SubmittedFactorId,
                        principalTable: "SubmittedFactor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Factors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    UploadTime = table.Column<DateTime>(nullable: false),
                    IsDone = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SubmittedFactorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factors_SubmittedFactor_SubmittedFactorId",
                        column: x => x.SubmittedFactorId,
                        principalTable: "SubmittedFactor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Verifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<long>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Verifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Bytes = table.Column<byte[]>(nullable: true),
                    PreFactorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Factors_PreFactorId",
                        column: x => x.PreFactorId,
                        principalTable: "Factors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_SubmitedFactorId",
                table: "Contact",
                column: "SubmitedFactorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FactorItem_ProductId",
                table: "FactorItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorItem_SubmittedFactorId",
                table: "FactorItem",
                column: "SubmittedFactorId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_SubmittedFactorId",
                table: "Factors",
                column: "SubmittedFactorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factors_UserId",
                table: "Factors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_PreFactorId",
                table: "Image",
                column: "PreFactorId");

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "FactorItem");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Verifications");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "SubmittedFactor");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
