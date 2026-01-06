using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NewsManagementService.Migrations
{
    /// <inheritdoc />
    public partial class Create_Application_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MacroNewsCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroNewsCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferencesReplicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferencesReplicas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MacroNewsCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsCategories_MacroNewsCategories_MacroNewsCategoryId",
                        column: x => x.MacroNewsCategoryId,
                        principalTable: "MacroNewsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsCategoryUserPreferencesReplica",
                columns: table => new
                {
                    SubscribedNewsCategoriesId = table.Column<int>(type: "integer", nullable: false),
                    UserPreferencesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsCategoryUserPreferencesReplica", x => new { x.SubscribedNewsCategoriesId, x.UserPreferencesId });
                    table.ForeignKey(
                        name: "FK_NewsCategoryUserPreferencesReplica_NewsCategories_Subscribe~",
                        column: x => x.SubscribedNewsCategoriesId,
                        principalTable: "NewsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsCategoryUserPreferencesReplica_UserPreferencesReplicas_~",
                        column: x => x.UserPreferencesId,
                        principalTable: "UserPreferencesReplicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsSummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    NewsCategoryId = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsSummaries_NewsCategories_NewsCategoryId",
                        column: x => x.NewsCategoryId,
                        principalTable: "NewsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsCategories_MacroNewsCategoryId",
                table: "NewsCategories",
                column: "MacroNewsCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsCategoryUserPreferencesReplica_UserPreferencesId",
                table: "NewsCategoryUserPreferencesReplica",
                column: "UserPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsSummaries_NewsCategoryId",
                table: "NewsSummaries",
                column: "NewsCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsCategoryUserPreferencesReplica");

            migrationBuilder.DropTable(
                name: "NewsSummaries");

            migrationBuilder.DropTable(
                name: "UserPreferencesReplicas");

            migrationBuilder.DropTable(
                name: "NewsCategories");

            migrationBuilder.DropTable(
                name: "MacroNewsCategories");
        }
    }
}
