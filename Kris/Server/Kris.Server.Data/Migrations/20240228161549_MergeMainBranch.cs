using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class MergeMainBranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<double>(
                name: "Position_0_Altitude",
                table: "UserPositions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Position_0_Latitude",
                table: "UserPositions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Position_0_Longitude",
                table: "UserPositions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Position_0_Timestamp",
                table: "UserPositions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Position_1_Altitude",
                table: "UserPositions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Position_1_Latitude",
                table: "UserPositions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Position_1_Longitude",
                table: "UserPositions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Position_1_Timestamp",
                table: "UserPositions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Position_2_Altitude",
                table: "UserPositions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Position_2_Latitude",
                table: "UserPositions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Position_2_Longitude",
                table: "UserPositions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Position_2_Timestamp",
                table: "UserPositions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MapPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position_Latitude = table.Column<double>(type: "float", nullable: false),
                    Position_Longitude = table.Column<double>(type: "float", nullable: false),
                    Position_Altitude = table.Column<double>(type: "float", nullable: false),
                    Symbol_Shape = table.Column<int>(type: "int", nullable: false),
                    Symbol_Color = table.Column<int>(type: "int", nullable: false),
                    Symbol_Sign = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    SessionUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapPoints_SessionUsers_SessionUserId",
                        column: x => x.SessionUserId,
                        principalTable: "SessionUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapPoints_SessionUserId",
                table: "MapPoints",
                column: "SessionUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapPoints");

            migrationBuilder.DropColumn(
                name: "Position_0_Altitude",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_0_Latitude",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_0_Longitude",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_0_Timestamp",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_1_Altitude",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_1_Latitude",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_1_Longitude",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_1_Timestamp",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_2_Altitude",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_2_Latitude",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_2_Longitude",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Position_2_Timestamp",
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
    }
}
