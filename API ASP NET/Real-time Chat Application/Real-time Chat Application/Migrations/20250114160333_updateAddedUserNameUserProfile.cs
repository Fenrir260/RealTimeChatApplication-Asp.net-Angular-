using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_time_Chat_Application.Migrations
{
    /// <inheritdoc />
    public partial class updateAddedUserNameUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserProfilesDB",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserProfilesDB");
        }
    }
}
