using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Killers.Perks.AssignCategory;
using DBDAnalytics.CatalogService.Application.Features.Killers.Perks.Create;
using DBDAnalytics.CatalogService.Application.Features.Killers.Perks.Delete;
using DBDAnalytics.CatalogService.Application.Features.Killers.Perks.GetAll;
using DBDAnalytics.CatalogService.Application.Features.Killers.Perks.GetById;
using DBDAnalytics.CatalogService.Application.Features.Killers.Perks.Update;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Assign;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [Route("api/killers/{killerId}/perks")]
    public sealed class KillerPerkController : Controller
    {
        private readonly IMediator _mediator;

        public KillerPerkController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] Guid killerId, CancellationToken cancellationToken)
        {
            var query = new GetPerksByKillerIdQuery(killerId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllKillerPerksQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AddPerk([FromRoute] Guid killerId, [FromForm] CreateKillerPerkRequest request)
        {
            List<AddPerkToKillerCommandData> perksData = [];

            foreach (var perk in request.Perks)
                perksData.Add(new AddPerkToKillerCommandData(killerId, perk.OldId, perk.Name, ControllerExtensions.ToFileInput(perk.Image), perk.SemanticImageName));

            var command = new CreateKillerPerkCommand(perksData);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: _ => Ok(result.Value));
        }

        [HttpPatch("{killerPerkId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UpdatePerk([FromRoute] Guid killerId, [FromRoute] Guid killerPerkId, [FromForm] UpdateKillerPerkRequest request)
        {
            var command = new UpdateKillerPerkCommand(killerId, killerPerkId, request.NewName, ControllerExtensions.ToFileInput(request.Image), request.SematicName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{killerPerkId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeletePerk([FromRoute] Guid killerId, [FromRoute] Guid killerPerkId)
        {
            var command = new DeleteKillerPerkCommand(killerId, killerPerkId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpPut("{perkId}/category")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> AssignCategoryToPerk(Guid killerId, Guid perkId, [FromBody] AssignCategoryToPerkRequest request)
        {
            var command = new AssignCategoryToPerkCommand(killerId, perkId, request.CategoryId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}