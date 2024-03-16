using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerghAdmin.Migrations
{
    public partial class KledingMatenVeld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KledingMaten",
                table: "Donateur",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KledingMaten",
                table: "Donateur");
        }
    }
}
