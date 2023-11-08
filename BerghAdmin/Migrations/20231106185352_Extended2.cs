using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerghAdmin.Migrations
{
    /// <inheritdoc />
    public partial class Extended2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GolfdagSponsor_Golfdagen_GolfdagId",
                table: "GolfdagSponsor");

            migrationBuilder.DropIndex(
                name: "IX_GolfdagSponsor_GolfdagId",
                table: "GolfdagSponsor");

            migrationBuilder.DropColumn(
                name: "GolfdagId",
                table: "GolfdagSponsor");

            migrationBuilder.CreateTable(
                name: "GolfdagGolfdagSponsor",
                columns: table => new
                {
                    HeeftGesponsoredId = table.Column<int>(type: "int", nullable: false),
                    SponsorenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolfdagGolfdagSponsor", x => new { x.HeeftGesponsoredId, x.SponsorenId });
                    table.ForeignKey(
                        name: "FK_GolfdagGolfdagSponsor_GolfdagSponsor_SponsorenId",
                        column: x => x.SponsorenId,
                        principalTable: "GolfdagSponsor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GolfdagGolfdagSponsor_Golfdagen_HeeftGesponsoredId",
                        column: x => x.HeeftGesponsoredId,
                        principalTable: "Golfdagen",
                        principalColumn: "GolfdagId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GolfdagGolfdagSponsor_SponsorenId",
                table: "GolfdagGolfdagSponsor",
                column: "SponsorenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GolfdagGolfdagSponsor");

            migrationBuilder.AddColumn<int>(
                name: "GolfdagId",
                table: "GolfdagSponsor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GolfdagSponsor_GolfdagId",
                table: "GolfdagSponsor",
                column: "GolfdagId");

            migrationBuilder.AddForeignKey(
                name: "FK_GolfdagSponsor_Golfdagen_GolfdagId",
                table: "GolfdagSponsor",
                column: "GolfdagId",
                principalTable: "Golfdagen",
                principalColumn: "GolfdagId");
        }
    }
}
