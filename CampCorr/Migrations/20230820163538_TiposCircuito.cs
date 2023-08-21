using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class TiposCircuito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Circuitos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Circuitos");
        }
    }
}
