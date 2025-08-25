namespace DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions
{
    public interface IHasPerks<TPerk>
    {
        public List<TPerk> Perks { get; }
    }
}