using DBDAnalytics.CatalogService.Application.Features.Maps.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/maps")]
    public class MapController : Controller
    {
        private readonly IMediator _mediator;

        public MapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllMapsQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}