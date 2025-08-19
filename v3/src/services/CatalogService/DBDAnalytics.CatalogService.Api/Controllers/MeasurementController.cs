using DBDAnalytics.CatalogService.Api.Models.Request;
using DBDAnalytics.CatalogService.Application.Features.Measurements.AddMap;
using DBDAnalytics.CatalogService.Application.Features.Measurements.Create;
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
    }
}
