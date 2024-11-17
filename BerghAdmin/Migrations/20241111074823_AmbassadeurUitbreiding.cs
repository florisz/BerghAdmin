using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerghAdmin.Migrations
{
    /// <inheritdoc />
    public partial class AmbassadeurUitbreiding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactuurVerzendWijze",
                table: "Sponsoren",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Bedrag",
                table: "Facturen",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AmbassadeurId",
                table: "Facturen",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturen_AmbassadeurId",
                table: "Facturen",
                column: "AmbassadeurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturen_Ambassadeur_AmbassadeurId",
                table: "Facturen",
                column: "AmbassadeurId",
                principalTable: "Ambassadeur",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturen_Ambassadeur_AmbassadeurId",
                table: "Facturen");

            migrationBuilder.DropIndex(
                name: "IX_Facturen_AmbassadeurId",
                table: "Facturen");

            migrationBuilder.DropColumn(
                name: "FactuurVerzendWijze",
                table: "Sponsoren");

            migrationBuilder.DropColumn(
                name: "AmbassadeurId",
                table: "Facturen");

            migrationBuilder.AlterColumn<float>(
                name: "Bedrag",
                table: "Facturen",
                type: "float",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
