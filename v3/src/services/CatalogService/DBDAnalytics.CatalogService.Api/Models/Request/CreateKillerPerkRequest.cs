namespace DBDAnalytics.CatalogService.Api.Models.Request
{
    //public sealed record CreateKillerPerkRequest(
    //    int OldId,
    //    string Name,
    //    IFormFile? Image,
    //    string SemanticImageName);

    public sealed record CreateKillerPerkRequest(List<AddPerkToKillerRequestData> Perks);

    public sealed record AddPerkToKillerRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName);
}