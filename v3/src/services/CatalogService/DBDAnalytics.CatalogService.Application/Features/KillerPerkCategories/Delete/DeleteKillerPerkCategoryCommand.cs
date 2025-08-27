using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Delete
{
    public sealed record DeleteKillerPerkCategoryCommand(int Id) : IRequest<Result>,
        IHasIntId;
}