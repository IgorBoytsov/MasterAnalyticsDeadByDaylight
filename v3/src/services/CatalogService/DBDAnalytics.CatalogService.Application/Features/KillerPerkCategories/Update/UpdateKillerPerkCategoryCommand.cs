using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Update
{
    public sealed record UpdateKillerPerkCategoryCommand(int KillerPerkCategoryId, string Name) : IRequest<Result>,
        IHasName;
}