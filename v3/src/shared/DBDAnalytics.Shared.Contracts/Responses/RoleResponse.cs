namespace DBDAnalytics.Shared.Contracts.Responses
{
    public sealed record RoleResponse(int Id, int OldId, string Name)
    {
        public static readonly RoleResponse Empty = new(0, 0, string.Empty);
    }
}