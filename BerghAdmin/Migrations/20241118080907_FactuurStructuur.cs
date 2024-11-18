using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerghAdmin.Migrations
{
    /// <inheritdoc />
    public partial class FactuurStructuur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerzonden",
                table: "Facturen");

            migrationBuilder.AddColumn<int>(
                name: "FactuurStatus",
                table: "Facturen",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FactuurStatus",
                table: "Facturen");

            migrationBuilder.AddColumn<bool>(
                name: "IsVerzonden",
                table: "Facturen",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
