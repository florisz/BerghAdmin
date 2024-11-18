using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerghAdmin.Migrations
{
    /// <inheritdoc />
    public partial class SponsorUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturen_Ambassadeur_AmbassadeurId",
                table: "Facturen");

            migrationBuilder.RenameColumn(
                name: "AmbassadeurId",
                table: "Facturen",
                newName: "SponsorId");

            migrationBuilder.RenameIndex(
                name: "IX_Facturen_AmbassadeurId",
                table: "Facturen",
                newName: "IX_Facturen_SponsorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturen_Sponsoren_SponsorId",
                table: "Facturen",
                column: "SponsorId",
                principalTable: "Sponsoren",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturen_Sponsoren_SponsorId",
                table: "Facturen");

            migrationBuilder.RenameColumn(
                name: "SponsorId",
                table: "Facturen",
                newName: "AmbassadeurId");

            migrationBuilder.RenameIndex(
                name: "IX_Facturen_SponsorId",
                table: "Facturen",
                newName: "IX_Facturen_AmbassadeurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturen_Ambassadeur_AmbassadeurId",
                table: "Facturen",
                column: "AmbassadeurId",
                principalTable: "Ambassadeur",
                principalColumn: "Id");
        }
    }
}
