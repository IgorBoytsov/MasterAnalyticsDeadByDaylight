using DBDAnalytics.MatchService.Application.Abstractions.Common;
using DBDAnalytics.MatchService.Application.Abstractions.Repositories;
using DBDAnalytics.MatchService.Domain.Models;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.MatchService.Application.Features.Matches.Create
{
    public sealed class CreateMatchCommandHandler(
        IMatchRepository matchRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateMatchCommand, Result>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var match = Match.Create(
                    request.UserId,
                    request.GameModeId, request.GameEventId,
                    request.MapId, request.WhoPlacedMapId, request.WhoPlacedMapWinId,
                    request.PatchId,
                    request.CountKills, request.CountHooks, request.CountRecentGenerators,
                    request.StartedAt, request.Duration);

                foreach (var killer in request.Killers)
                {
                    var killerPerformance = KillerPerformance.Create(
                        match.Id, killer.KillerId,
                        killer.OfferingId,
                        killer.AssociationId, killer.PlatformId,
                        killer.Prestige, killer.Score, killer.Experience, killer.NumberBloodDrops,
                        killer.IsBot, killer.IsAnonymousMode);

                    foreach (var perkId in killer.Perks)
                        killerPerformance.AddPerk(perkId);

                    foreach (var addonId in killer.Addons)
                        killerPerformance.AddAddon(addonId);

                    match.AddKillerPerformance(killerPerformance);
                }

                foreach (var survivor in request.Survivors)
                {
                    var survivorPerformance = SurvivorPerformance.Create(
                        match.Id, survivor.SurvivorId,
                        survivor.OfferingId,
                        survivor.AssociationId, survivor.TypeDeathId, survivor.PlatformId,
                        survivor.Prestige, survivor.Score, survivor.Experience, survivor.NumberBloodDrops,
                        survivor.IsBot, survivor.IsAnonymousMode);

                    foreach (var perkId in survivor.Perks)
                        survivorPerformance.AddPerk(perkId);

                    foreach (var item in survivor.Items)
                        survivorPerformance.AddItem(item.ItemId, item.AddonIds);

                    match.AddSurvivorPerformance(survivorPerformance);
                }

                await _matchRepository.AddAsync(match, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Save, $"Ошибка на стороне сервера - {ex.Message}"));
            }

        }
    }
}