using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSChallenger.Server.Migrations
{
    /// <inheritdoc />
    public partial class UseBeatsaverKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "hash",
                table: "map",
                newName: "beat_saver_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "beat_saver_id",
                table: "map",
                newName: "hash");
        }
    }
}
