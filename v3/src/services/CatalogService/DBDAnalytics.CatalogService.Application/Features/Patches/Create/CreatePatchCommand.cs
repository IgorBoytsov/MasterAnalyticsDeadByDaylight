using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Create
{
    public sealed record CreatePatchCommand(int OldId, string Name, DateTime Date) : IRequest<Result<PatchResponse>>,
        IHasName;
}