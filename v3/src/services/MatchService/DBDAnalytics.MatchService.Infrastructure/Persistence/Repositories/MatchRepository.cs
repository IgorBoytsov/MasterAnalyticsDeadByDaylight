using DBDAnalytics.MatchService.Application.Abstractions.Repositories;
using DBDAnalytics.MatchService.Domain.Models;
using DBDAnalytics.MatchService.Infrastructure.Persistence.Contexts;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.MatchService.Infrastructure.Persistence.Repositories
{
    internal sealed class MatchRepository(WriteDbContext context) 
        : BaseRepository<Match, WriteDbContext>(context), IMatchRepository
    {
    }
}