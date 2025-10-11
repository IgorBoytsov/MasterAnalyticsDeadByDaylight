namespace DBDAnalytics.Shared.Contracts.Responses.CharacterInfo
{
    public sealed record TypeDeathResponse(int Id, int OldId, string Name)
    {
        public static readonly TypeDeathResponse Empty = new(0, 0, string.Empty);
    }
}