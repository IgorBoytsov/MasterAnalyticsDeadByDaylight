using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Delete
{
    public sealed record DeleteTypeDeathCommand(int Id) : IRequest<Result>,
        IHasIntId;
}