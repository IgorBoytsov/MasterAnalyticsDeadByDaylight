using DBDAnalytics.MatchService.Application.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Configurations.Read
{
    internal sealed class MatchDetailsReadConfiguration : IEntityTypeConfiguration<MatchDetailsView>
    {
        public void Configure(EntityTypeBuilder<MatchDetailsView> builder)
        {
            builder.ToView("v_match_details");
            builder.HasKey(m => m.MatchId);

            builder.OwnsOne(m => m.KillerDetails, ownedBuilder => ownedBuilder.ToJson());

            builder.OwnsMany(m => m.SurvivorDetails, ownedBuilder =>
            {
                ownedBuilder.OwnsOne(survivorDetail => survivorDetail.Item);
                ownedBuilder.ToJson();
            });
        }
    }
}