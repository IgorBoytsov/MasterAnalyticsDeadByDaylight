using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Update
{
    public sealed record UpdatePatchCommand(int Id, int OldId, string Name, DateTime Date) : IRequest<Result>,
        IHasIntId,
        IHasName;
}