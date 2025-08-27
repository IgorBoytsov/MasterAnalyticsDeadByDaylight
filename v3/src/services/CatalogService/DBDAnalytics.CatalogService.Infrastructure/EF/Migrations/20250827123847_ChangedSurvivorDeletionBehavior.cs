using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSurvivorDeletionBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurvivorPerks_Survivors_SurvivorId",
                table: "SurvivorPerks");

            migrationBuilder.AddForeignKey(
                name: "FK_SurvivorPerks_Survivors_SurvivorId",
                table: "SurvivorPerks",
                column: "SurvivorId",
                principalTable: "Survivors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurvivorPerks_Survivors_SurvivorId",
                table: "SurvivorPerks");

            migrationBuilder.AddForeignKey(
                name: "FK_SurvivorPerks_Survivors_SurvivorId",
                table: "SurvivorPerks",
                column: "SurvivorId",
                principalTable: "Survivors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
