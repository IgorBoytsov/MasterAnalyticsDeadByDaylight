namespace DBDAnalytics.CatalogService.Api.Models.Request.Update
{
    public sealed record UpdateKillerRequest(string Name,
        IFormFile? ImageAbility, string SemanticImageAbilityName,
        IFormFile? ImagePortrait, string SemanticImagePortraitName);
}