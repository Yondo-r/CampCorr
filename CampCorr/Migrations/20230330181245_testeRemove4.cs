using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class testeRemove4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TemporadaId",
                table: "Equipes",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.DropIndex(
                name: "IX_Equipes_TemporadaId",
                table: "Equipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipes_Temporadas_TemporadaId",
                table: "Equipes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Equipes_Temporadas_TemporadaId",
                table: "Equipes",
                column: "TemporadaId",
                principalTable: "Temporadas",
                principalColumn: "TemporadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_TemporadaId",
                table: "Equipes",
                column: "TemporadaId");

            migrationBuilder.AlterColumn<int>(
                name: "TemporadaId",
                table: "Equipes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
