using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class CorrecaoNomeColuna2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TempoMelhoVolta",
                table: "ResultadosCorrida",
                newName: "TempoMelhorVolta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TempoMelhorVolta",
                table: "ResultadosCorrida",
                newName: "TempoMelhoVolta");
        }
    }
}
