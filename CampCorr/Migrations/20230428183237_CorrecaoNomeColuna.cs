using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class CorrecaoNomeColuna : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DescriçãoPenalidade",
                table: "ResultadosCorrida",
                newName: "DescricaoPenalidade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DescricaoPenalidade",
                table: "ResultadosCorrida",
                newName: "DescriçãoPenalidade");
        }
    }
}
