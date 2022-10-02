using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZillPillService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "MedicationSheduller");

            migrationBuilder.AddColumn<int>(
                name: "ShedullerType",
                table: "UserMedicinalProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "MedicationSheduller",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "MedicationSheduller",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShedullerType",
                table: "UserMedicinalProduct");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "MedicationSheduller");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "MedicationSheduller");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "MedicationSheduller",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
