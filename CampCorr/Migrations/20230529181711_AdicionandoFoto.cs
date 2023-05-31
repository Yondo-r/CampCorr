using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampCorr.Migrations
{
    public partial class AdicionandoFoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImagemResultadoCorrida",
                table: "ResultadosCorrida",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "Pilotos",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Emblema",
                table: "Equipes",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Logo",
                table: "Campeonatos",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemResultadoCorrida",
                table: "ResultadosCorrida");

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
    }
}
