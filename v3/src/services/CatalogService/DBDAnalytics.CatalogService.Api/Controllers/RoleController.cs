using DBDAnalytics.CatalogService.Api.Models.Request;
using DBDAnalytics.CatalogService.Application.Features.GameEvents.Delete;
using DBDAnalytics.CatalogService.Application.Features.Roles.Create;
using DBDAnalytics.CatalogService.Application.Features.Roles.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public sealed class RoleController : Controller
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
        {
            var command = new CreateRoleCommand(request.OldId, request.Name);

            var result = await _mediator.Send(command);

            return result.ToActionResult(onSuccess: () => Ok(result.Value));
        }

        [HttpDelete("{roleId}")]
        //[Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int roleId)
        {
            var command = new DeleteRoleCommand(roleId);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }
    }
}