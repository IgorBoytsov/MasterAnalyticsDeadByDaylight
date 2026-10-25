using DBDAnalytics.StatsService.Domain.ValueObjects;

namespace DBDAnalytics.StatsService.Domain.Models
{
    public sealed class SurvivorPerformance
    {
        public bool Escaped { get; }
        public DeathType TypeOfDeath { get; } = null!;
        public Item Item { get; } = null!;
        public IReadOnlyCollection<Addon> ItemAddons { get; } = [];
    }
}