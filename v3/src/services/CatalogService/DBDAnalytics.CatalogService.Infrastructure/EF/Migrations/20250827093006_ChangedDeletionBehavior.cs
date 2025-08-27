using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBDAnalytics.CatalogService.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDeletionBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemAddons_Item_ItemId",
                table: "ItemAddons");

            migrationBuilder.DropForeignKey(
                name: "FK_KillerAddons_Killers_KillerId",
                table: "KillerAddons");

            migrationBuilder.DropForeignKey(
                name: "FK_KillerPerks_Killers_KillerId",
                table: "KillerPerks");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_Measurements_MeasurementId",
                table: "Maps");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemAddons_Item_ItemId",
                table: "ItemAddons",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KillerAddons_Killers_KillerId",
                table: "KillerAddons",
                column: "KillerId",
                principalTable: "Killers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KillerPerks_Killers_KillerId",
                table: "KillerPerks",
                column: "KillerId",
                principalTable: "Killers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Measurements_MeasurementId",
                table: "Maps",
                column: "MeasurementId",
                principalTable: "Measurements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemAddons_Item_ItemId",
                table: "ItemAddons");

            migrationBuilder.DropForeignKey(
                name: "FK_KillerAddons_Killers_KillerId",
                table: "KillerAddons");

            migrationBuilder.DropForeignKey(
                name: "FK_KillerPerks_Killers_KillerId",
                table: "KillerPerks");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_Measurements_MeasurementId",
                table: "Maps");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemAddons_Item_ItemId",
                table: "ItemAddons",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KillerAddons_Killers_KillerId",
                table: "KillerAddons",
                column: "KillerId",
                principalTable: "Killers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KillerPerks_Killers_KillerId",
                table: "KillerPerks",
                column: "KillerId",
                principalTable: "Killers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Measurements_MeasurementId",
                table: "Maps",
                column: "MeasurementId",
                principalTable: "Measurements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
