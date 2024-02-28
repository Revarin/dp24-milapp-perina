using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class MergeMainBranch2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPositions_SessionUsers_SessionUserId",
                table: "UserPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "SessionUserId",
                table: "UserPositions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositions_SessionUsers_Id",
                table: "UserPositions",
                column: "Id",
                principalTable: "SessionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPositions_SessionUsers_Id",
                table: "UserPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions");

            migrationBuilder.AddColumn<Guid>(
                name: "SessionUserId",
                table: "UserPositions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions",
                column: "SessionUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositions_SessionUsers_SessionUserId",
                table: "UserPositions",
                column: "SessionUserId",
                principalTable: "SessionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
