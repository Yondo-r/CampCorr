using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class AlteracaoNomeCircuito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etapas_Kartodromos_KartodromoId",
                table: "Etapas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kartodromos",
                table: "Kartodromos");

            migrationBuilder.RenameTable(
                name: "Kartodromos",
                newName: "Circuitos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Circuitos",
                table: "Circuitos",
                column: "KartodromoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Etapas_Circuitos_KartodromoId",
                table: "Etapas",
                column: "KartodromoId",
                principalTable: "Circuitos",
                principalColumn: "KartodromoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etapas_Circuitos_KartodromoId",
                table: "Etapas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Circuitos",
                table: "Circuitos");

            migrationBuilder.RenameTable(
                name: "Circuitos",
                newName: "Kartodromos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kartodromos",
                table: "Kartodromos",
                column: "KartodromoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Etapas_Kartodromos_KartodromoId",
                table: "Etapas",
                column: "KartodromoId",
                principalTable: "Kartodromos",
                principalColumn: "KartodromoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
