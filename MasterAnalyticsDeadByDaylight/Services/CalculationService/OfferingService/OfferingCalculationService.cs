using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using Microsoft.EntityFrameworkCore;

namespace MasterAnalyticsDeadByDaylight.Services.CalculationService.OfferingService
{
    public class OfferingCalculationService(Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory) : IOfferingCalculationService
    {
        private readonly DataService _dataService = new(contextFactory);

        public async Task<List<OfferingStat>> CalculatingOfferingStat(PlayerAssociation typeAssociation, OfferingCategory offeringCategory)
        {
            List<OfferingStat> offeringStat = [];

            int id_association = typeAssociation.IdPlayerAssociation;
            List<Offering> offerings = await _dataService.GetAllDataInListAsync<Offering>(x => x.Include(x => x.IdRoleNavigation).Where(x => x.IdCategory == offeringCategory.IdCategory));
            List <Survivor> survivors = await _dataService.GetAllDataInListAsync<Survivor>(x => x.Skip(1));
            List<Killer> killers = await _dataService.GetAllDataInListAsync<Killer>(x => x.Skip(1));

            foreach (var offering in offerings.OrderBy(x => x.IdRarity))
            {
                List<OfferingCharacterUse> offeringSurvivorUse = [];
                List<OfferingCharacterUse> offeringKillerUse = [];

                int allAmountSurvivorUsed = 0;
                int allAmountKillerUsed = 0;

                foreach (var survivor in survivors)
                {
                    var survivorInfos = await _dataService.GetAllDataInListAsync<SurvivorInfo>(x => x
                        .Include(x => x.IdSurvivorNavigation)
                            .Where(x => x.IdAssociation == id_association)
                                .Where(x => x.IdSurvivor == survivor.IdSurvivor)
                                    .Where(x => x.IdSurvivorOffering == offering.IdOffering));
                   
                    allAmountSurvivorUsed += survivorInfos.Count;

                    var offeringCharacterUse = new OfferingCharacterUse()
                    {
                        NameCharacter = survivor.SurvivorName,
                        AmountUsedOffering = survivorInfos.Count
                    };

                    offeringSurvivorUse.Add(offeringCharacterUse);
                }

                foreach (var killer in killers)
                {
                    var killerInfos = await _dataService.GetAllDataInListAsync<KillerInfo>(x => x.Include(x => x
                        .IdKillerNavigation)
                            .Where(x => x.IdAssociation == id_association)
                                .Where(x => x.IdKiller == killer.IdKiller)
                                    .Where(x => x.IdKillerOffering == offering.IdOffering));
                   
                    allAmountKillerUsed += killerInfos.Count;

                    var offeringCharacterUse = new OfferingCharacterUse()
                    {
                        NameCharacter = killer.KillerName,
                        AmountUsedOffering = killerInfos.Count
                    };
                    offeringKillerUse.Add(offeringCharacterUse);
                }

                double numberPickKillerOffering = _dataService.Count<KillerInfo>(x => x
                    .Where(x => x.IdAssociation == id_association)
                        .Where(x => x.IdKillerOffering == offering.IdOffering));

                int numberMatchKiller = _dataService.Count<KillerInfo>(x => x
                    .Where(x => x.IdAssociation == id_association));

                double numberPickSurvivorOffering = _dataService.Count<SurvivorInfo>(x => x
                    .Where(x => x.IdAssociation == id_association)
                        .Where(x => x.IdSurvivorOffering == offering.IdOffering));

                int numberMatchSurvivor = _dataService.Count<SurvivorInfo>(x => x
                    .Where(x => x.IdAssociation == id_association));

                double pickRateKillerPercent = Math.Round(numberPickKillerOffering / numberMatchKiller * 100, 2);
                double pickRateSurvivorPercent = Math.Round(numberPickSurvivorOffering / numberMatchSurvivor * 100, 2);

                var offStat = new OfferingStat()
                {
                    OfferingName = offering.OfferingName,
                    OfferingImage = offering.OfferingImage,
                    OfferingID = offering.IdOffering,
                    OfferingRole = offering.IdRoleNavigation.RoleName,
                    AllAmountSurvivorUsed = allAmountSurvivorUsed,
                    AllAmountKillerUsed = allAmountKillerUsed,
                    OfferingSurvivorUses = offeringSurvivorUse.OrderByDescending(x => x.AmountUsedOffering).ToList(),
                    OfferingKillerUses = offeringKillerUse.OrderByDescending(x => x.AmountUsedOffering).ToList(),
                    PickRateSurvivorPercent = pickRateSurvivorPercent,
                    NumberMatchSurvivor = numberMatchSurvivor,
                    NumberPickSurvivorOffering = numberPickSurvivorOffering,
                    PickRateKillerPercent = pickRateKillerPercent,
                    NumberPickKillerOffering = numberPickKillerOffering,
                    NumberMatchKiller = numberMatchKiller
                };
                offeringStat.Add(offStat);
            }

            return offeringStat;
        }
    }
}
