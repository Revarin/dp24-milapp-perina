using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviors2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SessionUsers_CurrentSessionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentSessionId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentSessionId",
                table: "Users",
                column: "CurrentSessionId",
                unique: true,
                filter: "[CurrentSessionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SessionUsers_CurrentSessionId",
                table: "Users",
                column: "CurrentSessionId",
                principalTable: "SessionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SessionUsers_CurrentSessionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CurrentSessionId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentSessionId",
                table: "Users",
                column: "CurrentSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SessionUsers_CurrentSessionId",
                table: "Users",
                column: "CurrentSessionId",
                principalTable: "SessionUsers",
                principalColumn: "Id");
        }
    }
}
