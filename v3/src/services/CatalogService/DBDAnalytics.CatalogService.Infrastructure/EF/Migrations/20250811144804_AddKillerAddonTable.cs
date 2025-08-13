using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddKillerAddonTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KillerAddons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OldId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, collation: "case_insensitive"),
                    ImageKey = table.Column<string>(type: "text", nullable: true),
                    KillerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KillerAddons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KillerAddons_Killers_KillerId",
                        column: x => x.KillerId,
                        principalTable: "Killers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KillerAddons_KillerId",
                table: "KillerAddons",
                column: "KillerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KillerAddons");
        }
    }
}
