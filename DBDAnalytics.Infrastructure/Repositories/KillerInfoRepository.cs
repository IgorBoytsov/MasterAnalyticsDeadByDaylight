using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class KillerInfoRepository(Func<DBDContext> context) : IKillerInfoRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        public int Create(int idKiller,
                          int? idPerk1, int? idPerk2, int? idPerk3, int? idPerk4,
                          int idAddon1, int idAddon2,
                          int idAssociation, int idPlatform, int? idKillerOffering,
                          int prestige, bool isBot, bool isAnonymousMode, int killerAccount)
        {
            using (var _context = _contextFactory())
            {
                var killerInfo = new KillerInfo
                {
                    IdKiller = idKiller,
                    IdPerk1 = idPerk1,
                    IdPerk2 = idPerk2,
                    IdPerk3 = idPerk3,
                    IdPerk4 = idPerk4,
                    IdAddon1 = idAddon1,
                    IdAddon2 = idAddon2,
                    IdAssociation = idAssociation,
                    IdPlatform = idPlatform,
                    IdKillerOffering = idKillerOffering,
                    Prestige = prestige,
                    Bot = isBot,
                    AnonymousMode = isAnonymousMode,
                    KillerAccount = killerAccount
                };

                _context.KillerInfos.Add(killerInfo);
                _context.SaveChanges();

                int id = _context.KillerInfos
                   .OrderByDescending(x => x.IdKillerInfo)
                       .Select(x => x.IdKillerInfo)
                           .FirstOrDefault();

                return id;
            }
        }
    }
}