using DBDAnalytics.CatalogService.Api.Models.Request.Assign;
using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Killers.Perks.AssignCategory;
using DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Create;
using DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Delete;
using DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.GetById;
using DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/survivors/{survivorId}/perks")]
    public class SurvivorPerkController : Controller
    {
        private readonly IMediator _mediator;

        public SurvivorPerkController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] Guid survivorId, CancellationToken cancellationToken)
        {
            var query = new GetSurvivorPerksBySurvivorIdQuery(survivorId);
            
            var result = await _mediator.Send(query, cancellationToken);    

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> CreatePerk(Guid survivorId, [FromForm] CreateSurvivorPerkRequest request)
        {
            List<AddSurvivorPerkCommandData> createSurvivorPerks = [];

            foreach (var item in request.Perks)
                createSurvivorPerks.Add(new AddSurvivorPerkCommandData(survivorId, item.OldId, item.Name, ControllerExtensions.ToFileInput(item.Image), item.SemanticImageName));

            var command = new CreateSurvivorPerkCommand(createSurvivorPerks);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpPatch("{addonId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UpdatePerk([FromRoute] Guid survivorId, [FromRoute] Guid addonId, [FromForm] UpdateSurvivorPerkRequest request)
        {
            var command = new UpdateSurvivorPerkCommand(survivorId, addonId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SemanticName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpPut("{perkId}/category")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AssignCategoryToPerk(Guid survivorId, Guid perkId, [FromBody] AssignCategoryToSurvivorPerKRequest request)
        {
            var command = new AssignCategoryToPerkCommand(survivorId, perkId, request.CategoryId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{perkId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeletePerk(Guid survivorId, Guid perkId)
        {
            var command = new DeleteSurvivorPerkCommand(survivorId, perkId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}