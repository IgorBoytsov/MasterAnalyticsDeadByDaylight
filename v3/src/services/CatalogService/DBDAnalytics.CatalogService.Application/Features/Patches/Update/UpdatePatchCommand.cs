using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Update
{
    public sealed record UpdatePatchCommand(int Id, int OldId, string Name, DateTime Date) : IRequest<Result>,
        IHasIntId,
        IHasName;
}