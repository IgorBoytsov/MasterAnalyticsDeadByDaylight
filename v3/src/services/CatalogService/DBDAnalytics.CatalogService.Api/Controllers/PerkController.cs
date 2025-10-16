using DBDAnalytics.CatalogService.Application.Features.Killers.Perks.GetAll;
using DBDAnalytics.CatalogService.Application.Features.Perks.GetAllSurvivor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [Route("api/perks")]
    public sealed class PerkController : Controller
    {
        private readonly IMediator _mediator;

        public PerkController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("killers")]
        public async Task<IActionResult> GetAllKillerPerks(CancellationToken cancellationToken)
        {
            var query = new GetAllKillerPerksQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("survivors")]
        public async Task<IActionResult> GetAllSurvivorPerks(CancellationToken cancellationToken)
        {
            var query = new GetAllSurvivorPerksQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}