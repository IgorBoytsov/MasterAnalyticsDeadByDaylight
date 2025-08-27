using DBDAnalytics.CatalogService.Api.Models.Request;
using DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Create;
using DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/player-associations")]
    public sealed class PlayerAssociationController : Controller
    {
        private IMediator _mediator;

        public PlayerAssociationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreatePlayerAssociationRequest request)
        {
            var command = new CreatePlayerAssociationCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpDelete("{playerAssociationId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int playerAssociationId)
        {
            var command = new DeletePlayerAssociationCommand(playerAssociationId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}