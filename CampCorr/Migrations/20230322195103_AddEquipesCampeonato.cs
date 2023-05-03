using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class AddEquipesCampeonato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipeId",
                table: "Pilotos");

            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Equipes",
                newName: "Emblema");

            migrationBuilder.AddColumn<int>(
                name: "CampeonatoId",
                table: "Equipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TemporadaId",
                table: "Equipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_CampeonatoId",
                table: "Equipes",
                column: "CampeonatoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipes_Campeonatos_CampeonatoId",
                table: "Equipes",
                column: "CampeonatoId",
                principalTable: "Campeonatos",
                principalColumn: "CampeonatoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipes_Campeonatos_CampeonatoId",
                table: "Equipes");

            migrationBuilder.DropIndex(
                name: "IX_Equipes_CampeonatoId",
                table: "Equipes");

            migrationBuilder.DropColumn(
                name: "CampeonatoId",
                table: "Equipes");

            migrationBuilder.DropColumn(
                name: "TemporadaId",
                table: "Equipes");

            migrationBuilder.RenameColumn(
                name: "Emblema",
                table: "Equipes",
                newName: "MyProperty");

            migrationBuilder.AddColumn<int>(
                name: "EquipeId",
                table: "Pilotos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
