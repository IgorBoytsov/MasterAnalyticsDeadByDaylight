using DBDAnalytics.CatalogService.Api.Models.Request;
using DBDAnalytics.CatalogService.Application.Features.GameEvents.Delete;
using DBDAnalytics.CatalogService.Application.Features.Rarities.Create;
using DBDAnalytics.CatalogService.Application.Features.Rarities.Delete;
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