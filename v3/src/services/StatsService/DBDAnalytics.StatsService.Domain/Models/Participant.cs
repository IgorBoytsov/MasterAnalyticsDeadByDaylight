using DBDAnalytics.Shared.Domain.ValueObjects;
using DBDAnalytics.Shared.Domain.ValueObjects.Match.Shared;
using DBDAnalytics.StatsService.Domain.Enums;
using DBDAnalytics.StatsService.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.StatsService.Domain.Models
{
    public sealed class Participant : Entity<ParticipantId>
    {
        public UserId UserId { get; private set; }
        public PlayerRole Role { get; private set; }
        public Character Character { get; private set; } = null!;
        public Score Score { get; private set; }
        public Prestige Prestige { get; private set; }
        public bool IsBot { get; private set; }

        public IReadOnlyCollection<Perk> Perks { get; private set; } = [];
        public Offering Offering { get; private set; } = null!;

        public KillerPerformance? KillerPerformance { get; private set; }
        public SurvivorPerformance? SurvivorPerformance { get; private set; }

        private Participant() { }
    }
}