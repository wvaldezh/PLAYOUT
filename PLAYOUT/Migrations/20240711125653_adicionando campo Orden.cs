using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PLAYOUT.Migrations
{
    /// <inheritdoc />
    public partial class adicionandocampoOrden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "Spots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "Musicales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProgramacionId",
                table: "Canales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Canales_ProgramacionId",
                table: "Canales",
                column: "ProgramacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Canales_Programaciones_ProgramacionId",
                table: "Canales",
                column: "ProgramacionId",
                principalTable: "Programaciones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Canales_Programaciones_ProgramacionId",
                table: "Canales");

            migrationBuilder.DropIndex(
                name: "IX_Canales_ProgramacionId",
                table: "Canales");

            migrationBuilder.DropColumn(
                name: "Orden",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "Orden",
                table: "Musicales");

            migrationBuilder.DropColumn(
                name: "ProgramacionId",
                table: "Canales");
        }
    }
}
