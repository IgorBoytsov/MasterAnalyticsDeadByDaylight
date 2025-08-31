using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Create;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Delete;
using DBDAnalytics.CatalogService.Application.Features.Measurements.GetAll;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/measurements")]
    public sealed class MeasurementController : Controller
    {
        private readonly IMediator _mediator;

        public MeasurementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllMeasurementsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromForm] MeasurementRequest request)
        {
            List<CreateMapCommandData> mapsData = [];

            foreach (var map in request.Maps)
                mapsData.Add(new CreateMapCommandData(map.OldId, map.Name, ControllerExtensions.ToFileInput(map.Image), map.SemanticImageName));

            var command = new CreateMeasurementCommand(request.OldId, request.Name, mapsData);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpPatch("{measurementId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid measurementId, [FromForm] UpdateMeasurementRequest request)
        {
            var command = new UpdateMeasurementCommand(measurementId, request.NewName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{measurementId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid measurementId)
        {
            var command = new DeleteMeasurementCommand(measurementId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}