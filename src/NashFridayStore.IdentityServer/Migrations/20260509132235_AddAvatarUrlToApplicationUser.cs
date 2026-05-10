using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NashFridayStore.IdentityServer.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarUrlToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                schema: "auth",
                table: "users",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                schema: "auth",
                table: "users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar_url",
                schema: "auth",
                table: "users");

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                schema: "auth",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);
        }
    }
}
