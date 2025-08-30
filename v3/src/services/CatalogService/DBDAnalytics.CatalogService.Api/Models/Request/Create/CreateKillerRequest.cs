namespace DBDAnalytics.CatalogService.Api.Models.Request.Create
{
    public sealed record CreateKillerRequest(
        int OldId, 
        string Name, 
        IFormFile? KillerImage, string SemanticKillerImageName, 
        IFormFile? AbilityImage, string SemanticAbilityImageName,
        List<CreateKillerAddonRequestData> Addons,
        List<CreateKillerPerkRequestData> Perks);

    public sealed record CreateKillerAddonRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName);

    public sealed record CreateKillerPerkRequestData(int OldId, string Name, IFormFile? Image, string SemanticImageName);
}