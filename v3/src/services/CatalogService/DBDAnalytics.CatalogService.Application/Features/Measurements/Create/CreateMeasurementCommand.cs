using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Create
{
    public sealed record CreateMeasurementCommand(int OldId, string Name, List<CreateMapCommandData> Maps) : IRequest<Result<MeasurementResponse>>,
        IHasName;
    
    public sealed record CreateMapCommandData(int OldId, string Name, FileInput? Image, string SemanticImageName) : 
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}