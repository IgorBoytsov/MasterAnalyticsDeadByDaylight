using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Create;
using DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Delete;
using DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Update;
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