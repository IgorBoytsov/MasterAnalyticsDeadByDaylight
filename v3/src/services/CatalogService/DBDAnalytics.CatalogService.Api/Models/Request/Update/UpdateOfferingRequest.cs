namespace DBDAnalytics.CatalogService.Api.Models.Request.Update
{
    public sealed record UpdateOfferingRequest(string NewName, IFormFile? Image, string SemanticName, int RoleId, int? RarityId, int? CategoryId);
}