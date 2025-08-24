using DBDAnalytics.CatalogService.Api.Models.Request;
using DBDAnalytics.CatalogService.Application.Features.Items.AddAddon;
using DBDAnalytics.CatalogService.Application.Features.Items.AssignRarity;
using DBDAnalytics.CatalogService.Application.Features.Items.Create;
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

        [HttpPut("{itemId}/addons/{itemAddonId}/rarity")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AssignRarityToAddon(Guid itemId, Guid itemAddonId, [FromBody] AssignRarityToItemAddonRequest request)
        {
            var command = new AssignRarityToItemAddonCommand(itemId, itemAddonId, request.RarityId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}