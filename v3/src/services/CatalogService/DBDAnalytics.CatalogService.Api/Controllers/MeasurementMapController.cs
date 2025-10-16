using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Create;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Delete;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.GetById;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/measurements/{measurementId}/maps")]
    public class MeasurementMapController : Controller
    {
        private readonly IMediator _mediator;

        public MeasurementMapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] Guid measurementId, CancellationToken cancellationToken)
        {
            var query = new GetMapsByMeasurementIdQuery(measurementId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Add([FromRoute] Guid measurementId, [FromForm] CreateMapRequest request)
        {
            List<AddMapToMeasurementCommandData> mapsData = [];

            foreach (var map in request.Maps)
                mapsData.Add(new AddMapToMeasurementCommandData(measurementId, map.OldId, map.Name, ControllerExtensions.ToFileInput(map.Image), map.SemanticImageName));

            var command = new CreateMapCommand(mapsData);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpPatch("{mapId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid measurementId, [FromRoute] Guid mapId, [FromForm] UpdateMapRequest request)
        {
            var command = new UpdateMapCommand(measurementId, mapId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SemanticName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpDelete("{mapId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid measurementId, [FromRoute] Guid mapId)
        {
            var command = new DeleteMapCommand(measurementId, mapId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}