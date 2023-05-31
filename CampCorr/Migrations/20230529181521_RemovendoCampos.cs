using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class RemovendoCampos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Pilotos");

            migrationBuilder.DropColumn(
                name: "Emblema",
                table: "Equipes");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Campeonatos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Pilotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Emblema",
                table: "Equipes",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Campeonatos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
