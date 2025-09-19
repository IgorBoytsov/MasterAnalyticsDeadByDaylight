using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Delete
{
    public sealed record DeletePatchCommand(int Id) : IRequest<Result>,
        IHasIntId;
}