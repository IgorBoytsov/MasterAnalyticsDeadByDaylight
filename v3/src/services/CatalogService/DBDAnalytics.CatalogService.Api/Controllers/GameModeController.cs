using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.GameEvents.Create;
using DBDAnalytics.CatalogService.Application.Features.GameModes.Delete;
using DBDAnalytics.CatalogService.Application.Features.GameModes.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/game-modes")]
    public sealed class GameModeController : Controller
    {
        private IMediator _mediator;
        public GameModeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateGameModeRequest request)
        {
            var command = new CreateGameModeCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpPatch("{gameModeId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] int gameModeId, [FromBody] UpdateGameModeRequest request)
        {
            var command = new UpdateGameModeCommand(gameModeId, request.NewName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{gameModeId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int gameModeId)
        {
            var command = new DeleteGameModeCommand(gameModeId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}