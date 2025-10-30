using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBDAnalytics.MatchService.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameModeId = table.Column<int>(type: "integer", nullable: false),
                    GameEventId = table.Column<int>(type: "integer", nullable: false),
                    MapId = table.Column<Guid>(type: "uuid", nullable: false),
                    WhoPlacedMapId = table.Column<int>(type: "integer", nullable: false),
                    WhoPlacedMapWinId = table.Column<int>(type: "integer", nullable: false),
                    PatchId = table.Column<int>(type: "integer", nullable: false),
                    CountKills = table.Column<int>(type: "integer", nullable: false),
                    CountHooks = table.Column<int>(type: "integer", nullable: false),
                    CountRecentGenerators = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KillerPerformances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    KillerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfferingId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerAssociationId = table.Column<int>(type: "integer", nullable: false),
                    PlatformId = table.Column<int>(type: "integer", nullable: false),
                    Prestige = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    NumberBloodDrops = table.Column<int>(type: "integer", nullable: false),
                    IsBot = table.Column<bool>(type: "boolean", nullable: false),
                    IsAnonymousMode = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KillerPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KillerPerformances_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurvivorPerformances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    SurvivorId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfferingId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerAssociationId = table.Column<int>(type: "integer", nullable: false),
                    TypeDeathId = table.Column<int>(type: "integer", nullable: false),
                    PlatformId = table.Column<int>(type: "integer", nullable: false),
                    Prestige = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    NumberBloodDrops = table.Column<int>(type: "integer", nullable: false),
                    IsBot = table.Column<bool>(type: "boolean", nullable: false),
                    IsAnonymousMode = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurvivorPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurvivorPerformances_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KillerAddons",
                columns: table => new
                {
                    KillerPerformanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddonId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KillerAddons", x => new { x.KillerPerformanceId, x.AddonId });
                    table.ForeignKey(
                        name: "FK_KillerAddons_KillerPerformances_KillerPerformanceId",
                        column: x => x.KillerPerformanceId,
                        principalTable: "KillerPerformances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KillerPerks",
                columns: table => new
                {
                    KillerPerformanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    PerkId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KillerPerks", x => new { x.KillerPerformanceId, x.PerkId });
                    table.ForeignKey(
                        name: "FK_KillerPerks_KillerPerformances_KillerPerformanceId",
                        column: x => x.KillerPerformanceId,
                        principalTable: "KillerPerformances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurvivorItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SurvivorPerformanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurvivorItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurvivorItems_SurvivorPerformances_SurvivorPerformanceId",
                        column: x => x.SurvivorPerformanceId,
                        principalTable: "SurvivorPerformances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurvivorPerks",
                columns: table => new
                {
                    SurvivorPerformanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    PerkId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurvivorPerks", x => new { x.SurvivorPerformanceId, x.PerkId });
                    table.ForeignKey(
                        name: "FK_SurvivorPerks_SurvivorPerformances_SurvivorPerformanceId",
                        column: x => x.SurvivorPerformanceId,
                        principalTable: "SurvivorPerformances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurvivorItemAddons",
                columns: table => new
                {
                    SurvivorItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddonId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurvivorItemAddons", x => new { x.SurvivorItemId, x.AddonId });
                    table.ForeignKey(
                        name: "FK_SurvivorItemAddons_SurvivorItems_SurvivorItemId",
                        column: x => x.SurvivorItemId,
                        principalTable: "SurvivorItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KillerPerformances_MatchId",
                table: "KillerPerformances",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_SurvivorItems_SurvivorPerformanceId",
                table: "SurvivorItems",
                column: "SurvivorPerformanceId");

            migrationBuilder.CreateIndex(
                name: "IX_SurvivorPerformances_MatchId",
                table: "SurvivorPerformances",
                column: "MatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KillerAddons");

            migrationBuilder.DropTable(
                name: "KillerPerks");

            migrationBuilder.DropTable(
                name: "SurvivorItemAddons");

            migrationBuilder.DropTable(
                name: "SurvivorPerks");

            migrationBuilder.DropTable(
                name: "KillerPerformances");

            migrationBuilder.DropTable(
                name: "SurvivorItems");

            migrationBuilder.DropTable(
                name: "SurvivorPerformances");

            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}
