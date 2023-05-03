using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class AddPilotosNaTemporada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TemporadaId",
                table: "Pilotos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pilotos_TemporadaId",
                table: "Pilotos",
                column: "TemporadaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pilotos_Temporadas_TemporadaId",
                table: "Pilotos",
                column: "TemporadaId",
                principalTable: "Temporadas",
                principalColumn: "TemporadaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pilotos_Temporadas_TemporadaId",
                table: "Pilotos");

            migrationBuilder.DropIndex(
                name: "IX_Pilotos_TemporadaId",
                table: "Pilotos");

            migrationBuilder.DropColumn(
                name: "TemporadaId",
                table: "Pilotos");
        }
    }
}
