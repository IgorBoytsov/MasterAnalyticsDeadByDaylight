using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Rarities.Create;
using DBDAnalytics.CatalogService.Application.Features.Rarities.Delete;
using DBDAnalytics.CatalogService.Application.Features.Rarities.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/rarities")]
    public sealed class RarityController : Controller
    {
        private readonly IMediator _mediator;

        public RarityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateRarityRequest request)
        {
            var command = new CreateRarityCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpPatch("{rarityId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int rarityId, [FromBody] UpdateRarityRequest request)
        {
            var command = new UpdateRarityCommand(rarityId, request.NewName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{rarityId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int rarityId)
        {
            var command = new DeleteRarityCommand(rarityId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}