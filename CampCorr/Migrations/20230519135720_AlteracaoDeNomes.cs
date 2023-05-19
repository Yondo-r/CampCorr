using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class AlteracaoDeNomes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etapas_Circuitos_KartodromoId",
                table: "Etapas");

            migrationBuilder.RenameColumn(
                name: "KartodromoId",
                table: "Etapas",
                newName: "CircuitoId");

            migrationBuilder.RenameIndex(
                name: "IX_Etapas_KartodromoId",
                table: "Etapas",
                newName: "IX_Etapas_CircuitoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Etapas_Circuitos_CircuitoId",
                table: "Etapas",
                column: "CircuitoId",
                principalTable: "Circuitos",
                principalColumn: "CircuitoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etapas_Circuitos_CircuitoId",
                table: "Etapas");

            migrationBuilder.RenameColumn(
                name: "CircuitoId",
                table: "Etapas",
                newName: "KartodromoId");

            migrationBuilder.RenameIndex(
                name: "IX_Etapas_CircuitoId",
                table: "Etapas",
                newName: "IX_Etapas_KartodromoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Etapas_Circuitos_KartodromoId",
                table: "Etapas",
                column: "KartodromoId",
                principalTable: "Circuitos",
                principalColumn: "CircuitoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
