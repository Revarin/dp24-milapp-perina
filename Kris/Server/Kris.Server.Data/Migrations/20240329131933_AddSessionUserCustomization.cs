using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionUserCustomization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "SessionUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Symbol_Color",
                table: "SessionUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Symbol_Shape",
                table: "SessionUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Symbol_Sign",
                table: "SessionUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "SessionUsers");

            migrationBuilder.DropColumn(
                name: "Symbol_Color",
                table: "SessionUsers");

            migrationBuilder.DropColumn(
                name: "Symbol_Shape",
                table: "SessionUsers");

            migrationBuilder.DropColumn(
                name: "Symbol_Sign",
                table: "SessionUsers");
        }
    }
}
