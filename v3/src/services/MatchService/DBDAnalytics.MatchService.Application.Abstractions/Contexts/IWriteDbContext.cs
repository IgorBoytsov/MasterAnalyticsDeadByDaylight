using DBDAnalytics.MatchService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.MatchService.Application.Abstractions.Contexts
{
    public interface IWriteDbContext : IBaseWriteDbContext
    {
        DbSet<Match> Matches { get; set; }
    }
}