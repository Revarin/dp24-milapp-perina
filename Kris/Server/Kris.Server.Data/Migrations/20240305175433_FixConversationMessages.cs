using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kris.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixConversationMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationEntity_Sessions_SessionId",
                table: "ConversationEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversationEntitySessionUserEntity_ConversationEntity_ConversationsId",
                table: "ConversationEntitySessionUserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageEntity_ConversationEntity_ConversationId",
                table: "MessageEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageEntity_SessionUsers_SenderId",
                table: "MessageEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageEntity",
                table: "MessageEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConversationEntity",
                table: "ConversationEntity");

            migrationBuilder.RenameTable(
                name: "MessageEntity",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "ConversationEntity",
                newName: "Conversations");

            migrationBuilder.RenameIndex(
                name: "IX_MessageEntity_SenderId",
                table: "Messages",
                newName: "IX_Messages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageEntity_ConversationId",
                table: "Messages",
                newName: "IX_Messages_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_ConversationEntity_SessionId",
                table: "Conversations",
                newName: "IX_Conversations_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Conversations",
                table: "Conversations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationEntitySessionUserEntity_Conversations_ConversationsId",
                table: "ConversationEntitySessionUserEntity",
                column: "ConversationsId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Sessions_SessionId",
                table: "Conversations",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_SessionUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "SessionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationEntitySessionUserEntity_Conversations_ConversationsId",
                table: "ConversationEntitySessionUserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Sessions_SessionId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_SessionUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Conversations",
                table: "Conversations");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "MessageEntity");

            migrationBuilder.RenameTable(
                name: "Conversations",
                newName: "ConversationEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderId",
                table: "MessageEntity",
                newName: "IX_MessageEntity_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ConversationId",
                table: "MessageEntity",
                newName: "IX_MessageEntity_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_Conversations_SessionId",
                table: "ConversationEntity",
                newName: "IX_ConversationEntity_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageEntity",
                table: "MessageEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConversationEntity",
                table: "ConversationEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationEntity_Sessions_SessionId",
                table: "ConversationEntity",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationEntitySessionUserEntity_ConversationEntity_ConversationsId",
                table: "ConversationEntitySessionUserEntity",
                column: "ConversationsId",
                principalTable: "ConversationEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageEntity_ConversationEntity_ConversationId",
                table: "MessageEntity",
                column: "ConversationId",
                principalTable: "ConversationEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageEntity_SessionUsers_SenderId",
                table: "MessageEntity",
                column: "SenderId",
                principalTable: "SessionUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
