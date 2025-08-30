using DBDAnalytics.CatalogService.Api.Models.Request.Assign;
using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Items.Update;
using DBDAnalytics.CatalogService.Application.Features.Items.UpdateAddon;
using DBDAnalytics.CatalogService.Application.Features.Killers.AddAddon;
using DBDAnalytics.CatalogService.Application.Features.Killers.AddPerk;
using DBDAnalytics.CatalogService.Application.Features.Killers.AssignCategoryToPerk;
using DBDAnalytics.CatalogService.Application.Features.Killers.Create;
using DBDAnalytics.CatalogService.Application.Features.Killers.Delete;
using DBDAnalytics.CatalogService.Application.Features.Killers.RemoveAddon;
using DBDAnalytics.CatalogService.Application.Features.Killers.RemovePerk;
using DBDAnalytics.CatalogService.Application.Features.Killers.Update;
using DBDAnalytics.CatalogService.Application.Features.Killers.UpdateAddon;
using DBDAnalytics.CatalogService.Application.Features.Killers.UpdatePerk;
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

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromForm] CreateKillerRequest request)
        {
            FileInput? killerImageInput = ControllerExtensions.ToFileInput(request.KillerImage);
            FileInput? abilityImageInput = ControllerExtensions.ToFileInput(request.AbilityImage);

            List<CreateAddonCommandData> addonData = [];
            List<CreatePerkCommandData> perkData = [];

            foreach (var addon in request.Addons)
                addonData.Add(new CreateAddonCommandData(addon.OldId, addon.Name, ControllerExtensions.ToFileInput(addon.Image), addon.SemanticImageName));

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

        [HttpPost("{killerId}/addons")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AddAddon([FromRoute] Guid killerId, [FromForm] CreateKillerAddonRequest request)
        {
            List<AddAddonToKillerCommandData> addonsData = [];

            foreach(var addon in request.Addons)
                addonsData.Add(new AddAddonToKillerCommandData(killerId, addon.OldId, addon.Name, ControllerExtensions.ToFileInput(addon.Image), addon.SemanticImageName));

            var command = new CreateKillerAddonCommand(addonsData);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: _ => Ok(result.Value));
        }

        [HttpPatch("{killerId}/addons/{killerAddonId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UpdateAddon([FromRoute] Guid killerId, [FromRoute] Guid killerAddonId, [FromForm] UpdateKillerAddonRequest request)
        {
            var command = new UpdateKillerAddonCommand(killerId, killerAddonId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SematicName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpPost("{killerId}/perks")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AddPerk([FromRoute] Guid killerId, [FromForm] CreateKillerPerkRequest request)
        {
            List<AddPerkToKillerCommandData> perksData = [];

            foreach (var perk in request.Perks)
                perksData.Add(new AddPerkToKillerCommandData(killerId, perk.OldId, perk.Name, ControllerExtensions.ToFileInput(perk.Image), perk.SemanticImageName));

            var command = new CreateKillerPerkCommand(perksData);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: _ => Ok(result.Value));
        }

        [HttpPatch("{killerId}/perks/{killerPerkId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UpdatePerk([FromRoute] Guid killerId, [FromRoute] Guid killerPerkId, [FromForm] UpdateKillerPerkRequest request)
        {
            var command = new UpdateKillerPerkCommand(killerId, killerPerkId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SematicName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpPut("{killerId}/perks/{perkId}/category")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AssignCategoryToPerk(Guid killerId, Guid perkId, [FromBody] AssignCategoryToPerkRequest request)
        {
            var command = new AssignCategoryToPerkCommand(killerId, perkId, request.CategoryId);

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

        [HttpDelete("{killerId}/addons/{killerAddonId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeleteAddon([FromRoute] Guid killerId, [FromRoute] Guid killerAddonId)
        {
            var command = new DeleteKillerAddonCommand(killerId, killerAddonId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{killerId}/perks/{killerPerkId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeletePerk([FromRoute] Guid killerId, [FromRoute] Guid killerPerkId)
        {
            var command = new DeleteKillerPerkCommand(killerId, killerPerkId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

    }
}