using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Utils.Calculation;
using MasterAnalyticsDeadByDaylight.Utils.Managers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;

/*Киллер*/

// Киллрейт
// Винрейт
// Проведенное в этих матчах
// Самый короткий, длинный матч
// Популярность аддонов
// Популярность перков
// Популярность подношений
// От чего умирали выжившие 
// Сколько генераторов осталось
// Количество повесов

/*Карты*/

// Количество сыгранных матчей по картам \ измерением
// 

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    class DetailedMatchStatisticsWindowViewModel : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDataService _dataService;

        private readonly DataManager _dataManager;

        public DetailedMatchStatisticsWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var dataService = _serviceProvider.GetService<IDataService>();
            var dataManager = _serviceProvider.GetRequiredService<DataManager>();

            _dataService = dataService;
            _dataManager = dataManager;
        }

        public void Update(object value)
        {
            if (value is IEnumerable<GameStatistic> Matches)
            {
                _matches = Matches.ToList();
                Title = $"Количество матчей - {_matches.Count}";
                IdentifyKillers(_matches);
            }
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/
       
        #region Колекция : Список передоваемых матчей

        private List<GameStatistic> _matches;

        public ObservableCollection<KillerCollection> KillerList { get; set; } = [];

        #endregion

        #region Коллекции : Подрабная статистика

        public ObservableCollection<PrestigeTracker> PrestigeKillerTrackers { get; set; } = [];

        public ObservableCollection<PrestigeTracker> PrestigeSurvivorTrackers { get; set; } = [];

        public ObservableCollection<FrequencyUsingPerkTracker> FrequencyKillerUsingPerkTracker { get; set; } = [];

        public ObservableCollection<FrequencyUsingPerkTracker> FrequencySurvivorUsingPerkTracker { get; set; } = [];

        #endregion

        #region Свойство : Заголовок

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойство : Выбор киллера, по которому будет произведен расчет статистики

        private KillerCollection _selectedKiller;
        public KillerCollection SelectedKiller
        {
            get => _selectedKiller;
            set
            {
                _selectedKiller = value;
                OnPropertyChanged();

                SurvivorPrestigeStats();
                KillerPrestigeStats();

                KillerFrequencyUsingPerkStats();
                SurvivorFrequencyUsingPerkStats();
            }
        }

        #endregion

        #region Свойство : Считать по выбранному персонажу, или по всем матчам.

        private bool _allOrSelect;
        public bool AllOrSelect
        {
            get => _allOrSelect;
            set
            {
                _allOrSelect = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Команды

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Метод : Определение киллеров, которые находятся в списке матчей

        private void IdentifyKillers(List<GameStatistic> matches)
        {
            KillerList.Clear();

            // Получаем уникальные Id убийц из списка матчей
            var killerIdsInMatches = matches
                .Select(match => match.IdKillerNavigation.IdKiller)
                .Distinct()
                .ToHashSet();

            // Фильтруем убийц, которые участвуют в матчах
            var matchedKillers = _dataManager.Killers
                .Where(killer => killerIdsInMatches.Contains(killer.IdKiller));

            // Добавляем отфильтрованных убийц в KillerList
            foreach (var killer in matchedKillers)
            {
                KillerList.Add(new KillerCollection
                {
                    IdKiller = killer.IdKiller,
                    KillerName = killer.KillerName,
                    KillerImage = killer.KillerImage,
                    CountMatch = matches.Count(x => x.IdKillerNavigation.IdKiller == killer.IdKiller)
                });
            }
        }

        #endregion

        /*--Методы расчета подробной статистики-----------------------------------------------------------*/

        #region Статистика престижей
        
        private void KillerPrestigeStats()
        {
            PrestigeKillerTrackers.Clear();
            for (int i = 0; i <= 100; i++)
            {
                PrestigeKillerTrackers.Add(new PrestigeTracker
                {
                    Prestige = i.ToString(),
                    Count = _matches.Count(x => x.IdKillerNavigation.Prestige == i)
                });
            }
        }

        private void SurvivorPrestigeStats()
        {
            PrestigeSurvivorTrackers.Clear();
            for (int i = 0; i <= 100; i++)
            {
                PrestigeSurvivorTrackers.Add(new PrestigeTracker
                {
                    Prestige = i.ToString(),
                    Count = _matches.Count(x => x.IdSurvivors1Navigation.Prestige == i) +
                            _matches.Count(x => x.IdSurvivors2Navigation.Prestige == i) + 
                            _matches.Count(x => x.IdSurvivors3Navigation.Prestige == i) +
                            _matches.Count(x => x.IdSurvivors4Navigation.Prestige == i),
                });
            }
        }

        #endregion

        #region Статистика используемых пекрво

        private async void KillerFrequencyUsingPerkStats()
        {
            FrequencyKillerUsingPerkTracker.Clear();
            var stats = await CalculationGeneral.FrequencyUsingKillerPerks(_matches, _dataManager.KillerPerks);

            foreach (var item in stats.OrderByDescending(x => x.Count))
            {
                FrequencyKillerUsingPerkTracker.Add(item);
            }
        }

        private async void SurvivorFrequencyUsingPerkStats()
        {
            FrequencySurvivorUsingPerkTracker.Clear();
            var stats = await CalculationGeneral.FrequencyUsingSurvivorPerks(_matches, _dataManager.SurvivorPerks);

            foreach (var item in stats.OrderByDescending(x => x.Count))
            {
                FrequencySurvivorUsingPerkTracker.Add(item);
            }
        }

        #endregion

    }
}
