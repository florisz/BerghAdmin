using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BerghAdmin.Migrations
{
    /// <inheritdoc />
    public partial class EvenementenNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvenementPersoon_Evenementen_IsDeelnemerVanId",
                table: "EvenementPersoon");

            migrationBuilder.RenameColumn(
                name: "IsDeelnemerVanId",
                table: "EvenementPersoon",
                newName: "FietsTochtenId");

            migrationBuilder.RenameIndex(
                name: "IX_EvenementPersoon_IsDeelnemerVanId",
                table: "EvenementPersoon",
                newName: "IX_EvenementPersoon_FietsTochtenId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Rollen",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_EvenementPersoon_Evenementen_FietsTochtenId",
                table: "EvenementPersoon",
                column: "FietsTochtenId",
                principalTable: "Evenementen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvenementPersoon_Evenementen_FietsTochtenId",
                table: "EvenementPersoon");

            migrationBuilder.RenameColumn(
                name: "FietsTochtenId",
                table: "EvenementPersoon",
                newName: "IsDeelnemerVanId");

            migrationBuilder.RenameIndex(
                name: "IX_EvenementPersoon_FietsTochtenId",
                table: "EvenementPersoon",
                newName: "IX_EvenementPersoon_IsDeelnemerVanId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Rollen",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_EvenementPersoon_Evenementen_IsDeelnemerVanId",
                table: "EvenementPersoon",
                column: "IsDeelnemerVanId",
                principalTable: "Evenementen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
