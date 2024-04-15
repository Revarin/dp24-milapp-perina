using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedFlagToMapObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "MapPoints",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "MapPoints");
        }
    }
}
