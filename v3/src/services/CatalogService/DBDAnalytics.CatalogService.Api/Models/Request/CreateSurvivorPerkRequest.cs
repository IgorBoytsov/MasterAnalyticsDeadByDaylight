namespace DBDAnalytics.CatalogService.Api.Models.Request
{
    public sealed record CreateSurvivorPerkRequest(List<AddPerkToSurvivorRequestData> Perks);

    public sealed record AddPerkToSurvivorRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName);
}