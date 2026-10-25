using DBDAnalytics.Shared.Domain.ValueObjects;
using DBDAnalytics.Shared.Domain.ValueObjects.Match;
using DBDAnalytics.StatsService.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.StatsService.Domain.Models
{
    public sealed class Match : AggregateRoot<MatchId>
    {
        public UserId ReportingUserId { get; private set; }
        public GameMode GameMode { get; private set; } = null!;
        public Map Map { get; private set; } = null!;
        public Patch Patch { get; private set; } = null!;
        public MatchStartedAt StartedAt { get; private set; }
        public MatchDuration Duration { get; private set; }
        public RecentGenerator RecentGenerator { get; private set; }

        private readonly List<Participant> _participants = [];
        public IReadOnlyCollection<Participant> Participants => _participants.AsReadOnly();

        private Match() { }


    }
}