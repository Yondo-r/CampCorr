using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class CriandoTabelas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Campeonatos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Campeonatos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Kartodromos",
                columns: table => new
                {
                    KartodromoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Endereço = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kartodromos", x => x.KartodromoId);
                });

            migrationBuilder.CreateTable(
                name: "Piloto",
                columns: table => new
                {
                    PilotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoSanguineo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Peso = table.Column<decimal>(type: "Decimal(5,2)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipeId = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piloto", x => x.PilotoId);
                });

            migrationBuilder.CreateTable(
                name: "Regulamento",
                columns: table => new
                {
                    RegulamentoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regulamento", x => x.RegulamentoId);
                });

            migrationBuilder.CreateTable(
                name: "Temporadas",
                columns: table => new
                {
                    TemporadaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ano = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantidadeEtapas = table.Column<int>(type: "Int", nullable: false),
                    CampeonatoId = table.Column<int>(type: "int", nullable: false),
                    RegulamentoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temporadas", x => x.TemporadaId);
                    table.ForeignKey(
                        name: "FK_Temporadas_Campeonatos_CampeonatoId",
                        column: x => x.CampeonatoId,
                        principalTable: "Campeonatos",
                        principalColumn: "CampeonatoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Temporadas_Regulamento_RegulamentoId",
                        column: x => x.RegulamentoId,
                        principalTable: "Regulamento",
                        principalColumn: "RegulamentoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Etapas",
                columns: table => new
                {
                    EtapaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Traçado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TemporadaId = table.Column<int>(type: "int", nullable: false),
                    KartodromoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etapas", x => x.EtapaId);
                    table.ForeignKey(
                        name: "FK_Etapas_Kartodromos_KartodromoId",
                        column: x => x.KartodromoId,
                        principalTable: "Kartodromos",
                        principalColumn: "KartodromoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Etapas_Temporadas_TemporadaId",
                        column: x => x.TemporadaId,
                        principalTable: "Temporadas",
                        principalColumn: "TemporadaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Etapas_KartodromoId",
                table: "Etapas",
                column: "KartodromoId");

            migrationBuilder.CreateIndex(
                name: "IX_Etapas_TemporadaId",
                table: "Etapas",
                column: "TemporadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Temporadas_CampeonatoId",
                table: "Temporadas",
                column: "CampeonatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Temporadas_RegulamentoId",
                table: "Temporadas",
                column: "RegulamentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Etapas");

            migrationBuilder.DropTable(
                name: "Piloto");

            migrationBuilder.DropTable(
                name: "Kartodromos");

            migrationBuilder.DropTable(
                name: "Temporadas");

            migrationBuilder.DropTable(
                name: "Regulamento");

            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Campeonatos");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Campeonatos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
