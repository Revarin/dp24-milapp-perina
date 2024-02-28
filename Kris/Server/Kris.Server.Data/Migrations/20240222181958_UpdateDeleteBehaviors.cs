using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionUsers_Sessions_SessionId",
                table: "SessionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionUsers_Users_UserId",
                table: "SessionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPositions_SessionUsers_UserId_SessionId",
                table: "UserPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SessionUsers_Id_CurrentSessionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Id_CurrentSessionId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionUsers",
                table: "SessionUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserPositions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SessionUserId",
                table: "UserPositions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "SessionUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionUsers",
                table: "SessionUsers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentSessionId",
                table: "Users",
                column: "CurrentSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPositions_SessionUserId",
                table: "UserPositions",
                column: "SessionUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionUsers_UserId",
                table: "SessionUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionUsers_Sessions_SessionId",
                table: "SessionUsers",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionUsers_Users_UserId",
                table: "SessionUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionUsers_Sessions_SessionId",
                table: "SessionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionUsers_Users_UserId",
                table: "SessionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPositions_SessionUsers_SessionUserId",
                table: "UserPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SessionUsers_CurrentSessionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentSessionId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions");

            migrationBuilder.DropIndex(
                name: "IX_UserPositions_SessionUserId",
                table: "UserPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionUsers",
                table: "SessionUsers");

            migrationBuilder.DropIndex(
                name: "IX_SessionUsers_UserId",
                table: "SessionUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "SessionUserId",
                table: "UserPositions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SessionUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPositions",
                table: "UserPositions",
                columns: new[] { "UserId", "SessionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionUsers",
                table: "SessionUsers",
                columns: new[] { "UserId", "SessionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id_CurrentSessionId",
                table: "Users",
                columns: new[] { "Id", "CurrentSessionId" },
                unique: true,
                filter: "[CurrentSessionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionUsers_Sessions_SessionId",
                table: "SessionUsers",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionUsers_Users_UserId",
                table: "SessionUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPositions_SessionUsers_UserId_SessionId",
                table: "UserPositions",
                columns: new[] { "UserId", "SessionId" },
                principalTable: "SessionUsers",
                principalColumns: new[] { "UserId", "SessionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SessionUsers_Id_CurrentSessionId",
                table: "Users",
                columns: new[] { "Id", "CurrentSessionId" },
                principalTable: "SessionUsers",
                principalColumns: new[] { "UserId", "SessionId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
