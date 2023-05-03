using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class Resultado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultadosCorrida",
                columns: table => new
                {
                    ResultadoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EtapaId = table.Column<int>(type: "int", nullable: false),
                    Posicao = table.Column<int>(type: "int", nullable: true),
                    Pontos = table.Column<int>(type: "int", nullable: true),
                    EquipeId = table.Column<int>(type: "int", nullable: false),
                    PilotoId = table.Column<int>(type: "int", nullable: false),
                    DescriçãoPenalidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PontosPenalidade = table.Column<int>(type: "int", nullable: true),
                    MelhorVolta = table.Column<bool>(type: "bit", nullable: false),
                    TempoMelhoVolta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PosicaoLargada = table.Column<int>(type: "int", nullable: true),
                    TempoTotal = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosCorrida", x => x.ResultadoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultadosCorrida");
        }
    }
}
