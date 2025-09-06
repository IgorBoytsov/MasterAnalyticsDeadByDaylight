namespace DBDAnalytics.Shared.Contracts.Requests.CatalogService.Create
{
    public sealed record CreatePatchRequest(int OldId, string Name, DateTime Date);
}