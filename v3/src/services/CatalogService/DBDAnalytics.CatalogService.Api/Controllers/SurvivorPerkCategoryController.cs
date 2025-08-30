using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Create;
using DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Delete;
using DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/survivor-perk-categories")]
    public sealed class SurvivorPerkCategoryController : Controller
    {
        private readonly IMediator _mediator;

        public SurvivorPerkCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateSurvivorPerkCategoryRequest request)
        {
            var command = new CreateSurvivorPerkCategoryCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpPatch("{survivorPerkCategoryId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int survivorPerkCategoryId, [FromBody] UpdateSurvivorPerkCategoryRequest request)
        {
            var command = new UpdateSurvivorPerkCategoryCommand(survivorPerkCategoryId, request.NewName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{survivorPerkCategoryId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int survivorPerkCategoryId)
        {
            var command = new DeleteSurvivorPerkCategoryCommand(survivorPerkCategoryId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}