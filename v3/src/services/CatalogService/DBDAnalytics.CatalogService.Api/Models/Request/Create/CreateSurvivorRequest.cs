namespace DBDAnalytics.CatalogService.Api.Models.Request.Create
{
    public sealed record CreateSurvivorRequest(int OldId, string Name, IFormFile? Image, string SemanticImageName, List<CreateSurvivorPerkRequestData> Perks);

    public sealed record CreateSurvivorPerkRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName);
}