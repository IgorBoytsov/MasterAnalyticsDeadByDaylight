using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Survivors.Create;
using DBDAnalytics.CatalogService.Application.Features.Survivors.Delete;
using DBDAnalytics.CatalogService.Application.Features.Survivors.GetAll;
using DBDAnalytics.CatalogService.Application.Features.Survivors.Update;
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

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllSurvivorsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromForm] CreateSurvivorRequest request)
        {
            List<CreateSurvivorPerkCommandData> createSurvivorPerks = [];

            foreach (var item in request.Perks)
                createSurvivorPerks.Add(new CreateSurvivorPerkCommandData(item.OldId, item.Name, ControllerExtensions.ToFileInput(item.Image), item.SemanticImageName, item.CategoryId));

            var command = new CreateSurvivorCommand(request.OldId, request.Name, ControllerExtensions.ToFileInput(request.Image), request.SemanticImageName, createSurvivorPerks);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpPatch("{survivorId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] Guid survivorId, [FromForm] UpdateSurvivorRequest request)
        {
            var command = new UpdateSurvivorCommand(survivorId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SemanticName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpDelete("{survivorId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeleteSurvivor(Guid survivorId)
        {
            var command = new DeleteSurvivorCommand(survivorId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}