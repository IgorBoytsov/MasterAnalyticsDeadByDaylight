using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Create;
using DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Delete;
using DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.GetAll;
using DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Update;
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

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllKillerPerkCategoriesQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateKillerPerkCategoryRequest request)
        {
            var command = new CreateKillerPerkCategoryCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpPatch("{killerPerkCategoryId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] int killerPerkCategoryId, [FromBody] UpdateKillerPerkCategoryRequest request)
        {
            var command = new UpdateKillerPerkCategoryCommand(killerPerkCategoryId, request.NewName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
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