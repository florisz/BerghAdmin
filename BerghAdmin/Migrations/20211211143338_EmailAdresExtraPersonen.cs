using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerghAdmin.Migrations
{
    public partial class EmailAdresExtraPersonen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>("IsVerwijderd", "Personen", "bit", defaultValue: false);
            migrationBuilder.AddColumn<string>("EmailAdresExtra", "Personen", "nvarchar(256)", maxLength:256, nullable:true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("IsVerwijderd", "Personen");
            migrationBuilder.DropColumn("EmailAdresExtra", "Personen");

        }
    }
}