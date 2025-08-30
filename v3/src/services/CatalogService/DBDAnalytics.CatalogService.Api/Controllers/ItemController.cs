using DBDAnalytics.CatalogService.Api.Models.Request.Assign;
using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.GameEvents.Delete;
using DBDAnalytics.CatalogService.Application.Features.Items.AddAddon;
using DBDAnalytics.CatalogService.Application.Features.Items.AssignRarity;
using DBDAnalytics.CatalogService.Application.Features.Items.Create;
using DBDAnalytics.CatalogService.Application.Features.Items.Delete;
using DBDAnalytics.CatalogService.Application.Features.Items.RemoveAddon;
using DBDAnalytics.CatalogService.Application.Features.Items.Update;
using DBDAnalytics.CatalogService.Application.Features.Items.UpdateAddon;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/items")]
    public sealed class ItemController : Controller
    {
        private IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromForm] CreateItemRequest request)
        {
            List<CreateItemAddonCommandData> createItemAddonsData = [];

            foreach (var item in request.Addons)
                createItemAddonsData.Add(new CreateItemAddonCommandData(item.OldId, item.Name, ControllerExtensions.ToFileInput(item.Image), item.SemanticImageName, item.RarityId));

            var command = new CreateItemCommand(request.OldId, request.Name, ControllerExtensions.ToFileInput(request.Image), request.SemanticImageName, createItemAddonsData);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpPatch("{itemId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid itemId, [FromForm] UpdateItemRequest request)
        {
            var command = new UpdateItemCommand(itemId, request.NewName, ControllerExtensions.ToFileInput(request.NewImage), request.SemanticName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpPost("{itemId}/addons")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AddAddon([FromRoute] Guid itemId, [FromForm] CreateItemAddonRequest request)
        {
            List<AddItemAddonCommandData> createdAddons = [];

            foreach (var item in request.Addons)
                createdAddons.Add(new AddItemAddonCommandData(itemId, item.OldId, item.Name, ControllerExtensions.ToFileInput(item.Image), item.SemanticImageName, item.RarityId));

            var command = new CreateItemAddonCommand(createdAddons);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpPatch("{itemId}/addons/{itemAddonId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UpdateAddon([FromRoute] Guid itemId, [FromRoute] Guid itemAddonId, [FromForm] UpdateItemAddonRequest request)
        {
            var command = new UpdateItemAddonCommand(itemId,itemAddonId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SematicName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpPut("{itemId}/addons/{itemAddonId}/rarity")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AssignRarityToAddon(Guid itemId, Guid itemAddonId, [FromBody] AssignRarityToItemAddonRequest request)
        {
            var command = new AssignRarityToItemAddonCommand(itemId, itemAddonId, request.RarityId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{itemId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid itemId)
        {
            var command = new DeleteItemCommand(itemId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{itemId}/addons/{itemAddonId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid itemId, [FromRoute] Guid itemAddonId)
        {
            var command = new DeleteItemAddonCommand(itemId, itemAddonId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}