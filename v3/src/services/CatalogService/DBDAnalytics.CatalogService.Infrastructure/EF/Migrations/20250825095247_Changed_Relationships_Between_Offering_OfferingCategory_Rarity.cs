using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Relationships_Between_Offering_OfferingCategory_Rarity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Offerings_RoleId",
                table: "Offerings",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offerings_Roles_RoleId",
                table: "Offerings",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offerings_Roles_RoleId",
                table: "Offerings");

            migrationBuilder.DropIndex(
                name: "IX_Offerings_RoleId",
                table: "Offerings");
        }
    }
}
