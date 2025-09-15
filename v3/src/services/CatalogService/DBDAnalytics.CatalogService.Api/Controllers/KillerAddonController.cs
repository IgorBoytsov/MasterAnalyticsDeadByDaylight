using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Killers.Addons.Create;
using DBDAnalytics.CatalogService.Application.Features.Killers.Addons.Delete;
using DBDAnalytics.CatalogService.Application.Features.Killers.Addons.GetAll;
using DBDAnalytics.CatalogService.Application.Features.Killers.Addons.GetById;
using DBDAnalytics.CatalogService.Application.Features.Killers.Addons.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [Route("api/killers/{killerId}/addons")]
    public sealed class KillerAddonController : Controller
    {
        private readonly IMediator _mediator;

        public KillerAddonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForKiller([FromRoute] Guid killerId, CancellationToken cancellationToken)
        {
            var query = new GetAddonsByKillerIdQuery(killerId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllKillerAddonsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AddAddon([FromRoute] Guid killerId, [FromForm] CreateKillerAddonRequest request)
        {
            List<AddAddonToKillerCommandData> addonsData = [];

            foreach (var addon in request.Addons)
                addonsData.Add(new AddAddonToKillerCommandData(killerId, addon.OldId, addon.Name, ControllerExtensions.ToFileInput(addon.Image), addon.SemanticImageName));

            var command = new CreateKillerAddonCommand(addonsData);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: _ => Ok(result.Value));
        }

        [HttpPatch("{killerAddonId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UpdateAddon([FromRoute] Guid killerId, [FromRoute] Guid killerAddonId, [FromForm] UpdateKillerAddonRequest request)
        {
            var command = new UpdateKillerAddonCommand(killerId, killerAddonId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SemanticName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpDelete("{killerAddonId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeleteAddon([FromRoute] Guid killerId, [FromRoute] Guid killerAddonId)
        {
            var command = new DeleteKillerAddonCommand(killerId, killerAddonId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}