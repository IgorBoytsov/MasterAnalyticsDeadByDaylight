using DBDAnalytics.CatalogService.Api.Models.Request;
using DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Create;
using DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Delete;
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

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateOfferingCategoryRequest request)
        {
            var command = new CreateOfferingCategoryCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
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