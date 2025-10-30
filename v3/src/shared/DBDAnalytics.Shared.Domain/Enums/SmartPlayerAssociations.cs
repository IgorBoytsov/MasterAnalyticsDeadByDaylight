namespace DBDAnalytics.Shared.Domain.Enums
{
    public sealed class SmartPlayerAssociations
    {
        public int Id { get; }
        public string Name { get; } = null!;

        private SmartPlayerAssociations(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static readonly SmartPlayerAssociations Me = new(1, "Me");

        public static readonly SmartPlayerAssociations Partner = new(2, "Partner");

        public static readonly SmartPlayerAssociations Opponent = new(3, "Opponent");

        public static readonly SmartPlayerAssociations RandomPlayer = new(4, "RandomPlayer");

        public static SmartPlayerAssociations FromId(int id)
        {
            if (id == Me.Id) return Me;
            if (id == Partner.Id) return Partner;
            if (id == Opponent.Id) return Opponent;
            if (id == RandomPlayer.Id) return RandomPlayer;

            throw new KeyNotFoundException($"Ассоциация с Id '{id}' не найдена.");
        }
    }
}