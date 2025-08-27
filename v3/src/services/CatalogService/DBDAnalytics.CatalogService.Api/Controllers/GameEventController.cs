using DBDAnalytics.CatalogService.Api.Models.Request;
using DBDAnalytics.CatalogService.Application.Features.GameEvents.Delete;
using DBDAnalytics.CatalogService.Application.Features.GameModes.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/game-events")]
    public sealed class GameEventController : Controller
    {
        private IMediator _mediator;
        public GameEventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] GameEventRequest request)
        {
            var command = new CreateGameEventCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpDelete("{gameEventId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int gameEventId)
        {
            var command = new DeleteGameEventCommand(gameEventId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}