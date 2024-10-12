using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PLAYOUT.Migrations
{
    /// <inheritdoc />
    public partial class adicionandoquitandoIdCanalaprogramacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
