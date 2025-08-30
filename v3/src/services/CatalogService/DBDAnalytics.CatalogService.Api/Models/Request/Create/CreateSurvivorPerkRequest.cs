namespace DBDAnalytics.CatalogService.Api.Models.Request.Create
{
    public sealed record CreateSurvivorPerkRequest(List<AddPerkToSurvivorRequestData> Perks);

    public sealed record AddPerkToSurvivorRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName);
}