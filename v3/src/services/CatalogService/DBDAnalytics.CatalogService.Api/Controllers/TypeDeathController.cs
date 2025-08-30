using DBDAnalytics.CatalogService.Api.Models.Request;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Create;
using DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Delete;
using DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/type-deaths")]
    public sealed class TypeDeathController : Controller
    {
        private IMediator _mediator;

        public TypeDeathController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateTypeDeathRequest request)
        {
            var command = new CreateTypeDeathCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpPatch("{typeDeathId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] int typeDeathId, [FromBody] UpdateTypeDeathRequest request)
        {
            var command = new UpdateTypeDeathCommand(typeDeathId, request.NewName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{typeDeathId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int typeDeathId)
        {
            var command = new DeleteTypeDeathCommand(typeDeathId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}