using DBDAnalytics.Shared.Contracts.Requests.Shared;

namespace DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions
{
    public interface IMayHasFileInput
    {
        public FileInput? Image { get; }
    }
}