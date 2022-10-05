using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZillPillService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryDictionaryId",
                table: "MedicinalProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnionUtcDate",
                table: "MedicationSheduller",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "CountryDictionary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryDictionary", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicinalProduct_CountryDictionaryId",
                table: "MedicinalProduct",
                column: "CountryDictionaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicinalProduct_CountryDictionary_CountryDictionaryId",
                table: "MedicinalProduct",
                column: "CountryDictionaryId",
                principalTable: "CountryDictionary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicinalProduct_CountryDictionary_CountryDictionaryId",
                table: "MedicinalProduct");

            migrationBuilder.DropTable(
                name: "CountryDictionary");

            migrationBuilder.DropIndex(
                name: "IX_MedicinalProduct_CountryDictionaryId",
                table: "MedicinalProduct");

            migrationBuilder.DropColumn(
                name: "CountryDictionaryId",
                table: "MedicinalProduct");

            migrationBuilder.DropColumn(
                name: "UnionUtcDate",
                table: "MedicationSheduller");
        }
    }
}
