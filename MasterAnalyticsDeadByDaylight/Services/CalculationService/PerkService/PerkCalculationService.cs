﻿using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using Microsoft.EntityFrameworkCore;

namespace MasterAnalyticsDeadByDaylight.Services.CalculationService.PerkService
{
    public class PerkCalculationService(Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory) : IPerkCalculationService
    {
        IDataService _dataService = new DataService(contextFactory);

        public async Task<List<PerkStat>> CalculatingPerkStatAsync(Role role, PlayerAssociation typeAssociation, string sortingValue)
        {
            return await Task.Run(() => 
            {
                List<PerkStat> perkStats = [];

                List<KillerPerk> killerPerks = [];
                List<SurvivorPerk> survivorPerks = [];

                Action actionPerk = role.IdRole switch
                {
                    2 => () => killerPerks.AddRange(_dataService.GetAllDataInList<KillerPerk>(x => x.Include(x => x.IdCategoryNavigation).Where(x => x.IdCategoryNavigation.CategoryName == sortingValue))),
                    3 => () => survivorPerks.AddRange(_dataService.GetAllDataInList<SurvivorPerk>(x => x.Include(x => x.IdCategoryNavigation).Where(x => x.IdCategoryNavigation.CategoryName == sortingValue))),
                    _ => () => throw new Exception("Такого значение для сортировки нету")
                };

                actionPerk.Invoke();

                Action actionPerkStat = role.IdRole switch
                {
                    2 => () =>
                    {
                        List<KillerInfo> killerInfos = _dataService.GetAllDataInList<KillerInfo>(x => x.Where(x => x.IdAssociation == typeAssociation.IdPlayerAssociation));
                        List<Killer> killers = _dataService.GetAllDataInList<Killer>(x => x.Skip(1));
                        List<PerkCharacterUse> perkCharacterUseList = [];

                        foreach (var perk in killerPerks.OrderBy(x => x.PerkName))
                        {
                            perkCharacterUseList.Clear();
                            int currentPerkID = perk.IdKillerPerk;

                            var amountMatchWithCurrentPerk = killerInfos
                                   .Where(x =>
                                          x.IdPerk1 == currentPerkID ||
                                          x.IdPerk2 == currentPerkID ||
                                          x.IdPerk3 == currentPerkID ||
                                          x.IdPerk4 == currentPerkID).ToList();

                            double pickRate = Math.Round((double)amountMatchWithCurrentPerk.Count / killerInfos.Count * 100, 2);

                            List<GameStatistic> winMatch = [];

                            foreach (var item in amountMatchWithCurrentPerk)
                            {
                                winMatch.AddRange(_dataService.GetAllDataInList<GameStatistic>(x => x.Where(x => x.IdKillerNavigation.IdKillerInfo == item.IdKillerInfo).Where(x => x.CountKills == 3 | x.CountKills == 4)));
                            }

                            double winRate = Math.Round((double)winMatch.Count / amountMatchWithCurrentPerk.Count * 100, 2);

                            foreach (var killer in killers)
                            {
                                var countUsePerkInCurrentKiller = amountMatchWithCurrentPerk.Where(x => x.IdKiller == killer.IdKiller).ToList();

                                PerkCharacterUse perkCharacterUseItem = new PerkCharacterUse()
                                {
                                    NameCharacter = killer.KillerName,
                                    AmountUsedPerk = countUsePerkInCurrentKiller.Count
                                };

                                perkCharacterUseList.Add(perkCharacterUseItem);
                            }

                            PerkStat perkStat = new PerkStat()
                            {
                                PerkName = perk.PerkName,
                                PerkImage = perk.PerkImage,
                                PerkID = perk.IdKillerPerk,
                                AllAmountPerkUsed = amountMatchWithCurrentPerk.Count,
                                PickRatePercent = pickRate,
                                WinRateAVG = 0,
                                WinRatePercent = winRate,
                                PerkCharacterUses = perkCharacterUseList.OrderByDescending(x => x.AmountUsedPerk).ToList(),
                            };
                            perkStats.Add(perkStat);
                        }
                    },

                    3 => () =>
                    {
                        List<SurvivorInfo> survivorInfos = _dataService.GetAllDataInList<SurvivorInfo>(x => x.Where(x => x.IdAssociation == typeAssociation.IdPlayerAssociation));
                        List<Survivor> survivors = _dataService.GetAllDataInList<Survivor>(x => x.Skip(1));
                        List<PerkCharacterUse> perkCharacterUseList = [];

                        foreach (var perk in survivorPerks.OrderBy(x => x.PerkName))
                        {
                            perkCharacterUseList.Clear();
                            int currentPerkID = perk.IdSurvivorPerk;

                            var amountMatchWithCurrentPerk = survivorInfos
                                   .Where(x =>
                                          x.IdPerk1 == currentPerkID ||
                                          x.IdPerk2 == currentPerkID ||
                                          x.IdPerk3 == currentPerkID ||
                                          x.IdPerk4 == currentPerkID).ToList();

                            double pickRate = Math.Round((double)amountMatchWithCurrentPerk.Count / survivorInfos.Count * 100, 2);

                            double winRate = Math.Round((double)amountMatchWithCurrentPerk.Where(x => x.IdTypeDeath == 5).Count() / survivorInfos.Count * 100, 2);

                            foreach (var survivor in survivors)
                            {
                                var countUsePerkInCurrentSurvivor = amountMatchWithCurrentPerk.Where(x => x.IdSurvivor == survivor.IdSurvivor).ToList();

                                PerkCharacterUse perkCharacterUseItem = new PerkCharacterUse()
                                {
                                    NameCharacter = survivor.SurvivorName,
                                    AmountUsedPerk = countUsePerkInCurrentSurvivor.Count
                                };

                                perkCharacterUseList.Add(perkCharacterUseItem);
                            }

                            PerkStat perkStat = new PerkStat()
                            {
                                PerkName = perk.PerkName,
                                PerkImage = perk.PerkImage,
                                PerkID = perk.IdSurvivorPerk,
                                AllAmountPerkUsed = amountMatchWithCurrentPerk.Count,
                                PickRatePercent = pickRate,
                                WinRateAVG = 0,
                                WinRatePercent = winRate,
                                PerkCharacterUses = perkCharacterUseList.OrderByDescending(x => x.AmountUsedPerk).ToList(),
                            };
                            perkStats.Add(perkStat);
                        } 
                    },

                    _ => () =>
                    {
                        throw new Exception("Такая роль не обрабатывается");
                    }
                };

                actionPerkStat.Invoke();

                return perkStats;
            }); 
        }
    }
}