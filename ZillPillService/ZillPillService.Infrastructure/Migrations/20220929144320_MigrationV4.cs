using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZillPillService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "MedicationSheduller",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSended",
                table: "MedicationSheduller",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "MedicationSheduller");

            migrationBuilder.DropColumn(
                name: "IsSended",
                table: "MedicationSheduller");
        }
    }
}
