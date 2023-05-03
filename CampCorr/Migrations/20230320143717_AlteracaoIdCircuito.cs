using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class AlteracaoIdCircuito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KartodromoId",
                table: "Circuitos",
                newName: "CircuitoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CircuitoId",
                table: "Circuitos",
                newName: "KartodromoId");
        }
    }
}
