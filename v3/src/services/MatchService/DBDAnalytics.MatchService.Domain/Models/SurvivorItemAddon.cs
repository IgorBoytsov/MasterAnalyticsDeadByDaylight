using DBDAnalytics.MatchService.Domain.ValueObjects.SurvivorPerformance;

namespace DBDAnalytics.MatchService.Domain.Models
{
    public sealed class SurvivorItemAddon
    {
        public SurvivorItemId SurvivorItemId { get; private set; }
        public Guid AddonId { get; private set; }

        private SurvivorItemAddon() { }

        private SurvivorItemAddon(SurvivorItemId survivorItemId, Guid addonId)
        {
            SurvivorItemId = survivorItemId;
            AddonId = addonId;
        }

        public static SurvivorItemAddon Create(SurvivorItemId survivorItemId, Guid addonId)
        {
            return new SurvivorItemAddon(survivorItemId, addonId);
        }
    }
}