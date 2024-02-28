using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionUserEntity_Sessions_SessionId",
                table: "SessionUserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionUserEntity_Users_UserId",
                table: "SessionUserEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionUserEntity",
                table: "SessionUserEntity");

            migrationBuilder.RenameTable(
                name: "SessionUserEntity",
                newName: "SessioUsers");

            migrationBuilder.RenameIndex(
                name: "IX_SessionUserEntity_UserId",
                table: "SessioUsers",
                newName: "IX_SessioUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionUserEntity_SessionId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "SessionUserEntity");

            migrationBuilder.RenameIndex(
                name: "IX_SessioUsers_UserId",
                table: "SessionUserEntity",
                newName: "IX_SessionUserEntity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SessioUsers_SessionId",
                table: "SessionUserEntity",
                newName: "IX_SessionUserEntity_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionUserEntity",
                table: "SessionUserEntity",
                columns: new[] { "UserId", "SessionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SessionUserEntity_Sessions_SessionId",
                table: "SessionUserEntity",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionUserEntity_Users_UserId",
                table: "SessionUserEntity",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
