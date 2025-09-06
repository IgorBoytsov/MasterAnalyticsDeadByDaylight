using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Delete
{
    public sealed record DeletePatchCommand(int Id) : IRequest<Result>,
        IHasIntId;
}