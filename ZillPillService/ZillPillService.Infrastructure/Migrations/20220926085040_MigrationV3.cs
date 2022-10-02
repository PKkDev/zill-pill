using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZillPillService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnd",
                table: "UserMedicinalProduct",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStart",
                table: "UserMedicinalProduct",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateEnd",
                table: "UserMedicinalProduct");

            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "UserMedicinalProduct");
        }
    }
}
