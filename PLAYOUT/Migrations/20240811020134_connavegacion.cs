using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PLAYOUT.Migrations
{
    /// <inheritdoc />
    public partial class connavegacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Programaciones_CanalId",
                table: "Programaciones",
                column: "CanalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Programaciones_Canales_CanalId",
                table: "Programaciones",
                column: "CanalId",
                principalTable: "Canales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
        }
    }
}
