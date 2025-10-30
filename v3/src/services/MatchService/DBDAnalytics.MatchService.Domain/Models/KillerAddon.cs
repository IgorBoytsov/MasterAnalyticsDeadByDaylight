using DBDAnalytics.MatchService.Domain.ValueObjects.KillerPerformance;

namespace DBDAnalytics.MatchService.Domain.Models
{
    public sealed class KillerAddon
    {
        public KillerPerformanceId KillerPerformanceId { get; private set; }
        public Guid AddonId { get; private set; }

        private KillerAddon() { }

        public static KillerAddon Create(KillerPerformanceId performanceId, Guid addonId)
        {
            return new KillerAddon { KillerPerformanceId = performanceId, AddonId = addonId };
        }
    }
}