using DBDAnalytics.CatalogService.Api.Models.Request.Create;
using DBDAnalytics.CatalogService.Api.Models.Request.Update;
using DBDAnalytics.CatalogService.Application.Features.Platforms.Create;
using DBDAnalytics.CatalogService.Application.Features.Platforms.Delete;
using DBDAnalytics.CatalogService.Application.Features.Platforms.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/platforms")]
    public sealed class PlatformController : Controller
    {
        private IMediator _mediator;

        public PlatformController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreatePlatformRequest request)
        {
            var command = new CreatePlatformCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpPatch("{platformId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] int platformId, [FromBody] UpdatePlatformRequest request)
        {
            var command = new UpdatePlatformCommand(platformId, request.NewName);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{platformId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int platformId)
        {
            var command = new DeletePlatformCommand(platformId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}