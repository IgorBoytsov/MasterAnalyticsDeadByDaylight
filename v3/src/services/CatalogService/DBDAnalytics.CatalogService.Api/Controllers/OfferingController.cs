using DBDAnalytics.CatalogService.Api.Models.Request.Assign;
using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Offerings.AssignCategory;
using DBDAnalytics.CatalogService.Application.Features.Offerings.AssignRarity;
using DBDAnalytics.CatalogService.Application.Features.Offerings.Create;
using DBDAnalytics.CatalogService.Application.Features.Offerings.Delete;
using DBDAnalytics.CatalogService.Application.Features.Offerings.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/offerings")]
    public class OfferingController : Controller
    {
        private readonly IMediator _mediator;

        public OfferingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromForm] CreateOfferingRequest request)
        {
            var command = new CreateOfferingCommand(request.OldId, request.Name, ControllerExtensions.ToFileInput(request.Image), request.SemanticName, request.RoleId, request.RarityId, request.CategoryId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpPatch("{offeringId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid offeringId, [FromForm] UpdateOfferingRequest request)
        {
            var command = new UpdateOfferingCommand(offeringId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SemanticName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpPut("{offeringId}/category")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AssignCategory(Guid offeringId, [FromBody] AssignCategoryToOfferingRequest request)
        {
            var command = new AssignCategoryToOfferingCommand(offeringId, request.CategoryId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpPut("{offeringId}/rarity")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AssignRarity(Guid offeringId, [FromBody] AssignRarityToOfferingRequest request)
        {
            var command = new AssignRarityToOfferingCommand(offeringId, request.RarityId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{offeringId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AssignRarity(Guid offeringId)
        {
            var command = new DeleteOfferingCommand(offeringId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}