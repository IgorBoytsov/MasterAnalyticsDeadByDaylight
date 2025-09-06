namespace DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update
{
    public sealed record UpdatePatchRequest(int OldId, string NewName, DateTime Date);
}