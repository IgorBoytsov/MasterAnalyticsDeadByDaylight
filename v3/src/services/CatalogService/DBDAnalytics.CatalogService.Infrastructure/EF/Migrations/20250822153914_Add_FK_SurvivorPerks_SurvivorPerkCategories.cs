using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class Add_FK_SurvivorPerks_SurvivorPerkCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SurvivorPerks_CategoryId",
                table: "SurvivorPerks",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurvivorPerks_SurvivorPerkCategories_CategoryId",
                table: "SurvivorPerks",
                column: "CategoryId",
                principalTable: "SurvivorPerkCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurvivorPerks_SurvivorPerkCategories_CategoryId",
                table: "SurvivorPerks");

            migrationBuilder.DropIndex(
                name: "IX_SurvivorPerks_CategoryId",
                table: "SurvivorPerks");
        }
    }
}
