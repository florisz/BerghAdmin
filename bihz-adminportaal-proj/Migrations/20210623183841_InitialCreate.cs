using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BIHZ.AdminPortaal.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Geslacht = table.Column<int>(type: "int", nullable: false),
                    Voorletters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voornaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Achternaam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plaats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Land = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefoon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobiel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAdres = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersoonRol",
                columns: table => new
                {
                    PersoonId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersoonRol", x => new { x.PersoonId, x.RolId });
                });

            migrationBuilder.CreateTable(
                name: "Rollen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Beschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rollen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersoonInRol",
                columns: table => new
                {
                    PersonenId = table.Column<int>(type: "int", nullable: false),
                    RollenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersoonInRol", x => new { x.PersonenId, x.RollenId });
                    table.ForeignKey(
                        name: "FK_PersoonInRol_Personen_PersonenId",
                        column: x => x.PersonenId,
                        principalTable: "Personen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersoonInRol_Rollen_RollenId",
                        column: x => x.RollenId,
                        principalTable: "Rollen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Personen",
                columns: new[] { "Id", "Achternaam", "Adres", "EmailAdres", "GeboorteDatum", "Geslacht", "Land", "Mobiel", "Plaats", "Postcode", "Telefoon", "Voorletters", "Voornaam" },
                values: new object[,]
                {
                    { 1, "Happie", "Straat 1", "ahappie@mail.com", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Nederland", "06-12345678", "Amsterdam", "1234 AB", "onbekend", "A. B.", "Appie" },
                    { 2, "Bengel", "Straat 2", "bbengel@mail.com", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Nederland", "06-12345678", "Rotterdam", "4321 AB", "onbekend", "B.", "Bert" }
                });

            migrationBuilder.InsertData(
                table: "Rollen",
                columns: new[] { "Id", "Beschrijving" },
                values: new object[,]
                {
                    { 1, "Renner" },
                    { 2, "Begeleider" },
                    { 3, "Reserve" },
                    { 4, "Commissielid" },
                    { 5, "Vriend van" },
                    { 6, "Mailing abonnee" },
                    { 7, "Golfer" },
                    { 8, "Ambassadeur" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersoonInRol_RollenId",
                table: "PersoonInRol",
                column: "RollenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersoonInRol");

            migrationBuilder.DropTable(
                name: "PersoonRol");

            migrationBuilder.DropTable(
                name: "Personen");

            migrationBuilder.DropTable(
                name: "Rollen");
        }
    }
}
