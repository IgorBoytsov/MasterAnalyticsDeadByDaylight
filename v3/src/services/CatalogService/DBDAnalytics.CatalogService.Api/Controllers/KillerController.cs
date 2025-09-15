using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Killers.Create;
using DBDAnalytics.CatalogService.Application.Features.Killers.Delete;
using DBDAnalytics.CatalogService.Application.Features.Killers.GetAll;
using DBDAnalytics.CatalogService.Application.Features.Killers.Update;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [Controller]
    [Route("api/killers")]
    public class KillerController : Controller
    {
        private readonly IMediator _mediator;

        public KillerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllKillersQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromForm] CreateKillerRequest request)
        {
            FileInput? killerImageInput = ControllerExtensions.ToFileInput(request.KillerImage);
            FileInput? abilityImageInput = ControllerExtensions.ToFileInput(request.AbilityImage);

            List<CreateAddonCommandData> addonData = [];
            List<CreatePerkCommandData> perkData = [];

            if (request.Addons is not null)
                foreach (var addon in request.Addons)
                    addonData.Add(new CreateAddonCommandData(addon.OldId, addon.Name, ControllerExtensions.ToFileInput(addon.Image), addon.SemanticImageName));

            if(request.Perks is not null)
                foreach (var perk in request.Perks)
                    perkData.Add(new CreatePerkCommandData(perk.OldId, perk.Name, ControllerExtensions.ToFileInput(perk.Image), perk.SemanticImageName));

            var command = new CreateKillerCommand(
                request.OldId,
                request.Name,
                killerImageInput,
                request.SemanticKillerImageName,
                abilityImageInput,
                request.SemanticAbilityImageName,
                addonData,
                perkData);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: _ => Ok(result.Value));
        }

        [HttpPatch("{killerId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid killerId, [FromForm] UpdateKillerRequest request)
        {
            var command = new UpdateKillerCommand(killerId, request.Name,
                ControllerExtensions.ToFileInput(request.ImageAbility), request.SemanticImageAbilityName, 
                ControllerExtensions.ToFileInput(request.ImagePortrait), request.SemanticImagePortraitName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{killerId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeleteKiller([FromRoute] Guid killerId)
        {
            var command = new DeleteKillerCommand(killerId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}