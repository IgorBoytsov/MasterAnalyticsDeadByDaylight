using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
              sql: @"
                    CREATE COLLATION case_insensitive (
                      PROVIDER = 'icu', 
                      LOCALE = 'und-u-ks-level2', 
                      DETERMINISTIC = FALSE
                    );",

              suppressTransaction: true);

            migrationBuilder.CreateTable(
                name: "KillerPerkCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OldId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KillerPerkCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Killers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OldId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, collation: "case_insensitive"),
                    KillerImageKey = table.Column<string>(type: "text", nullable: true),
                    AbilityImageKey = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Killers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KillerPerks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OldId = table.Column<int>(type: "integer", nullable: false),
                    KillerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, collation: "case_insensitive"),
                    ImageKey = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KillerPerks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KillerPerks_KillerPerkCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "KillerPerkCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_KillerPerks_Killers_KillerId",
                        column: x => x.KillerId,
                        principalTable: "Killers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KillerPerks_CategoryId",
                table: "KillerPerks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_KillerPerks_KillerId",
                table: "KillerPerks",
                column: "KillerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
               sql: @"DROP COLLATION IF EXISTS case_insensitive;",
               suppressTransaction: true);

            migrationBuilder.DropTable(
                name: "KillerPerks");

            migrationBuilder.DropTable(
                name: "KillerPerkCategories");

            migrationBuilder.DropTable(
                name: "Killers");
        }
    }
}
