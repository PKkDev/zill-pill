using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZillPillService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicinalProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Characteristics = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinalProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicinalProductCertificate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    License = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Approved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicinalProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinalProductCertificate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinalProductCertificate_MedicinalProduct_MedicinalProductId",
                        column: x => x.MedicinalProductId,
                        principalTable: "MedicinalProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicinalProductChemical",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicinalProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinalProductChemical", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinalProductChemical_MedicinalProduct_MedicinalProductId",
                        column: x => x.MedicinalProductId,
                        principalTable: "MedicinalProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicinalProductImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    MedicinalProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinalProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinalProductImage_MedicinalProduct_MedicinalProductId",
                        column: x => x.MedicinalProductId,
                        principalTable: "MedicinalProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicinalProductRelease",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicinalProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinalProductRelease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinalProductRelease_MedicinalProduct_MedicinalProductId",
                        column: x => x.MedicinalProductId,
                        principalTable: "MedicinalProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicinalProductUser",
                columns: table => new
                {
                    MedicinalProductsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinalProductUser", x => new { x.MedicinalProductsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_MedicinalProductUser_MedicinalProduct_MedicinalProductsId",
                        column: x => x.MedicinalProductsId,
                        principalTable: "MedicinalProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicinalProductUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMedicinalProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MedicinalProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMedicinalProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMedicinalProduct_MedicinalProduct_MedicinalProductId",
                        column: x => x.MedicinalProductId,
                        principalTable: "MedicinalProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMedicinalProduct_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfile_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationSheduller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    UserMedicinalProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationSheduller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationSheduller_UserMedicinalProduct_UserMedicinalProductId",
                        column: x => x.UserMedicinalProductId,
                        principalTable: "UserMedicinalProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicationSheduller_UserMedicinalProductId",
                table: "MedicationSheduller",
                column: "UserMedicinalProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinalProductCertificate_MedicinalProductId",
                table: "MedicinalProductCertificate",
                column: "MedicinalProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicinalProductChemical_MedicinalProductId",
                table: "MedicinalProductChemical",
                column: "MedicinalProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinalProductImage_MedicinalProductId",
                table: "MedicinalProductImage",
                column: "MedicinalProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicinalProductRelease_MedicinalProductId",
                table: "MedicinalProductRelease",
                column: "MedicinalProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinalProductUser_UsersId",
                table: "MedicinalProductUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "Phone_Index",
                table: "User",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMedicinalProduct_MedicinalProductId",
                table: "UserMedicinalProduct",
                column: "MedicinalProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedicinalProduct_UserId",
                table: "UserMedicinalProduct",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "Email_Index",
                table: "UserProfile",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_UserId",
                table: "UserProfile",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicationSheduller");

            migrationBuilder.DropTable(
                name: "MedicinalProductCertificate");

            migrationBuilder.DropTable(
                name: "MedicinalProductChemical");

            migrationBuilder.DropTable(
                name: "MedicinalProductImage");

            migrationBuilder.DropTable(
                name: "MedicinalProductRelease");

            migrationBuilder.DropTable(
                name: "MedicinalProductUser");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "UserMedicinalProduct");

            migrationBuilder.DropTable(
                name: "MedicinalProduct");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
