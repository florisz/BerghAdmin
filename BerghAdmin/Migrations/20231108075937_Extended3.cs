using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerghAdmin.Migrations
{
    /// <inheritdoc />
    public partial class Extended3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GolfdagGolfdagSponsor_Golfdagen_HeeftGesponsoredId",
                table: "GolfdagGolfdagSponsor");

            migrationBuilder.RenameColumn(
                name: "HeeftGesponsoredId",
                table: "GolfdagGolfdagSponsor",
                newName: "GolfdagenGesponsoredId");

            migrationBuilder.AddForeignKey(
                name: "FK_GolfdagGolfdagSponsor_Golfdagen_GolfdagenGesponsoredId",
                table: "GolfdagGolfdagSponsor",
                column: "GolfdagenGesponsoredId",
                principalTable: "Golfdagen",
                principalColumn: "GolfdagId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GolfdagGolfdagSponsor_Golfdagen_GolfdagenGesponsoredId",
                table: "GolfdagGolfdagSponsor");

            migrationBuilder.RenameColumn(
                name: "GolfdagenGesponsoredId",
                table: "GolfdagGolfdagSponsor",
                newName: "HeeftGesponsoredId");

            migrationBuilder.AddForeignKey(
                name: "FK_GolfdagGolfdagSponsor_Golfdagen_HeeftGesponsoredId",
                table: "GolfdagGolfdagSponsor",
                column: "HeeftGesponsoredId",
                principalTable: "Golfdagen",
                principalColumn: "GolfdagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
