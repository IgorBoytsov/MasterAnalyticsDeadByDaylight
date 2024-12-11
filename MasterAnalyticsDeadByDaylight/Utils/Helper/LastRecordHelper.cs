using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using Microsoft.EntityFrameworkCore;

namespace MasterAnalyticsDeadByDaylight.Utils.Helper
{
    public class LastRecordHelper(Func<MasterAnalyticsDeadByDaylightDbContext> contextFactory)
    {
        private readonly Func<MasterAnalyticsDeadByDaylightDbContext> _contextFactory = contextFactory;

        public GameStatistic GameStatisticLastRecord()
        {
            using (var context = _contextFactory())
            {
                return context.GameStatistics

              .Include(map => map.IdMapNavigation.IdMeasurementNavigation)  //Карта
              .Include(placedMap => placedMap.IdWhoPlacedMapNavigation) //Кто поставил карту | Чья карта выпала
              .Include(patch => patch.IdPatchNavigation) //Номер патча
              .Include(gameMode => gameMode.IdGameModeNavigation) //Игровой режим
              .Include(gameEvent => gameEvent.IdGameEventNavigation) //Игровой ивент

              .Include(killerInfo => killerInfo.IdKillerNavigation.IdKillerNavigation) //Личные киллера (Изображение, имя и тд)

              .Include(killerOffering => killerOffering.IdKillerNavigation.IdKillerOfferingNavigation) //Подношение киллера

              .Include(killerInfo => killerInfo.IdKillerNavigation.IdPerk1Navigation) //Первый перк киллера
              .Include(killerInfo => killerInfo.IdKillerNavigation.IdPerk2Navigation) //Второй перк киллера
              .Include(killerInfo => killerInfo.IdKillerNavigation.IdPerk3Navigation)//Третий перк киллера
              .Include(firstPerk => firstPerk.IdKillerNavigation.IdPerk4Navigation) //Четвертый перк киллера

              .Include(killerInfo => killerInfo.IdKillerNavigation.IdAddon1Navigation)
              .Include(killerInfo => killerInfo.IdKillerNavigation.IdAddon2Navigation)

              .Include(killerInfo => killerInfo.IdKillerNavigation.IdAssociationNavigation) //С кем ассоциируется киллер : Я, противник

              .Include(killerInfo => killerInfo.IdKillerNavigation.IdPlatformNavigation) //Платформа с которой играл киллер

              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdSurvivorNavigation) // Первый выживший 
              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdPerk1Navigation) // Первый перк 
              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdPerk2Navigation) // Второй перк  
              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdPerk3Navigation) // Третий перк  
              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdPerk4Navigation) // Четвертый перк  
              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdItemNavigation) // Предмет  
              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdAddon1Navigation) // Аддоны предмета  
              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdAddon2Navigation) // Аддоны предмета  
              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdSurvivorOfferingNavigation) // Подношение выжившего
              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdTypeDeathNavigation) // Тип смерти выжившего ( Как его убили : Крюк, От земли, Мементо и тд )
              .Include(firstSurvivor => firstSurvivor.IdSurvivors1Navigation.IdPlatformNavigation) // Платформа с которой играл выживший

              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdSurvivorNavigation) // Второй выживший 
              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdPerk1Navigation) // Первый перк 
              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdPerk2Navigation) // Второй перк  
              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdPerk3Navigation) // Третий перк  
              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdPerk4Navigation) // Четвертый перк  
              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdItemNavigation) // Предмет  
              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdAddon1Navigation) // Аддоны предмета  
              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdAddon2Navigation) // Аддоны предмета  
              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdSurvivorOfferingNavigation) // Подношение выжившего
              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdTypeDeathNavigation) // Тип смерти выжившего ( Как его убили : Крюк, От земли, Мементо и тд )
              .Include(secondSurvivor => secondSurvivor.IdSurvivors2Navigation.IdPlatformNavigation) // Платформа с которой играл выживший

              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdSurvivorNavigation) // Третий выживший 
              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdPerk1Navigation) // Первый перк 
              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdPerk2Navigation) // Второй перк  
              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdPerk3Navigation) // Третий перк  
              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdPerk4Navigation) // Четвертый перк  
              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdItemNavigation) // Предмет  
              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdAddon1Navigation) // Аддоны предмета  
              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdAddon2Navigation) // Аддоны предмета  
              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdSurvivorOfferingNavigation) // Подношение выжившего
              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdTypeDeathNavigation) // Тип смерти выжившего ( Как его убили : Крюк, От земли, Мементо и тд )
              .Include(thirdSurvivor => thirdSurvivor.IdSurvivors3Navigation.IdPlatformNavigation) // Платформа с которой играл выживший

              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdSurvivorNavigation) // Четвертый выживший 
              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdPerk1Navigation) // Первый перк 
              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdPerk2Navigation) // Второй перк  
              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdPerk3Navigation) // Третий перк  
              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdPerk4Navigation) // Четвертый перк  
              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdItemNavigation) // Предмет  
              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdAddon1Navigation) // Аддоны предмета  
              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdAddon2Navigation) // Аддоны предмета  
              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdSurvivorOfferingNavigation) // Подношение выжившего
              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdTypeDeathNavigation) // Тип смерти выжившего ( Как его убили : Крюк, От земли, Мементо и тд )
              .Include(fourthSurvivor => fourthSurvivor.IdSurvivors4Navigation.IdPlatformNavigation) // Платформа с которой играл выживший

              .OrderByDescending(x => x.DateTimeMatch).FirstOrDefault();
            }
        }

    }
}
