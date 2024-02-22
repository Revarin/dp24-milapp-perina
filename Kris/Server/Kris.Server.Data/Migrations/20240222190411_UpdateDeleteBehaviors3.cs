using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviors3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPositions_SessionUsers_SessionUserId",
                table: "UserPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SessionUsers_CurrentSessionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentSessionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserPositions");

            migrationBuilder.AlterColumn<Guid>(
                name: "SessionUserId",
                table: "UserPositions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentSessionId",
                table: "Users",
                column: "CurrentSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositions_SessionUsers_SessionUserId",
                table: "UserPositions",
                column: "SessionUserId",
                principalTable: "SessionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SessionUsers_CurrentSessionId",
                table: "Users",
                column: "CurrentSessionId",
                principalTable: "SessionUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPositions_SessionUsers_SessionUserId",
                table: "UserPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SessionUsers_CurrentSessionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentSessionId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "SessionUserId",
                table: "UserPositions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "SessionId",
                table: "UserPositions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserPositions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentSessionId",
                table: "Users",
                column: "CurrentSessionId",
                unique: true,
                filter: "[CurrentSessionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositions_SessionUsers_SessionUserId",
                table: "UserPositions",
                column: "SessionUserId",
                principalTable: "SessionUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SessionUsers_CurrentSessionId",
                table: "Users",
                column: "CurrentSessionId",
                principalTable: "SessionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
