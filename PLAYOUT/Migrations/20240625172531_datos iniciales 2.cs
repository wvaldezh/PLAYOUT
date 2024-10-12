using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PLAYOUT.Migrations
{
    /// <inheritdoc />
    public partial class datosiniciales2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Canals",
                table: "Canals");

            migrationBuilder.RenameTable(
                name: "Canals",
                newName: "Canales");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Canales",
                table: "Canales",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Canales",
                table: "Canales");

            migrationBuilder.RenameTable(
                name: "Canales",
                newName: "Canals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Canals",
                table: "Canals",
                column: "Id");
        }
    }
}
