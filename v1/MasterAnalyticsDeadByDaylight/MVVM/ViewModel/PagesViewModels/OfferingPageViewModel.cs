using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation;
using MasterAnalyticsDeadByDaylight.Utils.Calculation;
using MasterAnalyticsDeadByDaylight.Utils.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Data;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    public class OfferingPageViewModel : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDataService _dataService;
        private readonly IPageNavigationService _pageNavigationService;

        private readonly DataManager _dataManager;

        public OfferingPageViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dataService = _serviceProvider.GetService<IDataService>();
            _pageNavigationService = _serviceProvider.GetService<IPageNavigationService>();
            _dataManager = _serviceProvider.GetRequiredService<DataManager>();

            IsPopupFilterOpen = false;

            GetKillers();
            GetSurvivors();

            SelectedAssociation = PlayerAssociations.FirstOrDefault();
            GetOfferingCategories();
        }

        public void Update(object value)
        {
           
        }

        /*--Общие Свойства \ Коллекции--------------------------------------------------------------------*/

        #region Коллекции : Общие

        //public ObservableCollection<string> PlayerAssociations { get; set; } = ["Личная за киллера","Личная за выживших","Противник киллер","Противник выживший"];

        public ObservableCollection<string> PlayerAssociations { get; set; } = ["Личная", "Противник"];

        public ObservableCollection<Offering> Offerings { get; set; } = [];

        public ObservableCollection<OfferingCategory> OfferingCategories { get; set; } = [];

        private List<Survivor> _survivors { get; set; } = [];

        private List<Killer> _killers { get; set; } = [];

        private List<KillerInfo> _killerInfos { get; set; } = [];

        private List<SurvivorInfo> _survivorInfos { get; set; } = [];

        public ObservableCollection<OfferingStat> OfferingStats { get; set; } = [];

        #endregion

        #region Константы : ID игровой ассоциации

        private const int _ME = 1;
        private const int _PARTNER = 2;
        private const int _OPPONENT = 3;
        private const int _RANDOM_PLAYER = 4;

        private int _currentAssociation;

        #endregion

        #region Свойства : Выбор роли

        private string _selectedAssociation;
        public string SelectedAssociation
        {
            get => _selectedAssociation;
            set
            {
                _selectedAssociation = value;
                if (SelectedOfferingCategory != null || SelectedOffering != null)
                {
                    DefineAndGetOfferingData();
                }
                OfferingStats.Clear();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Выбор типа подношения

        private OfferingCategory _selectedOfferingCategory;
        public OfferingCategory SelectedOfferingCategory
        {
            get => _selectedOfferingCategory;
            set
            {
                _selectedOfferingCategory = value;
                GetOfferings();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Выбор подношения | Индекса 

        private Offering _selectedOffering;
        public Offering SelectedOffering
        {
            get => _selectedOffering;
            set
            {
                _selectedOffering = value;
                if (value != null)
                {
                    DefineAndGetOfferingData();
                    OnPropertyChanged();
                }
                
            }
        } 
        
        private int _selectedOfferingIndex;
        public int SelectedOfferingIndex
        {
            get => _selectedOfferingIndex;
            set
            {
                if (value >= 0 && value < Offerings.Count)
                {
                    _selectedOfferingIndex = value;
                    SelectedOffering = Offerings[value];
                    OnPropertyChanged();
                }
                OnPropertyChanged();
            }
        }

        #endregion  

        #region Свойство : Максимальная ширина элементов

        public int MaxWidth { get; set; } = 1200;

        #endregion

        #region Свойства : CountMatch, PickRate

        private int _takeIntoMatchCount;
        public int TakeIntoMatchCount
        {
            get => _takeIntoMatchCount;
            set
            {
                _takeIntoMatchCount = value;
                OnPropertyChanged();
            }
        }

        private double _pickRateKiller;
        public double PickRateKiller
        {
            get => _pickRateKiller;
            set
            {
                _pickRateKiller = value;
                OnPropertyChanged();
            }
        }

        private double _pickRateSurvivor;
        public double PickRateSurvivor
        {
            get => _pickRateSurvivor;
            set
            {
                _pickRateSurvivor = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<OfferingCharacterUse> OfferingKillerUse { get; set; } = [];

        public ObservableCollection<OfferingCharacterUse> OfferingSurvivorUse { get; set; } = [];

        #endregion

        #region Свойство : Popup - Список подношений для сравнения

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
        private RelayCommand _nextOfferingCommand;
        public RelayCommand NextOfferingCommand { get => _nextOfferingCommand ??= new(obj => { NextOffering(); }); }

        private RelayCommand _previousOfferingCommand;
        public RelayCommand PreviousOfferingCommand { get => _previousOfferingCommand ??= new(obj => { PreviousOffering(); }); }

        //Команды добавление киллеров в список сравнения
        private RelayCommand _addSingleToComparisonCommand;
        public RelayCommand AddSingleToComparisonCommand { get => _addSingleToComparisonCommand ??= new(obj => { AddToComparison(); }); }

        private RelayCommand _addAllToComparisonCommand;
        public RelayCommand AddAllToComparisonCommand { get => _addAllToComparisonCommand ??= new(obj => { AddAllToComparison(); }); }
       
        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { ReloadData(); }); }

        //Очистка списка статистики киллеров
        private RelayCommand _clearComparisonListCommand;
        public RelayCommand ClearComparisonListCommand { get => _clearComparisonListCommand ??= new(obj => { OfferingStats.Clear(); }); }

        //Команд открытие страницы сравнений
        private RelayCommand _openComparisonPageCommand;
        public RelayCommand OpenComparisonPageCommand { get => _openComparisonPageCommand ??= new(obj => { OpenComparisonPage(); }); }

        //Открытие Popup
        private RelayCommand _openPopupListOfferingsCommand;
        public RelayCommand OpenPopupListOfferingsCommand { get => _openPopupListOfferingsCommand ??= new(obj => { IsPopupFilterOpen = true; }); }

        /*--Получение первоначальных данных---------------------------------------------------------------*/

        #region Метод : Определение и получение нужных данных в список Offering "Подношение" 

        private void DefineAndGetOfferingData()
        {
            _killerInfos.Clear();
            _survivorInfos.Clear();
            Action action = SelectedAssociation switch
            {
                "Личная" => () =>
                {
                    _currentAssociation = _ME;
                    _killerInfos.AddRange(_dataService.GetAllDataInList<KillerInfo>(x => x.Where(x => x.IdKillerOffering == SelectedOffering.IdOffering && x.IdAssociation == _ME)));
                    _survivorInfos.AddRange(_dataService.GetAllDataInList<SurvivorInfo>(x => x.Where(x => x.IdSurvivorOffering == SelectedOffering.IdOffering && x.IdAssociation == _ME)));
                    CalculateHeaderStats();
                    CalculateExtendedStats();
                }
                ,
                "Противник" => () =>
                {
                    _currentAssociation = _OPPONENT;
                    _killerInfos.AddRange(_dataService.GetAllDataInList<KillerInfo>(x => x.Where(x => x.IdKillerOffering == SelectedOffering.IdOffering && x.IdAssociation == _OPPONENT)));
                    _survivorInfos.AddRange(_dataService.GetAllDataInList<SurvivorInfo>(x => x.Where(x => x.IdSurvivorOffering == SelectedOffering.IdOffering && x.IdAssociation == _OPPONENT)));
                    CalculateHeaderStats();
                    CalculateExtendedStats();
                }
                ,
                _ => null
            };
            action?.Invoke();
        }

        #endregion

        #region Метод : Получение списка "Категории подношений"

        private void GetOfferingCategories()
        {
            OfferingCategories.Clear();
            foreach (var item in _dataService.GetAllDataInList<OfferingCategory>())
                OfferingCategories.Add(item);

            SelectedOfferingCategory = OfferingCategories.FirstOrDefault();
        }

        #endregion

        #region Метод : Получение списка "Подношения"

        private void GetOfferings()
        {
            Offerings.Clear();
            foreach (var item in _dataService.GetAllDataInList<Offering>(x => x.Include(x => x.IdRoleNavigation).Where(x => x.IdCategory == SelectedOfferingCategory.IdCategory)))
                Offerings.Add(item);

            SelectedOffering = Offerings.FirstOrDefault();
        }

        #endregion

        #region Метод : Получение списка "Киллеров"

        private void GetKillers()
        {
            _killers.AddRange(_dataService.GetAllDataInList<Killer>());
        }

        #endregion

        #region Метод : Получение списка "Выживших"

        private void GetSurvivors()
        {
            _survivors.AddRange(_dataService.GetAllDataInList<Survivor>());
        }

        #endregion

        /*--Взаимодействие с списком----------------------------------------------------------------------*/

        #region Методы : Переключение элементов списка выживщих (По индексу)

        private void PreviousOffering()
        {
            SelectedOfferingIndex--;
        }

        private void NextOffering()
        {
            SelectedOfferingIndex++;
        }

        #endregion

        #region Метод : Обновление данных

        private void ReloadData()
        {
            DefineAndGetOfferingData();
        }

        #endregion

        /*--Расчеты---------------------------------------------------------------------------------------*/

        #region Метод : Открытие страницы сравнений

        private void OpenComparisonPage()
        {
            _pageNavigationService.NavigateTo("ComparisonPage", OfferingStats);
        }

        #endregion

        #region Метод : Основная статистика

        private void CalculateHeaderStats()
        {
            TakeIntoMatchCount = 0;
            CalculateHeaderKillerStats();
            CalculateHeaderSurvivorStats();
        }

        private void CalculateHeaderKillerStats()
        {
            TakeIntoMatchCount += _killerInfos.Count;
            PickRateKiller = CalculationOffering.PickRate(_killerInfos.Count, _dataService.Count<KillerInfo>(x => x.Where(x => x.IdAssociation == _currentAssociation)));
        }

        private void CalculateHeaderSurvivorStats()
        {
            TakeIntoMatchCount += _survivorInfos.Count;
            PickRateSurvivor = CalculationOffering.PickRate(_survivorInfos.Count, _dataService.Count<SurvivorInfo>(x => x.Where(x => x.IdAssociation == _currentAssociation)));
        }

        #endregion

        #region Методы : Расширение статистика

        private void CalculateExtendedStats()
        {
            CalculateExtendedKillerStats();
            CalculateExtendedSurvivorStats();
        }

        private void CalculateExtendedKillerStats()
        {
            OfferingKillerUse.Clear();
            List<OfferingCharacterUse> offeringUse = [];
            foreach (var killer in _killers.Skip(1))
                offeringUse.Add(CalculationOffering.OfferingKillerUses(_killerInfos, killer, SelectedOffering));

            foreach (var item in offeringUse.OrderByDescending(x => x.AmountUsedOffering))
                OfferingKillerUse.Add(item);
        }

        private void CalculateExtendedSurvivorStats()
        {
            OfferingSurvivorUse.Clear();
            List<OfferingCharacterUse> offeringUse = [];
            foreach (var survivor in _survivors.Skip(1))
                offeringUse.Add(CalculationOffering.OfferingSurvivorUses(_survivorInfos, survivor, SelectedOffering));

            foreach (var item in offeringUse.OrderByDescending(x => x.AmountUsedOffering))
                OfferingSurvivorUse.Add(item);
        }

        #endregion

        #region Методы : Создание OfferingStat - добавлени его в список сравнения

        private void AddToComparison()
        {
            if (OfferingStats.Contains(OfferingStats.FirstOrDefault(x => x.OfferingID == SelectedOffering.IdOffering)))
                return;

            OfferingStats.Add(new OfferingStat()
            {
                OfferingName = SelectedOffering.OfferingName,
                OfferingImage = SelectedOffering.OfferingImage,
                OfferingID = SelectedOffering.IdOffering,
                OfferingRole = SelectedOffering.IdRole,
                OfferingKillerUses = [.. OfferingKillerUse.OrderByDescending(x => x.AmountUsedOffering)],
                PickKillerOfferingCount = _killerInfos.Count,
                PickRateKillerOffering = PickRateKiller,
                OfferingSurvivorUses = [.. OfferingSurvivorUse.OrderByDescending(x => x.AmountUsedOffering)],
                PickSurvivorOfferingCount = _survivorInfos.Count,
                PickRateSurvivorOffering = PickRateSurvivor,
            });
        }

        private async void AddAllToComparison()
        {   
            foreach (var offering in Offerings)
            {
                if (OfferingStats.Contains(OfferingStats.FirstOrDefault(x => x.OfferingID == offering.IdOffering)))
                    continue;

                List<OfferingCharacterUse> offeringKillerUse = [];
                List<OfferingCharacterUse> offeringSurvivorsUse = [];

                int countKillerRecords = 0;
                int countSurvivorRecords = 0;

                foreach (var killer in _dataManager.Killers.Skip(1))
                {
                    var killerInfos = await _dataService.GetAllDataInListAsync<KillerInfo>(x => x.Include(x => x.IdKillerNavigation)
                            .Where(x => x.IdAssociation == _currentAssociation)
                                .Where(x => x.IdKiller == killer.IdKiller)
                                    .Where(x => x.IdKillerOffering == offering.IdOffering));

                    countKillerRecords += killerInfos.Count;

                    offeringKillerUse.Add(new OfferingCharacterUse() { NameCharacter = killer.KillerName,AmountUsedOffering = killerInfos.Count });
                }

                foreach (var survivor in _dataManager.Survivors.Skip(1))
                {
                    var survivorInfos = await _dataService.GetAllDataInListAsync<SurvivorInfo>(x => x
                        .Include(x => x.IdSurvivorNavigation)
                            .Where(x => x.IdAssociation == _currentAssociation)
                                .Where(x => x.IdSurvivor == survivor.IdSurvivor)
                                    .Where(x => x.IdSurvivorOffering == offering.IdOffering));

                    countSurvivorRecords += survivorInfos.Count;

                    offeringSurvivorsUse.Add(new OfferingCharacterUse() { NameCharacter = survivor.SurvivorName, AmountUsedOffering = survivorInfos.Count });
                }

                OfferingStats.Add(new OfferingStat
                {
                    OfferingName = offering.OfferingName,
                    OfferingImage = offering.OfferingImage,
                    OfferingID = offering.IdOffering,
                    OfferingRole = offering.IdRole,
                    OfferingKillerUses = [.. offeringKillerUse.OrderByDescending(x => x.AmountUsedOffering)],
                    PickKillerOfferingCount = countKillerRecords,
                    PickRateKillerOffering = CalculationOffering.PickRate(countKillerRecords, _dataService.Count<KillerInfo>(x => x.Where(x => x.IdAssociation == _currentAssociation))),
                    OfferingSurvivorUses = [.. offeringSurvivorsUse.OrderByDescending(x => x.AmountUsedOffering)],
                    PickSurvivorOfferingCount = countSurvivorRecords,
                    PickRateSurvivorOffering = CalculationOffering.PickRate(countSurvivorRecords, _dataService.Count<SurvivorInfo>(x => x.Where(x => x.IdAssociation == _currentAssociation))),
                });
            }
        }

        #endregion
    }
}