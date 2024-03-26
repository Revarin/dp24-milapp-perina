using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToCaseSensitive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase(
                collation: "SQL_Latin1_General_CP1_CS_AS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase(
                oldCollation: "SQL_Latin1_General_CP1_CS_AS");
        }
    }
}
