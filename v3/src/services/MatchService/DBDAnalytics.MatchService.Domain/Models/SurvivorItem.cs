using DBDAnalytics.MatchService.Domain.ValueObjects.SurvivorPerformance;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.MatchService.Domain.Models
{
    public sealed class SurvivorItem : Entity<SurvivorItemId>
    {
        public SurvivorPerformanceId SurvivorPerformanceId { get; private set; }
        public Guid ItemId { get; private set; }

        private readonly List<SurvivorItemAddon> _addons = [];
        public IReadOnlyCollection<SurvivorItemAddon> Addons => _addons.AsReadOnly();

        private SurvivorItem() { }

        private SurvivorItem(SurvivorItemId id, SurvivorPerformanceId performanceId, Guid itemId) : base(id)
        {
            SurvivorPerformanceId = performanceId;
            ItemId = itemId;
        }

        public static SurvivorItem Create(SurvivorPerformanceId performanceId, Guid itemId)
        {
            return new SurvivorItem(
                SurvivorItemId.CreateNew(),
                performanceId,
                itemId);
        }

        public void AddAddon(Guid addonId)
        {
            if (_addons.Any(a => a.AddonId == addonId))
                throw new Exception("Нельзя дублировать одинаковые улучшение у одного предмета.");

            var newAddon = SurvivorItemAddon.Create(this.Id, addonId);
            _addons.Add(newAddon);
        }
    }
}