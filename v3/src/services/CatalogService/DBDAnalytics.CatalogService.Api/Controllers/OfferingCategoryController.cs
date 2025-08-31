using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Create;
using DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Delete;
using DBDAnalytics.CatalogService.Application.Features.OfferingCategories.GetAll;
using DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/offering-categories")]
    public class OfferingCategoryController : Controller
    {
        private readonly IMediator _mediator;

        public OfferingCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllOfferingCategoriesQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateOfferingCategoryRequest request)
        {
            var command = new CreateOfferingCategoryCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpPatch("{offeringCategoryId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] int offeringCategoryId, [FromBody] UpdateOfferingCategoryRequest request)
        {
            var command = new UpdateOfferingCategoryCommand(offeringCategoryId, request.NewName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{offeringCategoryId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int offeringCategoryId)
        {
            var command = new DeleteOfferingCategoryCommand(offeringCategoryId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}