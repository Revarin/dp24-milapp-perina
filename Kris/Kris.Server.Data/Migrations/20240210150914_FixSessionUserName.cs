using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSessionUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessioUsers_Sessions_SessionId",
                table: "SessioUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SessioUsers_Users_UserId",
                table: "SessioUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessioUsers",
                table: "SessioUsers");

            migrationBuilder.RenameTable(
                name: "SessioUsers",
                newName: "SessionUsers");

            migrationBuilder.RenameIndex(
                name: "IX_SessioUsers_UserId",
                table: "SessionUsers",
                newName: "IX_SessionUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SessioUsers_SessionId",
                table: "SessionUsers",
                newName: "IX_SessionUsers_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionUsers",
                table: "SessionUsers",
                columns: new[] { "UserId", "SessionId" });

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionUsers",
                table: "SessionUsers");

            migrationBuilder.RenameTable(
                name: "SessionUsers",
                newName: "SessioUsers");

            migrationBuilder.RenameIndex(
                name: "IX_SessionUsers_UserId",
                table: "SessioUsers",
                newName: "IX_SessioUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionUsers_SessionId",
                table: "SessioUsers",
                newName: "IX_SessioUsers_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessioUsers",
                table: "SessioUsers",
                columns: new[] { "UserId", "SessionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SessioUsers_Sessions_SessionId",
                table: "SessioUsers",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessioUsers_Users_UserId",
                table: "SessioUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
