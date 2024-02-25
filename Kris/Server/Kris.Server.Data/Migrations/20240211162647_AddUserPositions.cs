using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "SessionUsers");

            migrationBuilder.CreateTable(
                name: "UserPositions",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PositionsData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPositions", x => new { x.UserId, x.SessionId });
                    table.ForeignKey(
                        name: "FK_UserPositions_SessionUsers_UserId_SessionId",
                        columns: x => new { x.UserId, x.SessionId },
                        principalTable: "SessionUsers",
                        principalColumns: new[] { "UserId", "SessionId" });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPositions");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "SessionUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
