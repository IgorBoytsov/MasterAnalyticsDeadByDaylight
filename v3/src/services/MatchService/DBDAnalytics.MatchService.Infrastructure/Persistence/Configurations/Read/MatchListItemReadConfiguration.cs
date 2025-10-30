using DBDAnalytics.MatchService.Application.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Read
{
    internal sealed class MatchListItemReadConfiguration : IEntityTypeConfiguration<MatchListItemView>
    {
        public void Configure(EntityTypeBuilder<MatchListItemView> builder)
        {
            builder.ToView("v_match_list_items");
            //builder.HasKey(m => m.MatchId);
            builder.HasNoKey();
        }
    }
}