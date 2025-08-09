using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using System.Collections.ObjectModel;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    class ComparisonPageViewModel : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        public ComparisonPageViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Update(object value)
        {
            SortingList.Clear();

            ClearCollectionStats();

            if (value is IEnumerable<KillerStat> killerStats)
            {
                CurrentDisplayStat = TypeStats.KillerStat;
                _killerStats.AddRange(killerStats);

                foreach (var item in _killerStats)
                    KillerStats.Add(item);

                foreach (var item in SortingKillerList)
                    SortingList.Add(item);

                SelectedSortItem = SortingList.FirstOrDefault();
            }

            if (value is IEnumerable<SurvivorStat> survivorStats)
            {
                CurrentDisplayStat = TypeStats.SurvivorStat;
                _survivorStats.AddRange(survivorStats);

                foreach (var item in _survivorStats)
                    SurvivorStats.Add(item);

                foreach (var item in SortingSurvivorList)
                    SortingList.Add(item);

                SelectedSortItem = SortingList.FirstOrDefault();
            }

            if (value is IEnumerable<MapStat> mapStats)
            {
                CurrentDisplayStat = TypeStats.MapStat;
                _mapStats.AddRange(mapStats);

                foreach (var item in _mapStats)
                    MapStats.Add(item);

                foreach (var item in SortingMapList)
                    SortingList.Add(item);

                SelectedSortItem = SortingList.FirstOrDefault();
            }

            if (value is IEnumerable<OfferingStat> offeringStats)
            {
                CurrentDisplayStat = TypeStats.OfferingStat;
                _offeringStats.AddRange(offeringStats);

                foreach (var item in _offeringStats)
                    OfferingStats.Add(item);

                foreach (var item in SortingOfferingList)
                    SortingList.Add(item);

                SelectedSortItem = SortingList.FirstOrDefault();
            }

            if (value is IEnumerable<PerkStat> perkStats)
            {
                CurrentDisplayStat = TypeStats.PerkStat;
                _perkStats.AddRange(perkStats);

                foreach (var item in _perkStats)
                    PerkStats.Add(item);

                foreach (var item in SortingPerkList)
                    SortingList.Add(item);

                SelectedSortItem = SortingList.FirstOrDefault();
            }
        }

        private void ClearCollectionStats()
        {
            _killerStats.Clear();
            KillerStats.Clear();

            _survivorStats.Clear();
            SurvivorStats.Clear();

            _mapStats.Clear();
            MapStats.Clear();

            _offeringStats.Clear();
            OfferingStats.Clear();

            _perkStats.Clear();
            PerkStats.Clear();
        }

        /*--Общие Свойства \ Переменные \ Коллекции-------------------------------------------------------*/

        #region Переменная : IndexCounter<int> - Счетчик индексаторов списков

        public int IndexCounter = 1;

        #endregion  
         
        #region Коллекции : Основные

        //Киллеры
        private List<KillerStat> _killerStats { get; set; } = [];

        public ObservableCollection<KillerStat> KillerStats { get; set; } = [];

        //Выжившие
        private List<SurvivorStat> _survivorStats { get; set; } = [];

        public ObservableCollection<SurvivorStat> SurvivorStats { get; set; } = [];

        //Карты
        private List<MapStat> _mapStats { get; set; } = [];

        public ObservableCollection<MapStat> MapStats { get; set; } = [];

        //Подношение
        private List<OfferingStat> _offeringStats { get; set; } = [];

        public ObservableCollection<OfferingStat> OfferingStats { get; set; } = [];

        //Перки
        private List<PerkStat> _perkStats { get; set; } = [];

        public ObservableCollection<PerkStat> PerkStats { get; set; } = [];

        #endregion

        #region Коллекции : Список сортировки

        public ObservableCollection<string> SortingList { get; set; } = [];

        public List<string> SortingKillerList { get; set; } =
        [
            "По умолчанию",
            "Алфавит",
            "Пикрейт",
            "Винрейт",
            "Киллрейт",
            "Количеству сыгранных игр",
            "Количеству выигранных игр",
         ];

        public List<string> SortingSurvivorList { get; set; } =
        [
            "По умолчанию",
            "Алфавит",
            "Пикрейт",
            "Ливеров",
            "Анонимных",
            "Количеству сыгранных игр",
            "Количеству побегов",
         ];

        public List<string> SortingMapList { get; set; } =
        [
            "По умолчанию",
            "Алфавит",
            "Пикрейт",
            "Винрейт",
            "Киллрейт",
            "Количеству сыгранных игр",
            "Количеству игр с подношениями",
            "Количеству игр без подношений",
         ]; 
        
        public List<string> SortingOfferingList { get; set; } =
        [
            "По умолчанию",
            "Алфавит",
         ];

        public List<string> SortingPerkList { get; set; } =
       [
           "По умолчанию",
            "Алфавит",
            "Пикрейт",
         ];

        #endregion

        #region Свойство : CurrentDisplayStat - Хранит информацию о текущей отображаемой статистики

        private TypeStats _currentDisplayStat;
        public TypeStats CurrentDisplayStat
        {
            get => _currentDisplayStat;
            set
            {
                _currentDisplayStat = value;
                OnPropertyChanged();
            }
        }

        #endregion  

        #region Свойство : SelectedSortItem - Хранит значение, по которому определяется сортировка списков

        private string _selectedSortItem;
        public string SelectedSortItem
        {
            get => _selectedSortItem;
            set
            {
                _selectedSortItem = value;
                DetermineTypeStatistics();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Метод : Определяет какой список нужно сортировать

        private void DetermineTypeStatistics()
        {
            Action action = CurrentDisplayStat switch
            {
                TypeStats.KillerStat => SortKillerStatList,
                TypeStats.SurvivorStat => SortSurvivorStatList,
                TypeStats.MapStat => SortMapStatList,
                TypeStats.OfferingStat => SortOfferingStatList,
                TypeStats.PerkStat => SortPerkStatList,
                _ => null
            };
            action.Invoke();
        }

        #endregion

        /*--Сортировка списков----------------------------------------------------------------------------*/

        #region Методы сортировки киллеров : KillerStats 

        private void SortKillerStatList()
        {
            Action action = SelectedSortItem switch
            {
                "По умолчанию"               =>  DefaultSortKiller,
                "Алфавит"                    =>  SortKillerStats_ByKillerName,
                "Пикрейт"                    =>  SortKillerStats_ByKillerPickRate,
                "Винрейт"                    =>  SortKillerStats_ByKillerWinRate,
                "Киллрейт"                   =>  SortKillerStats_ByKillerKillRate,
                "Количеству сыгранных игр"   =>  SortKillerStats_ByKillerCountGame,
                "Количеству выигранных игр"  =>  SortKillerStats_ByKillerCountWinGame,
                _ => null
            };
            action.Invoke();
        }

        private void DefaultSortKiller()
        {
            KillerStats.Clear();
            foreach (var item in _killerStats)
            {
                KillerStats.Add(item);
                item.Index = KillerStats.IndexOf(item) + 1;
            }
        }

        private void SortKillerStats_ByKillerName()
        {
            KillerStats.Clear();
            foreach (var item in _killerStats.OrderBy(ks => ks.KillerName))
            {
                KillerStats.Add(item);
                item.Index = KillerStats.IndexOf(item) + 1;
            }
        }

        private void SortKillerStats_ByKillerPickRate()
        {
            KillerStats.Clear();
            foreach (var item in _killerStats.OrderByDescending(ks => ks.KillerPickRate))
            {
                KillerStats.Add(item);
                item.Index = KillerStats.IndexOf(item) + 1;
            }
        }

        private void SortKillerStats_ByKillerWinRate()
        {
            KillerStats.Clear();
            foreach (var item in _killerStats.OrderByDescending(ks => ks.KillerWinRate))
            {
                KillerStats.Add(item);
                item.Index = KillerStats.IndexOf(item) + 1;
            }
        }

        private void SortKillerStats_ByKillerKillRate()
        {
            KillerStats.Clear();
            foreach (var item in _killerStats.OrderByDescending(ks => ks.KillerKillRate))
            {
                KillerStats.Add(item);
                item.Index = KillerStats.IndexOf(item) + 1;
            }
        }

        private void SortKillerStats_ByKillerCountGame()
        {
            KillerStats.Clear();
            foreach (var item in _killerStats.OrderByDescending(ks => ks.KillerCountGame))
            {
                KillerStats.Add(item);
                item.Index = KillerStats.IndexOf(item) + 1;
            }
        }

        private void SortKillerStats_ByKillerCountWinGame()
        {
            KillerStats.Clear();
            foreach (var item in _killerStats.OrderByDescending(ks => ks.KillerMatchWin))
            {
                KillerStats.Add(item);
                item.Index = KillerStats.IndexOf(item) + 1;
            }
        }

        #endregion

        #region Методы сортировки выживших : SurvivorStats 

        private void SortSurvivorStatList()
        {
            Action action = SelectedSortItem switch
            {
                "По умолчанию"              => DefaultSortSurvivor,
                "Алфавит"                   => SortSurvivorStats_BySurvivorName,
                "Пикрейт"                   => SortSurvivorStats_BySurvivorPickRate,
                "Ливеров"                   => SortSurvivorStats_BySurvivorBotRate,
                "Анонимных"                 => SortSurvivorStats_BySurvivorAnonymousModeRate,
                "Количеству сыгранных игр"  => SortSurvivorStats_BySurvivorCountPlayMatch,
                "Количеству побегов"        => SortSurvivorStats_BySurvivorCountEscapeRate,
                _ => null
            };
            action.Invoke();
        }

        private void DefaultSortSurvivor()
        {
            SurvivorStats.Clear();
            foreach (var item in _survivorStats)
            {
                SurvivorStats.Add(item);
                item.Index = SurvivorStats.IndexOf(item) + 1;
            }
        }

        private void SortSurvivorStats_BySurvivorName()
        {
            SurvivorStats.Clear();
            foreach (var item in _survivorStats.OrderBy(ks => ks.SurvivorName))
            {
                SurvivorStats.Add(item);
                item.Index = SurvivorStats.IndexOf(item) + 1;
            }
        }

        private void SortSurvivorStats_BySurvivorPickRate()
        {
            SurvivorStats.Clear();
            foreach (var item in _survivorStats.OrderByDescending(ks => ks.SurvivorPickRate))
            {
                SurvivorStats.Add(item);
                item.Index = SurvivorStats.IndexOf(item) + 1;
            }
        }

        private void SortSurvivorStats_BySurvivorBotRate()
        {
            SurvivorStats.Clear();
            foreach (var item in _survivorStats.OrderByDescending(ks => ks.SurvivorBotPercentage))
            {
                SurvivorStats.Add(item);
                item.Index = SurvivorStats.IndexOf(item) + 1;
            }
        }

        private void SortSurvivorStats_BySurvivorAnonymousModeRate()
        {
            SurvivorStats.Clear();
            foreach (var item in _survivorStats.OrderByDescending(ks => ks.SurvivorAnonymousModePercentage))
            {
                SurvivorStats.Add(item);
                item.Index = SurvivorStats.IndexOf(item) + 1;
            }
        }

        private void SortSurvivorStats_BySurvivorCountPlayMatch()
        {
            SurvivorStats.Clear();
            foreach (var item in _survivorStats.OrderByDescending(ks => ks.SurvivorCount))
            {
                SurvivorStats.Add(item);
                item.Index = SurvivorStats.IndexOf(item) + 1;
            }
        }

        private void SortSurvivorStats_BySurvivorCountEscapeRate()
        {
            SurvivorStats.Clear();
            foreach (var item in _survivorStats.OrderByDescending(ks => ks.SurvivorEscapePercentage))
            {
                SurvivorStats.Add(item);
                item.Index = SurvivorStats.IndexOf(item) + 1;
            }
        }

        #endregion

        #region Методы cортировка карт : MapStats

        private void SortMapStatList()
        {
            Action action = SelectedSortItem switch
            {
                "По умолчанию" => DefaultSortMap,
                "Алфавит" => SortMapStats_ByMapName,
                "Пикрейт" => SortMapStats_ByMapPickRate,
                "Винрейт" => SortMapStats_ByMapKillerWinRate,
                "Киллрейт" => SortMapStats_ByMapKillRate,
                "Количеству сыгранных игр" => SortMapStats_ByCountMatch,
                "Количеству игр с подношениями" => SortMapStats_ByCountMatchWithOffering,
                "Количеству игр без подношений" => SortMapStats_ByCountMatchNoOffering,
                _ => null
            };
            action.Invoke();
        }

        private void DefaultSortMap()
        {
            MapStats.Clear();
            foreach (var item in _mapStats)
            {
                MapStats.Add(item);
                item.Index = MapStats.IndexOf(item) + 1;
            }
        }

        private void SortMapStats_ByMapName()
        {
            MapStats.Clear();
            foreach (var item in _mapStats.OrderBy(ks => ks.MapName))
            {
                MapStats.Add(item);
                item.Index = MapStats.IndexOf(item) + 1;
            }
        }

        private void SortMapStats_ByMapPickRate()
        {
            MapStats.Clear();
            foreach (var item in _mapStats.OrderByDescending(ks => ks.PickRateMap))
            {
                MapStats.Add(item);
                item.Index = MapStats.IndexOf(item) + 1;
            }
        }

        private void SortMapStats_ByMapKillerWinRate()
        {
            MapStats.Clear();
            foreach (var item in _mapStats.OrderByDescending(ks => ks.WinRateMapPercent))
            {
                MapStats.Add(item);
                item.Index = MapStats.IndexOf(item) + 1;
            }
        }

        private void SortMapStats_ByMapKillRate()
        {
            MapStats.Clear();
            foreach (var item in _mapStats.OrderByDescending(ks => ks.KillRateMapPercent))
            {
                MapStats.Add(item);
                item.Index = MapStats.IndexOf(item) + 1;
            }
        }

        private void SortMapStats_ByCountMatch()
        {
            MapStats.Clear();
            foreach (var item in _mapStats.OrderByDescending(ks => ks.CountGame))
            {
                MapStats.Add(item);
                item.Index = MapStats.IndexOf(item) + 1;
            }
        }

        private void SortMapStats_ByCountMatchWithOffering()
        {
            MapStats.Clear();
            foreach (var item in _mapStats.OrderByDescending(ks => ks.FalloutMapOfferingPercent))
            {
                MapStats.Add(item);
                item.Index = MapStats.IndexOf(item) + 1;
            }
        }

        private void SortMapStats_ByCountMatchNoOffering()
        {
            MapStats.Clear();
            foreach (var item in _mapStats.OrderByDescending(ks => ks.FalloutMapRandomPercent))
            {
                MapStats.Add(item);
                item.Index = MapStats.IndexOf(item) + 1;
            }
        }

        #endregion

        #region Методы : Сортировка подношений : OfferingStats

        private void SortOfferingStatList()
        {
            Action action = SelectedSortItem switch
            {
                "По умолчанию" => DefaultSortOffering,
                "Алфавит" => SortPerkStats_ByOfferingName,
                _ => null
            };
            action.Invoke();
        }

        private void DefaultSortOffering()
        {
            OfferingStats.Clear();
            foreach (var item in _offeringStats)
            {
                OfferingStats.Add(item);
                item.Index = OfferingStats.IndexOf(item) + 1;
            }
        }

        private void SortPerkStats_ByOfferingName()
        {
            OfferingStats.Clear();
            foreach (var item in _offeringStats.OrderBy(ks => ks.OfferingName))
            {
                OfferingStats.Add(item);
                item.Index = OfferingStats.IndexOf(item) + 1;
            }
        }

        #endregion

        #region Методы : Сортировка подношений : PerkStats

        private void SortPerkStatList()
        {
            Action action = SelectedSortItem switch
            {
                "По умолчанию" => DefaultSortPerk,
                "Алфавит" => SortPerkStats_ByPerkName,
                "Пикрейт" => SortPerkStats_ByPickRate,
                _ => null
            };
            action.Invoke();
        }

        private void DefaultSortPerk()
        {
            PerkStats.Clear();
            foreach (var item in _perkStats)
            {
                PerkStats.Add(item);
                item.Index = PerkStats.IndexOf(item) + 1;
            }
        }

        private void SortPerkStats_ByPerkName()
        {
            PerkStats.Clear();
            foreach (var item in _perkStats.OrderBy(ks => ks.PerkName))
            {
                PerkStats.Add(item);
                item.Index = PerkStats.IndexOf(item) + 1;
            }
        }

        private void SortPerkStats_ByPickRate()
        {
            PerkStats.Clear();
            foreach (var item in _perkStats.OrderByDescending(ks => ks.PickRate))
            {
                PerkStats.Add(item);
                item.Index = PerkStats.IndexOf(item) + 1;
            }
        }

        #endregion

    }
}