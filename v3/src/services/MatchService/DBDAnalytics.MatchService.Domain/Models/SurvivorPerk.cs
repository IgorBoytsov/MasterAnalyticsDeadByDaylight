using DBDAnalytics.MatchService.Domain.ValueObjects.SurvivorPerformance;

namespace DBDAnalytics.MatchService.Domain.Models
{
    public sealed class SurvivorPerk
    {
        public SurvivorPerformanceId SurvivorPerformanceId { get; private set; }
        public Guid PerkId { get; private set; }

        private SurvivorPerk() { }

        private SurvivorPerk(SurvivorPerformanceId survivorPerformanceId, Guid perkId)
        {
            SurvivorPerformanceId = survivorPerformanceId;
            PerkId = perkId;
        }

        public static SurvivorPerk Create(SurvivorPerformanceId survivorPerformanceId, Guid perkId) 
            => new SurvivorPerk(survivorPerformanceId, perkId);
    }
}