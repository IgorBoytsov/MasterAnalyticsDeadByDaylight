using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Create
{
    public sealed record CreatePatchCommand(int OldId, string Name, DateTime Date) : IRequest<Result<PatchResponse>>,
        IHasName;
}