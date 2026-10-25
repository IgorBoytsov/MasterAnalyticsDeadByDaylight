using DBDAnalytics.Shared.Domain.ValueObjects.Match;
using DBDAnalytics.StatsService.Domain.ValueObjects;

namespace DBDAnalytics.StatsService.Domain.Models
{
    public sealed class KillerPerformance
    {
        public CountKill Kills { get; }
        public CountHook Hooks { get; }
        public IReadOnlyCollection<Addon> Addons { get; } = [];
    }
}