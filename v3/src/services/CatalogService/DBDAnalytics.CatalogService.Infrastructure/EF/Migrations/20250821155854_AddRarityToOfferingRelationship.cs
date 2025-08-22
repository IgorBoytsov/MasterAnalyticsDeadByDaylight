using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddRarityToOfferingRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Offerings_RarityId",
                table: "Offerings",
                column: "RarityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offerings_Rarities_RarityId",
                table: "Offerings",
                column: "RarityId",
                principalTable: "Rarities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offerings_Rarities_RarityId",
                table: "Offerings");

            migrationBuilder.DropIndex(
                name: "IX_Offerings_RarityId",
                table: "Offerings");
        }
    }
}
