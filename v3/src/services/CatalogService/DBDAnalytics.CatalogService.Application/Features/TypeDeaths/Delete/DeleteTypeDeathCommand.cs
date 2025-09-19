using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Delete
{
    public sealed record DeleteTypeDeathCommand(int Id) : IRequest<Result>,
        IHasIntId;
}