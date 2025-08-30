using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Measurements.AddMap;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Create;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Delete;
using DBDAnalytics.CatalogService.Application.Features.Measurements.RemoveMap;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Update;
using DBDAnalytics.CatalogService.Application.Features.Measurements.UpdateMap;
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

        [HttpPost("{measurementId}/maps")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AddMap([FromRoute] Guid measurementId, [FromForm] CreateMapRequest request)
        {
            List<AddMapToMeasurementCommandData> mapsData = [];

            foreach (var map in request.Maps)
                mapsData.Add(new AddMapToMeasurementCommandData(measurementId, map.OldId, map.Name, ControllerExtensions.ToFileInput(map.Image), map.SemanticImageName));

            var command = new CreateMapCommand(mapsData);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpPatch("{measurementId}/maps/{mapId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UpdateMap([FromRoute] Guid measurementId, [FromRoute] Guid mapId, [FromForm] UpdateMapRequest request)
        {
            var command = new UpdateMapCommand(measurementId, mapId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SemanticName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{measurementId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeleteMeasurement([FromRoute] Guid measurementId)
        {
            var command = new DeleteMeasurementCommand(measurementId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{measurementId}/maps/{mapId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeleteMap([FromRoute] Guid measurementId, [FromRoute] Guid mapId)
        {
            var command = new DeleteMapCommand(measurementId, mapId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}