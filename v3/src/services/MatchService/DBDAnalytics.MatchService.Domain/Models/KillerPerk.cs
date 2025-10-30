using DBDAnalytics.MatchService.Domain.ValueObjects.KillerPerformance;

namespace DBDAnalytics.MatchService.Domain.Models
{
    public sealed class KillerPerk
    {
        public KillerPerformanceId KillerPerformanceId { get; private set; }
        public Guid PerkId { get; private set; }

        private KillerPerk() { }

        public static KillerPerk Create(KillerPerformanceId performanceId, Guid perkId)
        {
            return new KillerPerk { KillerPerformanceId = performanceId, PerkId = perkId };
        }
    }
}