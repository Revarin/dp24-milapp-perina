using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationsMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConversationEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConversationType = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationEntity_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConversationEntitySessionUserEntity",
                columns: table => new
                {
                    ConversationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationEntitySessionUserEntity", x => new { x.ConversationsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ConversationEntitySessionUserEntity_ConversationEntity_ConversationsId",
                        column: x => x.ConversationsId,
                        principalTable: "ConversationEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationEntitySessionUserEntity_SessionUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "SessionUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageEntity_ConversationEntity_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "ConversationEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageEntity_SessionUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "SessionUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationEntity_SessionId",
                table: "ConversationEntity",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationEntitySessionUserEntity_UsersId",
                table: "ConversationEntitySessionUserEntity",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageEntity_ConversationId",
                table: "MessageEntity",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageEntity_SenderId",
                table: "MessageEntity",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationEntitySessionUserEntity");

            migrationBuilder.DropTable(
                name: "MessageEntity");

            migrationBuilder.DropTable(
                name: "ConversationEntity");
        }
    }
}
