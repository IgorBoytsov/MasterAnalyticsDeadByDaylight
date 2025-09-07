using DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Create;
using DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Delete;
using DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.GetAll;
using DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Update;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Create;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/who-placed-map")]
    public sealed class WhoPlacedMapController : Controller
    {
        private readonly IMediator _mediator;

        public WhoPlacedMapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllWhoPlacedMapsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateWhoPlacedMapRequest request)
        {
            var command = new CreateWhoPlacedMapCommand(request.OldId, request.Name); 

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpPatch("{whoPlacedMapId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] int whoPlacedMapId, [FromBody] UpdateWhoPlacedMapRequest request)
        {
            var command = new UpdateWhoPlacedMapCommand(whoPlacedMapId, request.NewName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{whoPlacedMapId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int whoPlacedMapId)
        {
            var command = new DeleteWhoPlacedMapCommand(whoPlacedMapId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}