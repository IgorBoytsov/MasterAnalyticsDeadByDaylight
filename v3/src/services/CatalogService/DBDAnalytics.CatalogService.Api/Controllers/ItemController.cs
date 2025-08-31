using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Items.Create;
using DBDAnalytics.CatalogService.Application.Features.Items.Delete;
using DBDAnalytics.CatalogService.Application.Features.Items.GetAll;
using DBDAnalytics.CatalogService.Application.Features.Items.Update;
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

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllItemsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
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
   
        [HttpDelete("{itemId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] Guid itemId)
        {
            var command = new DeleteItemCommand(itemId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}