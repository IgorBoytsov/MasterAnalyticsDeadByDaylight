using DBDAnalytics.MatchService.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchListItemAndMatchDetailsViews : Migration
    {
        private readonly string[] _viewsNames =
            [
                "v_match_details",
                "v_match_list_items"
            ];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var viewName in _viewsNames)
                migrationBuilder.CreateView(viewName);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var viewName in _viewsNames)
                migrationBuilder.DropView(viewName);
        }
    }
}
