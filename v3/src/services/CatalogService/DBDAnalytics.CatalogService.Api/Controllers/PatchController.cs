using DBDAnalytics.CatalogService.Application.Features.Patches.Create;
using DBDAnalytics.CatalogService.Application.Features.Patches.Delete;
using DBDAnalytics.CatalogService.Application.Features.Patches.GetAll;
using DBDAnalytics.CatalogService.Application.Features.Patches.Update;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Create;
using DBDAnalytics.Shared.Contracts.Requests.CatalogService.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/patches")]
    public sealed class PatchController : Controller
    {
        private IMediator _mediator;
        
        public PatchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllPatchesQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreatePatchRequest request)
        {
            var command = new CreatePatchCommand(request.OldId, request.Name, request.Date);

            var result = await _mediator.Send(command);

            return result.ToActionResult(() => Ok(result.Value));
        }

        [HttpPatch("{patchId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Update([FromRoute] int patchId, [FromBody] UpdatePatchRequest request)
        {
            var command = new UpdatePatchCommand(patchId, request.OldId, request.NewName, request.Date);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpDelete("{patchId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int patchId)
        {
            var command = new DeletePatchCommand(patchId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}