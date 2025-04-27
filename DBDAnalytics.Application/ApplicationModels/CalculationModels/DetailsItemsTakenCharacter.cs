namespace DBDAnalytics.Application.ApplicationModels.CalculationModels
{
    public class DetailsItemsTakenCharacter
    {
        public int CharacterId { get; set; }

        public byte[]? CharacterImage { get; set; }

        public string CharacterName { get; set; } = null!;

        public int CountPick { get; set; }

        public double PickRate { get; set; }
    }
}