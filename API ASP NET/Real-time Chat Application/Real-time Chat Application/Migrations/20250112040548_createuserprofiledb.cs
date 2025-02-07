using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_time_Chat_Application.Migrations
{
    /// <inheritdoc />
    public partial class createuserprofiledb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatDB",
                columns: table => new
                {
                    ChatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatDB", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "UsersDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Authorized = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FriendshipsDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FriendId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendshipsDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendshipsDB_UsersDB_FriendId",
                        column: x => x.FriendId,
                        principalTable: "UsersDB",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FriendshipsDB_UsersDB_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersDB",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MessagesDB",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    Sentiment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesDB", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_MessagesDB_ChatDB_ChatId",
                        column: x => x.ChatId,
                        principalTable: "ChatDB",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessagesDB_UsersDB_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChatDB",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChatDB", x => new { x.UserId, x.ChatId });
                    table.ForeignKey(
                        name: "FK_UserChatDB_ChatDB_ChatId",
                        column: x => x.ChatId,
                        principalTable: "ChatDB",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChatDB_UsersDB_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfilesDB",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AboutMe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterestedIn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowInstagram = table.Column<bool>(type: "bit", nullable: false),
                    Telegram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowTelegram = table.Column<bool>(type: "bit", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfilesDB", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserProfilesDB_UsersDB_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendshipsDB_FriendId",
                table: "FriendshipsDB",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendshipsDB_UserId",
                table: "FriendshipsDB",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesDB_ChatId",
                table: "MessagesDB",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesDB_UserId",
                table: "MessagesDB",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChatDB_ChatId",
                table: "UserChatDB",
                column: "ChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendshipsDB");

            migrationBuilder.DropTable(
                name: "MessagesDB");

            migrationBuilder.DropTable(
                name: "UserChatDB");

            migrationBuilder.DropTable(
                name: "UserProfilesDB");

            migrationBuilder.DropTable(
                name: "ChatDB");

            migrationBuilder.DropTable(
                name: "UsersDB");
        }
    }
}
