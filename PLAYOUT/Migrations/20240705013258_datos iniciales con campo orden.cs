using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PLAYOUT.Migrations
{
    /// <inheritdoc />
    public partial class datosinicialesconcampoorden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "Canales",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Orden",
                table: "Canales");
        }
    }
}
