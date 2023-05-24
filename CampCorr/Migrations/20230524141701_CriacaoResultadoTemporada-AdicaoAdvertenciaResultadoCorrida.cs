using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class CriacaoResultadoTemporadaAdicaoAdvertenciaResultadoCorrida : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Advertencia",
                table: "ResultadosCorrida",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ResultadosTemporada",
                columns: table => new
                {
                    ResultadoTemporadaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemporadaId = table.Column<int>(type: "int", nullable: false),
                    PilotoId = table.Column<int>(type: "int", nullable: false),
                    EquipeId = table.Column<int>(type: "int", nullable: false),
                    NumeroVitorias = table.Column<int>(type: "int", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false),
                    Posicao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosTemporada", x => x.ResultadoTemporadaId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultadosTemporada");

            migrationBuilder.DropColumn(
                name: "Advertencia",
                table: "ResultadosCorrida");
        }
    }
}
