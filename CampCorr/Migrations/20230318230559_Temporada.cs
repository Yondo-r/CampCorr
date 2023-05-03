using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class Temporada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Temporadas_Regulamento_RegulamentoId",
                table: "Temporadas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regulamento",
                table: "Regulamento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Piloto",
                table: "Piloto");

            migrationBuilder.RenameTable(
                name: "Regulamento",
                newName: "Regulamentos");

            migrationBuilder.RenameTable(
                name: "Piloto",
                newName: "Pilotos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regulamentos",
                table: "Regulamentos",
                column: "RegulamentoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pilotos",
                table: "Pilotos",
                column: "PilotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Temporadas_Regulamentos_RegulamentoId",
                table: "Temporadas",
                column: "RegulamentoId",
                principalTable: "Regulamentos",
                principalColumn: "RegulamentoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Temporadas_Regulamentos_RegulamentoId",
                table: "Temporadas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regulamentos",
                table: "Regulamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pilotos",
                table: "Pilotos");

            migrationBuilder.RenameTable(
                name: "Regulamentos",
                newName: "Regulamento");

            migrationBuilder.RenameTable(
                name: "Pilotos",
                newName: "Piloto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regulamento",
                table: "Regulamento",
                column: "RegulamentoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Piloto",
                table: "Piloto",
                column: "PilotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Temporadas_Regulamento_RegulamentoId",
                table: "Temporadas",
                column: "RegulamentoId",
                principalTable: "Regulamento",
                principalColumn: "RegulamentoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
