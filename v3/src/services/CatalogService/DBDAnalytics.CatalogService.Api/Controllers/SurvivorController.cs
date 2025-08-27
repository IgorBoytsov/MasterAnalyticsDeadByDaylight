using DBDAnalytics.CatalogService.Api.Models.Request;
using DBDAnalytics.CatalogService.Application.Features.Survivors.AddPerk;
using DBDAnalytics.CatalogService.Application.Features.Survivors.AssignCategory;
using DBDAnalytics.CatalogService.Application.Features.Survivors.Create;
using DBDAnalytics.CatalogService.Application.Features.Survivors.Delete;
using DBDAnalytics.CatalogService.Application.Features.Survivors.RemovePerk;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/survivors")]
    public sealed class SurvivorController : Controller
    {
        private readonly IMediator _mediator;

        public SurvivorController(IMediator mediator)
        {
            _mediator = mediator;   
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromForm] CreateSurvivorRequest request)
        {
            List<CreateSurvivorPerkCommandData> createSurvivorPerks = [];

            foreach (var item in request.Perks)
                createSurvivorPerks.Add(new CreateSurvivorPerkCommandData(item.OldId, item.Name, ControllerExtensions.ToFileInput(item.Image), item.SemanticImageName));

            var command = new CreateSurvivorCommand(request.OldId, request.Name, ControllerExtensions.ToFileInput(request.Image), request.SemanticImageName, createSurvivorPerks);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpPost("{survivorId}/perks")]
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

        [HttpPut("{survivorId}/perks/{perkId}/category")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AssignCategoryToPerk(Guid survivorId, Guid perkId, [FromBody] AssignCategoryToSurvivorPerKRequest request)
        {
            var command = new AssignCategoryToPerkCommand(survivorId, perkId, request.CategoryId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{survivorId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeletePerk(Guid survivorId)
        {
            var command = new DeleteSurvivorCommand(survivorId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{survivorId}/perks/{perkId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeletePerk(Guid survivorId, Guid perkId)
        {
            var command = new DeleteSurvivorPerkCommand(survivorId, perkId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}