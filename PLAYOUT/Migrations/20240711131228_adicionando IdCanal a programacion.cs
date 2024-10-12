using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PLAYOUT.Migrations
{
    /// <inheritdoc />
    public partial class adicionandoIdCanalaprogramacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Canales_Programaciones_ProgramacionId",
                table: "Canales");

            migrationBuilder.DropIndex(
                name: "IX_Canales_ProgramacionId",
                table: "Canales");

            migrationBuilder.DropColumn(
                name: "ProgramacionId",
                table: "Canales");

            migrationBuilder.AlterColumn<Guid>(
                name: "CanalId",
                table: "Programaciones",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Programaciones_CanalId",
                table: "Programaciones",
                column: "CanalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Programaciones_Canales_CanalId",
                table: "Programaciones",
                column: "CanalId",
                principalTable: "Canales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programaciones_Canales_CanalId",
                table: "Programaciones");

            migrationBuilder.DropIndex(
                name: "IX_Programaciones_CanalId",
                table: "Programaciones");

            migrationBuilder.AlterColumn<Guid>(
                name: "CanalId",
                table: "Programaciones",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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
    }
}
