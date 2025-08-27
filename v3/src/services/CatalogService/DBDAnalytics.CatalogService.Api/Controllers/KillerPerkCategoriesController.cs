using DBDAnalytics.CatalogService.Api.Models.Request;
using DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Create;
using DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/killer-perk-categories")]
    public class KillerPerkCategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public KillerPerkCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateKillerPerkCategoryRequest request)
        {
            var command = new CreateKillerPerkCategoryCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpDelete("{killerPerkCategoryId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int killerPerkCategoryId)
        {
            var command = new DeleteKillerPerkCategoryCommand(killerPerkCategoryId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}