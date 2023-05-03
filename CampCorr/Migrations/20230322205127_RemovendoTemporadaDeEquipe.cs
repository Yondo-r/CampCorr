using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class RemovendoTemporadaDeEquipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TemporadaId",
                table: "Equipes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_TemporadaId",
                table: "Equipes",
                column: "TemporadaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipes_Temporadas_TemporadaId",
                table: "Equipes",
                column: "TemporadaId",
                principalTable: "Temporadas",
                principalColumn: "TemporadaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipes_Temporadas_TemporadaId",
                table: "Equipes");

            migrationBuilder.DropIndex(
                name: "IX_Equipes_TemporadaId",
                table: "Equipes");

            migrationBuilder.AlterColumn<int>(
                name: "TemporadaId",
                table: "Equipes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
