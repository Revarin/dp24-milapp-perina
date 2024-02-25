using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterUserPositionPositionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionsData",
                table: "UserPositions");

            migrationBuilder.AddColumn<string>(
                name: "Position0Data",
                table: "UserPositions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position1Data",
                table: "UserPositions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position2Data",
                table: "UserPositions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position0Data",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position1Data",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position2Data",
                table: "UserPositions");

            migrationBuilder.AddColumn<string>(
                name: "PositionsData",
                table: "UserPositions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
