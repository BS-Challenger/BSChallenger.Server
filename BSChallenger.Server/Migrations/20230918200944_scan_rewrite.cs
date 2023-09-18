using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BSChallenger.Server.Migrations
{
    /// <inheritdoc />
    public partial class scan_rewrite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_scan_history_rankings_ranking_id",
                table: "scan_history");

            migrationBuilder.RenameColumn(
                name: "ranking_id",
                table: "scan_history",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "ix_scan_history_ranking_id",
                table: "scan_history",
                newName: "ix_scan_history_user_id");

            migrationBuilder.AddColumn<int>(
                name: "active_users",
                table: "rankings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "weekly_scans",
                table: "rankings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "required_category_data",
                table: "level",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "score_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    map_id = table.Column<int>(type: "integer", nullable: true),
                    passed = table.Column<bool>(type: "boolean", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    accuracy = table.Column<float>(type: "real", nullable: false),
                    modifiers = table.Column<string>(type: "text", nullable: true),
                    scan_history_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_score_data", x => x.id);
                    table.ForeignKey(
                        name: "fk_score_data_map_map_id",
                        column: x => x.map_id,
                        principalTable: "map",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_score_data_scan_history_scan_history_id",
                        column: x => x.scan_history_id,
                        principalTable: "scan_history",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_level",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ranking_id = table.Column<string>(type: "text", nullable: true),
                    level = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_level", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_level_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_score_data_map_id",
                table: "score_data",
                column: "map_id");

            migrationBuilder.CreateIndex(
                name: "ix_score_data_scan_history_id",
                table: "score_data",
                column: "scan_history_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_level_user_id",
                table: "user_level",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_scan_history_users_user_id",
                table: "scan_history",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_scan_history_users_user_id",
                table: "scan_history");

            migrationBuilder.DropTable(
                name: "score_data");

            migrationBuilder.DropTable(
                name: "user_level");

            migrationBuilder.DropColumn(
                name: "active_users",
                table: "rankings");

            migrationBuilder.DropColumn(
                name: "weekly_scans",
                table: "rankings");

            migrationBuilder.DropColumn(
                name: "required_category_data",
                table: "level");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "scan_history",
                newName: "ranking_id");

            migrationBuilder.RenameIndex(
                name: "ix_scan_history_user_id",
                table: "scan_history",
                newName: "ix_scan_history_ranking_id");

            migrationBuilder.AddForeignKey(
                name: "fk_scan_history_rankings_ranking_id",
                table: "scan_history",
                column: "ranking_id",
                principalTable: "rankings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
