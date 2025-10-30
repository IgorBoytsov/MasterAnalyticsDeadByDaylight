namespace DBDAnalytics.MatchService.Application.Abstractions.Models
{
    public class MatchDetailsView
    {
        public Guid MatchId { get; set; }
        public DateTime StartedAt { get; set; }
        public TimeSpan Duration { get; set; }
        public Guid MapId { get; set; }
        public int GameModeId { get; set; }
        public int GameEventId { get; set; }
        public int PatchId { get; set; }

        public KillerDetails KillerDetails { get; set; } = null!;
        public List<SurvivorDetails> SurvivorDetails { get; set; } = [];
    }

    public class KillerDetails
    {
        public long PerformanceId { get; set; }
        public Guid KillerId { get; set; }
        public int PlayerAssociationId { get; set; }
        public int PlatformId { get; set; }
        public int Score { get; set; }
        public int Prestige { get; set; }
        public int Experience { get; set; }
        public int NumberBloodDrops { get; set; }
        public bool IsBot { get; set; }
        public bool IsAnonymousMode { get; set; }
        public List<Guid> PerkIds { get; set; } = new();
        public List<Guid> AddonIds { get; set; } = new();
    }

    public class SurvivorDetails
    {
        public long PerformanceId { get; set; }
        public Guid SurvivorId { get; set; }
        public int PlayerAssociationId { get; set; }
        public int PlatformId { get; set; }
        public int Score { get; set; }
        public int Prestige { get; set; }
        public int Experience { get; set; }
        public int NumberBloodDrops { get; set; }
        public int TypeDeathId { get; set; }
        public bool IsBot { get; set; }
        public bool IsAnonymousMode { get; set; }
        public List<Guid> PerkIds { get; set; } = [];
        public SurvivorItem Item { get; set; } = null!;
    }

    public class SurvivorItem
    {
        public Guid ItemId { get; set; }
        public List<Guid> AddonIds { get; set; } = new();
    }
}