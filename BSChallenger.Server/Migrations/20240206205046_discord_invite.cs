using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSChallenger.Server.Migrations
{
    /// <inheritdoc />
    public partial class discord_invite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "discord_url",
                table: "rankings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discord_url",
                table: "rankings");
        }
    }
}
