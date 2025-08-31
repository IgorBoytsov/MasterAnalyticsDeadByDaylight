using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.GameEvents.Delete;
using DBDAnalytics.CatalogService.Application.Features.GameEvents.GetAll;
using DBDAnalytics.CatalogService.Application.Features.GameEvents.Update;
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

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllGameEventsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateGameEventRequest request)
        {
            var command = new CreateGameEventCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpPatch("{gameEventID}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] int gameEventID, [FromBody] UpdateGameEventRequest request)
        {
            var command = new UpdateGameEventCommand(gameEventID, request.NewName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
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