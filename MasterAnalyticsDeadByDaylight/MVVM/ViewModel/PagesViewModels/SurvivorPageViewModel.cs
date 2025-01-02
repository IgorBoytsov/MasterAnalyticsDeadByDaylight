using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation;
using MasterAnalyticsDeadByDaylight.Utils.Calculation;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    class SurvivorPageViewModel : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDataService _dataService;
        private readonly IPageNavigationService _pageNavigationService;

        public SurvivorPageViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dataService = _serviceProvider.GetService<IDataService>();
            _pageNavigationService = _serviceProvider.GetService<IPageNavigationService>();

            IsPopupFilterOpen = false;

            // Получение нужных данных при запуске страницы
            GetSurvivors();
            GetPlayerAssociations();
        }

        public void Update(object value)
        {
            //Обновление расчетов, если был добавлен матч с участие данного выжившего
            if (value is Survivor survivor)
            {
                if (SelectedSurvivor.IdSurvivor == survivor.IdSurvivor)
                {
                    _matches.Clear();
                    _matches.AddRange(GetSurvivorInfo(SelectedSurvivor));
                    CalculateHeaderStats();
                    CalculateExtendedStats();
                }
            }
        }

        /*--Общие Свойства \ Коллекции--------------------------------------------------------------------*/

        #region Коллекции : Общие

        public ObservableCollection<Survivor> Survivors { get; set; } = [];

        public ObservableCollection<PlayerAssociation> PlayerAssociations { get; set; } = [];

        public ObservableCollection<SurvivorStat> SurvivorStats { get; set; } = [];

        #endregion

        #region Коллекции : Расширеная статистика

        public ObservableCollection<SurvivorTypeDeathTracker> SurvivorTypeDeaths { get; set; } = [];

        #endregion

        #region Коллекция : Информация выбранного выжившего

        private List<SurvivorInfo> _matches = [];

        #endregion

        #region Свойства : Выбор выжившего \ выбор индекса

        private Survivor _selectedSurvivor;
        public Survivor SelectedSurvivor
        {
            get => _selectedSurvivor;
            set
            {
                if (_selectedSurvivor != value)
                {
                    _selectedSurvivor = value;
                    _matches.Clear();
                    _matches.AddRange(GetSurvivorInfo(value));
                    CalculateHeaderStats();
                    CalculateExtendedStats();
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedSurvivorIndex;
        public int SelectedSurvivorIndex
        {
            get => _selectedSurvivorIndex;
            set
            {
                if (value >= 0 && value < Survivors.Count)
                {
                    _selectedSurvivorIndex = value;
                    SelectedSurvivor = Survivors[value];
                    OnPropertyChanged();
                }
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Выбор игровой ассоциации

        private PlayerAssociation _selectedPlayerAssociation;
        public PlayerAssociation SelectedPlayerAssociation
        {
            get => _selectedPlayerAssociation;
            set
            {
                if (_selectedPlayerAssociation != value)
                {
                    _selectedPlayerAssociation = value;
                    SurvivorStats.Clear();
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Свойства : CountMatches,E\C, E\R, P\R, AnonymousCount, AnonymousModeRate, BotCount, BotRate

        private int _countMatches;
        public int CountMatches
        {
            get => _countMatches;
            set
            {
                _countMatches = value;
                OnPropertyChanged();
            }
        } 

        private int _escapeCount;
        public int EscapeCount
        {
            get => _escapeCount;
            set
            {
                _escapeCount = value;
                OnPropertyChanged();
            }
        }

        private double _escapeRate;
        public double EscapeRate
        {
            get => _escapeRate;
            set
            {
                _escapeRate = value;
                OnPropertyChanged();
            }
        }

        private double _pickRate;
        public double PickRate
        {
            get => _pickRate;
            set
            {
                _pickRate = value;
                OnPropertyChanged();
            }
        }

        private int _anonymousModeCount;
        public int AnonymousModeCount
        {
            get => _anonymousModeCount;
            set
            {
                _anonymousModeCount = value;
                OnPropertyChanged();
            }
        }

        private double _anonymousModeRate;
        public double AnonymousModeRate
        {
            get => _anonymousModeRate;
            set
            {
                _anonymousModeRate = value;
                OnPropertyChanged();
            }
        }

        private int _botCount;
        public int BotCount
        {
            get => _botCount;
            set
            {
                _botCount = value;
                OnPropertyChanged();
            }
        } 
        
        private double _botRate;
        public double BotRate
        {
            get => _botRate;
            set
            {
                _botRate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойство : Максимальная ширина элементов

        public int MaxWidth { get; set; } = 1200;

        #endregion

        #region Свойство : Popup - Список выживших для сравнения

        private bool _isPopupFilterOpen;
        public bool IsPopupFilterOpen
        {
            get => _isPopupFilterOpen;
            set
            {
                _isPopupFilterOpen = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        // Команды переключение индексов
        private RelayCommand _nextSurvivorCommand;
        public RelayCommand NextSurvivorCommand { get => _nextSurvivorCommand ??= new(obj => { NextSurvivor(); }); }

        private RelayCommand _previousSurvivorCommand;
        public RelayCommand PreviousSurvivorCommand { get => _previousSurvivorCommand ??= new(obj => { PreviousSurvivor(); }); }

        //Команды добавление киллеров в список сравнения
        private RelayCommand _addSingleToComparisonCommand;
        public RelayCommand AddSingleToComparisonCommand { get => _addSingleToComparisonCommand ??= new(obj => { AddToComparison(); }); }

        private RelayCommand _addAllToComparisonCommand;
        public RelayCommand AddAllToComparisonCommand { get => _addAllToComparisonCommand ??= new(obj => { AddAllToComparison(); }); }

        //Очистка списка статистики киллеров
        private RelayCommand _clearComparisonListCommand;
        public RelayCommand ClearComparisonListCommand { get => _clearComparisonListCommand ??= new(obj => { SurvivorStats.Clear(); }); }

        //Команд открытие страницы сравнений
        private RelayCommand _openComparisonPageCommand;
        public RelayCommand OpenComparisonPageCommand { get => _openComparisonPageCommand ??= new(obj => { OpenComparisonPage(); }); }

        //Команда обновление данных
        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { ReloadData(); }); }

        //Открытие Popup
        private RelayCommand _openPopupListSurvivorsCommand;
        public RelayCommand OpenPopupListSurvivorsCommand { get => _openPopupListSurvivorsCommand ??= new(obj => { IsPopupFilterOpen = true; }); }

        /*--Получение первоначальных данных---------------------------------------------------------------*/

        #region Метод : Получение списка "Выживших"

        private void GetSurvivors()
        {
            foreach (var item in _dataService.GetAllDataInList<Survivor>(x => x.Skip(1)))
            {
                Survivors.Add(item);
            }
        }

        #endregion

        #region Метод : Получение списка "Игровой ассоциации" \ Присваивание по умолчанию SelectedPlayerAssociation первый элемент из PlayerAssociations

        private void GetPlayerAssociations()
        {
            foreach (var item in _dataService.GetAllDataInList<PlayerAssociation>())
            {
                PlayerAssociations.Add(item);
            }
            SelectedPlayerAssociation = PlayerAssociations.FirstOrDefault();
        }

        #endregion 

        #region Метод : Получение списка информации о выживших

        private List<SurvivorInfo> GetSurvivorInfo(Survivor survivor)
        {
            return _dataService.GetAllDataInList<SurvivorInfo>(
                x => x.Where(x => x.IdSurvivor == survivor.IdSurvivor && x.IdAssociation == SelectedPlayerAssociation.IdPlayerAssociation));
        }

        #endregion

        #region Метод : Обновление данных

        private void ReloadData()
        {
            _matches.Clear();
            _matches.AddRange(GetSurvivorInfo(SelectedSurvivor));
            CalculateHeaderStats();
            CalculateExtendedStats();
        }

        #endregion 

        /*--Взаимодействие с списком----------------------------------------------------------------------*/

        #region Методы : Переключение элементов списка выживщих (По индексу)

        private void PreviousSurvivor()
        {
            SelectedSurvivorIndex--;
        }

        private void NextSurvivor()
        {
            SelectedSurvivorIndex++;
        }

        #endregion

        /*--Расчеты---------------------------------------------------------------------------------------*/

        #region Метод : Открытие страницы сравнений

        private void OpenComparisonPage()
        {
            _pageNavigationService.NavigateTo("ComparisonPage", SurvivorStats);
        }

        #endregion

        #region Метод : Основная статистика

        private void CalculateHeaderStats()
        {
            if (_matches.Count != 0)
            {
                var allSurvivorsCount = _dataService.Count<SurvivorInfo>(x => x.Where(x => x.IdAssociation == SelectedPlayerAssociation.IdPlayerAssociation));
                CountMatches = _matches.Count;

                EscapeCount = CalculationSurvivor.EscapeCount(_matches, SelectedSurvivor.IdSurvivor);
                EscapeRate = CalculationSurvivor.EscapeRate(EscapeCount, CountMatches);

                PickRate = CalculationSurvivor.PickRate(CountMatches, allSurvivorsCount);

                AnonymousModeCount = CalculationSurvivor.AnonymousModeCount(_matches);
                AnonymousModeRate = CalculationSurvivor.AnonymousModeRate(AnonymousModeCount, CountMatches);

                BotCount = CalculationSurvivor.BotCount(_matches);
                BotRate = CalculationSurvivor.BotRate(BotCount, CountMatches);
            }
            else
            {
                CountMatches = 0;
                EscapeCount = 0; 
                EscapeRate = 0; 
                PickRate = 0;
                AnonymousModeCount = 0;
                AnonymousModeRate = 0;
                BotCount = 0;
                BotRate = 0;
            }
        }

        #endregion

        #region Методы : Расширение статистика

        private async void CalculateExtendedStats()
        {
            if (_matches.Count != 0)
            {
                SurvivorTypeDeaths.Clear();

                foreach (var item in await CalculationSurvivor.TypeDeathSurvivorsAsync(_matches, _dataService))
                    SurvivorTypeDeaths.Add(item);
            }
            else
            {
                SurvivorTypeDeaths.Clear();
                SurvivorTypeDeaths.Add(new SurvivorTypeDeathTracker { TypeDeathName = "Нету данных", CountGame = 0, TypeDeathPercentages = 0 });
            }
        }

        #endregion

        #region Методы : Создание SurvivorStat - добавлени его в список сравнения

        private void AddToComparison()
        {
            if (SurvivorStats.Contains(SurvivorStats.FirstOrDefault(x => x.IdSurvivor == SelectedSurvivor.IdSurvivor)))
                return;

            var survivorStat = new SurvivorStat
            {
                IdSurvivor = SelectedSurvivor.IdSurvivor,
                SurvivorName = SelectedSurvivor.SurvivorName,
                SurvivorImage = SelectedSurvivor.SurvivorImage,
                SurvivorCount = CountMatches,
                SurvivorPickRate = PickRate,
                SurvivorEscapeCount = EscapeCount,
                SurvivorEscapePercentage = EscapeRate,
                SurvivorAnonymousModeCount = AnonymousModeCount,
                SurvivorAnonymousModePercentage = AnonymousModeRate,
                SurvivorBotCount = BotCount,
                SurvivorBotPercentage = BotRate
            };

            SurvivorStats.Add(survivorStat);
        }

        private void AddAllToComparison()
        {
            var allSurvivorsCount = _dataService.Count<SurvivorInfo>(x => x.Where(x => x.IdAssociation == SelectedPlayerAssociation.IdPlayerAssociation));

            foreach (var survivor in Survivors)
            {
                if (SurvivorStats.Contains(SurvivorStats.FirstOrDefault(x => x.IdSurvivor == survivor.IdSurvivor)))
                    continue;

                var matches = GetSurvivorInfo(survivor);

                var survivorStat = new SurvivorStat
                {
                    IdSurvivor = survivor.IdSurvivor,
                    SurvivorName = survivor.SurvivorName,
                    SurvivorImage = survivor.SurvivorImage,
                    SurvivorCount = matches.Count,
                    SurvivorPickRate = CalculationSurvivor.PickRate(matches.Count, allSurvivorsCount),
                    SurvivorEscapeCount = CalculationSurvivor.EscapeCount(matches, survivor.IdSurvivor),
                    SurvivorEscapePercentage = CalculationSurvivor.EscapeRate(CalculationSurvivor.EscapeCount(matches, survivor.IdSurvivor), matches.Count),
                    SurvivorAnonymousModeCount = CalculationSurvivor.AnonymousModeCount(matches),
                    SurvivorAnonymousModePercentage = CalculationSurvivor.AnonymousModeRate(CalculationSurvivor.AnonymousModeCount(matches), matches.Count),
                    SurvivorBotCount = CalculationSurvivor.BotCount(matches),
                    SurvivorBotPercentage = CalculationSurvivor.BotRate(CalculationSurvivor.BotCount(matches), matches.Count)
                };
                SurvivorStats.Add(survivorStat);
            }
        }

        #endregion

    }
}
