using DBDAnalytics.MatchService.Application.Features.Matches.Create;
using DBDAnalytics.MatchService.Application.Features.Matches.GetAllForKillerSide;
using DBDAnalytics.MatchService.Application.Features.Matches.GetAllForSurvivorSide;
using DBDAnalytics.MatchService.Application.Features.Matches.GetDetailsMatch;
using DBDAnalytics.Shared.Contracts.Requests.MatchService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;

namespace DBDAnalytics.MatchService.Api.Controllers
{
    [ApiController]
    [Route("api/matches")]
    public class MatchController : Controller
    {
        private readonly IMediator _mediator;

        public MatchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMatchesRequest request)
        {
            var killers = request.Killers.Select(k => new CreateMatchKillerData(
                Guid.Parse(k.KillerId), Guid.Parse(k.OfferingId),
                k.AssociationId, k.PlatformId,
                k.Prestige, k.Score, k.Experience, k.NumberBloodDrops,
                k.IsBot, k.IsAnonymousMode,
                [.. k.Perks.Select(p => Guid.Parse(p))], 
                [.. k.Addons.Select(a => Guid.Parse(a))])).ToList();

            var survivors = request.Survivors.Select(s => new CreateMatchSurvivorData(
                Guid.Parse(s.SurvivorId), Guid.Parse(s.OfferingId),
                s.AssociationId, s.PlatformId, s.TypeDeathId,
                s.Prestige, s.Score, s.Experience, s.NumberBloodDrops,
                s.IsBot, s.IsAnonymousMode,
                [.. s.Perks.Select(p => Guid.Parse(p))],
                [.. s.Items.Select(i => new CreateMatchItemData(Guid.Parse(i.ItemId), 
                [.. i.AddonIds.Select(ia => Guid.Parse(ia))]
                ))])).ToList();

            var command = new CreateMatchCommand(
                Guid.Parse(request.UserId),
                request.GameModeId, request.GameEventId,
                Guid.Parse(request.MapId), request.WhoPlacedMapId, request.WhoPlacedMapWinId,
                request.PatchId,
                request.CountKills, request.CountHooks, request.CountRecentGenerators,
                DateTime.Parse(request.StartedAt), TimeSpan.Parse(request.Duration),
                killers,
                survivors);

            var result = await _mediator.Send(command);

            return result.ToActionResult(Ok);
        }

        [HttpGet("killer-side")]
        public async Task<IActionResult> GetAllKillerSide([FromBody] GetAllMatchesKillerSideRequest request)
        {
            var query = new GetAllMatchesForKillerSideQuery(request.UserId);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("survivor-side")]
        public async Task<IActionResult> GetAllSurvivorSide([FromBody] GetAllMatchesSurvivorSideRequest request)
        {
            var query = new GetAllMatchesForSurvivorSideQuery(request.UserId);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{matchId}/details")]
        public async Task<IActionResult> GetDetailsMatch([FromRoute] Guid matchId)
        {
            var query = new GetDetailsMatchQuery(matchId);

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}