using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Items.Addons.AssignRarity;
using DBDAnalytics.CatalogService.Application.Features.Items.Addons.Create;
using DBDAnalytics.CatalogService.Application.Features.Items.Addons.Delete;
using DBDAnalytics.CatalogService.Application.Features.Items.Addons.GetById;
using DBDAnalytics.CatalogService.Application.Features.Items.Addons.Update;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Assign;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/items/{itemId}/addons")]
    public sealed class ItemAddonController : Controller
    {
        private readonly IMediator _mediator;

        public ItemAddonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllById([FromRoute] Guid itemId, CancellationToken cancellationToken)
        {
            var query = new GetAddonsByItemIdQuery(itemId); 

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
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

        [HttpPatch("{itemAddonId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UpdateAddon([FromRoute] Guid itemId, [FromRoute] Guid itemAddonId, [FromForm] UpdateItemAddonRequest request)
        {
            var command = new UpdateItemAddonCommand(itemId, itemAddonId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SematicName);
            
            var result = await _mediator.Send(command);
            
            return result.ToActionResult(Ok);
        }

        [HttpPut("{itemAddonId}/rarity")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AssignRarityToAddon([FromRoute] Guid itemId, [FromRoute] Guid itemAddonId, [FromBody] AssignRarityToItemAddonRequest request)
        {
            var command = new AssignRarityToItemAddonCommand(itemId, itemAddonId, request.RarityId);
            
            var result = await _mediator.Send(command);
            
            return result.ToActionResult(Ok);
        }

        [HttpDelete("{itemAddonId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeleteAddon([FromRoute] Guid itemId, [FromRoute] Guid itemAddonId)
        {
            var command = new DeleteItemAddonCommand(itemId, itemAddonId);
           
            var result = await _mediator.Send(command);
           
            return result.ToActionResult(Ok);
        }
    }
}
