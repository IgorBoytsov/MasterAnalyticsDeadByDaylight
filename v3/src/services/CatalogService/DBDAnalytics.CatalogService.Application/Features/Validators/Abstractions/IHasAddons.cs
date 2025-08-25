namespace DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions
{
    public interface IHasAddons<TAddon>
    {
        public List<TAddon> Addons { get; }
    }
}