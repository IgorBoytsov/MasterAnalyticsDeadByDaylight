using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Create
{
    public sealed record CreateKillerCommand(
        int OldId, string Name, 
        FileInput? KillerImage, string SemanticKillerImageName,
        FileInput? AbilityImage, string SemanticAbilityImageName,
        List<CreateAddonCommandData> Addons,
        List<CreatePerkCommandData> Perks) : IRequest<Result<KillerResponse>>,
        IHasName;

    public sealed record CreateAddonCommandData(int OldId, string Name, FileInput? Image, string SemanticImageName) : 
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;

    public sealed record CreatePerkCommandData(int OldId, string Name, FileInput? Image, string SemanticImageName) :
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}