namespace DBDAnalytics.Shared.Contracts.Responses.CharacterInfo
{
    public sealed record PlayerAssociationResponse(int Id, int OldId, string Name)
    {
        public static readonly PlayerAssociationResponse Empty = new(0, 0, string.Empty);
    }
}