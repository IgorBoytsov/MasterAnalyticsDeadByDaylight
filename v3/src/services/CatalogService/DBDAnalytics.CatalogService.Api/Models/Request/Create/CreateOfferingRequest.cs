namespace DBDAnalytics.CatalogService.Api.Models.Request.Create
{
    public sealed record CreateOfferingRequest(int OldId, string Name, string SemanticName, IFormFile? Image, int RoleId, int? RarityId, int? CategoryId);
}