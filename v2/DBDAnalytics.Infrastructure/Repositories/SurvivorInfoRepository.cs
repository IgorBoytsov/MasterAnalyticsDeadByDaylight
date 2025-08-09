using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using DBDAnalytics.Infrastructure.Models;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class SurvivorInfoRepository(Func<DBDContext> context) : ISurvivorInfoRepository
    {
        private readonly Func<DBDContext> _contextFactory = context;

        public int Create(int idSurvivor,
                          int idFirstPerk, int idSecondPerk, int idThirdPerk, int idFourthPerk,
                          int idItem, int idFirstItemAddon, int idSecondItemAddon,
                          int idTypeDeath, int idAssociation, int idPlatform, int IdOffering,
                          int prestige, bool isBot, bool isAnonymousMode, int survivorAccount)
        {
            using (var _context = _contextFactory())
            {
                var survivorInfo = new SurvivorInfo
                {
                    IdSurvivor = idSurvivor,
                    IdPerk1 = idFirstPerk,
                    IdPerk2 = idSecondPerk,
                    IdPerk3 = idThirdPerk,
                    IdPerk4 = idFourthPerk,
                    IdItem = idItem,
                    IdAddon1 = idFirstItemAddon,
                    IdAddon2 = idSecondItemAddon,
                    IdTypeDeath = idTypeDeath,
                    IdAssociation = idAssociation,
                    IdPlatform = idPlatform,
                    IdSurvivorOffering = IdOffering,
                    Prestige = prestige,
                    Bot = isBot,
                    AnonymousMode = isAnonymousMode,
                    SurvivorAccount = survivorAccount
                };

                _context.SurvivorInfos.Add(survivorInfo);
                _context.SaveChanges();

                int id = _context.SurvivorInfos
                .OrderByDescending(x => x.IdSurvivorInfo)
                    .Select(x => x.IdSurvivorInfo)
                        .FirstOrDefault();

                return id;
            }
        }

        public List<int> GetLastNRecordsId(int countRecords)
        {
            using (var _context = _contextFactory())
            {
                var lastRecordsEntities = _context.SurvivorInfos
                        .OrderByDescending(x => x.IdSurvivorInfo)
                            .Take(countRecords)
                                .Select(x => x.IdSurvivorInfo)
                                    .Reverse();

                return [.. lastRecordsEntities];
            }
        }
    }
}