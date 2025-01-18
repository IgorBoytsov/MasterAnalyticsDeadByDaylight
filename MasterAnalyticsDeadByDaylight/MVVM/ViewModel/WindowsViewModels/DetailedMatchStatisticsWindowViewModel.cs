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

        #region Колекция : Список передоваемых матчей

        private List<GameStatistic> _matches;

        public ObservableCollection<Killer> KillerList { get; set; } = [];

        #endregion

        #region Свойство : Выбор киллера, по которому будет произведен расчет статистики

        private Killer _selectedKiller;
        public Killer SelectedKiller
        {
            get => _selectedKiller;
            set
            {
                _selectedKiller = value;
                OnPropertyChanged();
            }
        }

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

        #region Команды

        #endregion

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
                KillerList.Add(new Killer
                {
                    IdKiller = killer.IdKiller,
                    KillerName = killer.KillerName,
                });
            }
        }

        #endregion

    }
}
