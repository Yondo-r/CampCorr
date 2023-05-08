using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class AdicionandoTotalVoltas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalVoltas",
                table: "ResultadosCorrida",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalVoltas",
                table: "ResultadosCorrida");
        }
    }
}
