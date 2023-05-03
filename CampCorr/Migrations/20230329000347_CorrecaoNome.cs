using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class CorrecaoNome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PilotosTemporadasTemporadas",
                table: "PilotosTemporadasTemporadas");

            migrationBuilder.RenameTable(
                name: "PilotosTemporadasTemporadas",
                newName: "PilotosTemporadas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PilotosTemporadas",
                table: "PilotosTemporadas",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PilotosTemporadas",
                table: "PilotosTemporadas");

            migrationBuilder.RenameTable(
                name: "PilotosTemporadas",
                newName: "PilotosTemporadasTemporadas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PilotosTemporadasTemporadas",
                table: "PilotosTemporadasTemporadas",
                column: "Id");
        }
    }
}
