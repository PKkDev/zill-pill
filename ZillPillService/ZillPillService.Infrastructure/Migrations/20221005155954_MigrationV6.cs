using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZillPillService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeUtcOffset",
                table: "MedicationSheduller",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeUtcOffset",
                table: "MedicationSheduller");
        }
    }
}
