using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation;
using MasterAnalyticsDeadByDaylight.Utils.Calculation;
using MasterAnalyticsDeadByDaylight.Utils.Managers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    public class PerkPageViewModel : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDataService _dataService;
        private readonly IPageNavigationService _pageNavigationService;

        private readonly DataManager _dataManager;

        public PerkPageViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dataService = _serviceProvider.GetService<IDataService>();
            _pageNavigationService = _serviceProvider.GetService<IPageNavigationService>();
            _dataManager = _serviceProvider.GetRequiredService<DataManager>();

            IsPopupFilterOpen = false;
            SelectedAssociation = PlayerAssociations.FirstOrDefault();

            GerKillerPerkCategories();
            GerSurvivorPerkCategories();

            LoadRoles();
        }

        public void Update(object value)
        {
           
        }

        /*--Коллекции-------------------------------------------------------------------------------------*/

        #region Коллекции

        private List<KillerInfo> _killerInfos { get; set; } = [];

        private List<SurvivorInfo> _survivorInfos { get; set; } = [];

        public ObservableCollection<Role> Roles { get; set; } = [];

        public ObservableCollection<string> PlayerAssociations { get; set; } = ["Личная", "Противник"];

        public ObservableCollection<object> Perks { get; set; } = [];

        public ObservableCollection<object> PerkCategories { get; set; } = [];

        public List<KillerPerkCategory> _killerPerkCategories = [];

        public List<SurvivorPerkCategory> _survivorPerkCategories = [];

        public ObservableCollection<PerkStat> PerkStats { get; set; } = [];

        #endregion

        /*--Константы-------------------------------------------------------------------------------------*/

        #region Константы : Id Role

        private const int _KILLER_ROLE = 2;
        private const int _SURVIVOR_ROLE = 3;
        private const int _GENERAL_ROLE = 5;

        #endregion

        #region Константы : ID игровой ассоциации

        private const int _ME = 1;
        private const int _PARTNER = 2;
        private const int _OPPONENT = 3;
        private const int _RANDOM_PLAYER = 4;

        private int _currentAssociation;

        #endregion

        /*--Свойства--------------------------------------------------------------------------------------*/

        #region Свойство : Выбор роли

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();

                PerkStats.Clear();

                if (_selectedRole.IdRole == _KILLER_ROLE)
                {             
                    LoadKillerPerkCategories();
                }
                if (_selectedRole.IdRole == _SURVIVOR_ROLE)
                {
                    LoadSurvivorPerkCategories();
                }

            }
        }

        #endregion

        #region Свойство : Выбор ассоциации

        private string _selectedAssociation;
        public string SelectedAssociation
        {
            get => _selectedAssociation;
            set
            {
                _selectedAssociation = value;
                OnPropertyChanged();

                if (SelectedPerkCategory != null && SelectedPerk != null)
                    DefineAndGerDataForCalculation();
            }
        }

        #endregion

        #region Свойство : Выбор категории перков

        private object _selectedPerkCategory;
        public object SelectedPerkCategory
        {
            get => _selectedPerkCategory;
            set
            {
                _selectedPerkCategory = value;
                OnPropertyChanged();

                if (value is KillerPerkCategory)
                {
                    LoadKillerPerk();
                }
                if (value is SurvivorPerkCategory)
                {
                    LoadSurvivorPerk();
                }
            }
        }

        #endregion

        #region Свойства : Выбор перка \ Выбор индекса

        private object _selectedPerk;
        public object SelectedPerk
        {
            get => _selectedPerk;
            set
            {
                _selectedPerk = value;
                OnPropertyChanged();

                if (value is KillerPerk killerPerk)
                {
                    ImagePerk = killerPerk.PerkImage;
                }
                if (value is SurvivorPerk survivorPerk)
                {
                    ImagePerk = survivorPerk.PerkImage;
                }
                DefineAndGerDataForCalculation();
            }
        }

        private int _selectedPerkIndex;
        public int SelectedPerkIndex
        {
            get => _selectedPerkIndex;
            set
            {
                if (value >= 0 && value < Perks.Count)
                {
                    _selectedPerkIndex = value;
                    SelectedPerk = Perks[value];
                    OnPropertyChanged();
                }
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойство : Изображение перка

        private byte[] _imagePerk;
        public byte[] ImagePerk
        {
            get => _imagePerk;
            set
            {
                _imagePerk = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : PickRate,

        private double _pickRatePerk;
        public double PickRatePerk
        {
            get => _pickRatePerk;
            set
            {
                _pickRatePerk = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PerkCharacterUse> PerkCharactersUse { get; set; } = [];

        #endregion

        #region Свойство : Максимальная ширина элементов

        public int MaxWidth { get; set; } = 1200;

        #endregion

        #region Свойство : Popup - Список киллеров для сравнения

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
        private RelayCommand _nextPerkCommand;
        public RelayCommand NextPerkCommand { get => _nextPerkCommand ??= new(obj => { NextPerk(); }); }

        private RelayCommand _previousPerkCommand;
        public RelayCommand PreviousPerkCommand { get => _previousPerkCommand ??= new(obj => { PreviousPerk(); }); }

        //Команды добавление перков в список сравнения
        private RelayCommand _addSingleToComparisonCommand;
        public RelayCommand AddSingleToComparisonCommand { get => _addSingleToComparisonCommand ??= new(obj => { AddToComparison(); }); }

        private RelayCommand _addAllToComparisonCommand;
        public RelayCommand AddAllToComparisonCommand { get => _addAllToComparisonCommand ??= new(obj => { AddAllToComparison(); }); }

        private RelayCommand _reloadDataCommand;
        public RelayCommand ReloadDataCommand { get => _reloadDataCommand ??= new(obj => { ReloadData(); }); }

        //Очистка списка статистики перков
        private RelayCommand _clearComparisonListCommand;
        public RelayCommand ClearComparisonListCommand { get => _clearComparisonListCommand ??= new(obj => { PerkStats.Clear(); }); }

        //Команд открытие страницы сравнений
        private RelayCommand _openComparisonPageCommand;
        public RelayCommand OpenComparisonPageCommand { get => _openComparisonPageCommand ??= new(obj => { OpenComparisonPage(); }); }

        //Открытие Popup
        private RelayCommand _openPopupListPerksCommand;
        public RelayCommand OpenPopupListPerksCommand { get => _openPopupListPerksCommand ??= new(obj => { IsPopupFilterOpen = true; }); }

        /*--Методы получение \ заполнение данных----------------------------------------------------------*/

        #region Методы : Получение списков PerkCategories

        private void GerKillerPerkCategories()
        {
            _killerPerkCategories.Clear();
            _killerPerkCategories.AddRange(_dataService.GetAllData<KillerPerkCategory>(x => x.OrderBy(x => x.CategoryName)));
        }

        private void GerSurvivorPerkCategories()
        {
            _survivorPerkCategories.Clear();
            _survivorPerkCategories.AddRange(_dataService.GetAllData<SurvivorPerkCategory>(x => x.OrderBy(x => x.CategoryName)));
        }

        #endregion 

        #region Методы : Заполнение списка PerkCategories

        private void LoadKillerPerkCategories()
        {
            PerkCategories.Clear();
            foreach (var category in _killerPerkCategories)
            {
                PerkCategories.Add(category);
            }
            SelectedPerkCategory = PerkCategories.FirstOrDefault();
        }

        private void LoadSurvivorPerkCategories()
        {
            PerkCategories.Clear();
            foreach (var category in _survivorPerkCategories)
            {
                PerkCategories.Add(category);
            }
            SelectedPerkCategory = PerkCategories.FirstOrDefault();
        }

        #endregion

        #region Методы : Заполнение списка Perks

        private void LoadKillerPerk()
        {
            if (SelectedPerkCategory is KillerPerkCategory killerPerkCategory)
            {
                Perks.Clear();
                foreach (var item in _dataManager.KillerPerks.Where(x => x.IdCategory == killerPerkCategory.IdKillerPerkCategory))
                    Perks.Add(item);

                SelectedPerk = Perks.FirstOrDefault();
            }
        }

        private void LoadSurvivorPerk()
        {
            if (SelectedPerkCategory is SurvivorPerkCategory survivorPerkCategory)
            {
                Perks.Clear();
                foreach (var item in _dataManager.SurvivorPerks.Where(x => x.IdCategory == survivorPerkCategory.IdSurvivorPerkCategory))
                    Perks.Add(item);

                SelectedPerk = Perks.FirstOrDefault();
            }
        }

        #endregion

        #region Метод : Заполнение списка Roles

        private void LoadRoles()
        {
            foreach (var item in _dataManager.Roles.Where(x => x.IdRole != _GENERAL_ROLE))
            {
                Roles.Add(item);
            }
            SelectedRole = Roles.FirstOrDefault();
        }

        #endregion

        #region Метод : Получение списка KillerInfo и SurvivorInfo

        private void DefineAndGerDataForCalculation()
        {
            Action action = SelectedAssociation switch
            {
                "Личная" => () =>
                {
                    _killerInfos.Clear();
                    _survivorInfos.Clear();
                    _currentAssociation = _ME;

                    if (SelectedPerk is KillerPerk killerPerk)
                    {
                        _killerInfos.AddRange(_dataService.GetAllData<KillerInfo>(x => x.Where(x => x.IdAssociation == _ME).Where(x => 
                                          x.IdPerk1 == killerPerk.IdKillerPerk ||
                                          x.IdPerk2 == killerPerk.IdKillerPerk ||
                                          x.IdPerk3 == killerPerk.IdKillerPerk ||
                                          x.IdPerk4 == killerPerk.IdKillerPerk)));
                        CalculateKillerPerkHeaderStats();
                        CalculateKillerExtendedStats();
                    }
                    
                    if (SelectedPerk is SurvivorPerk survivorPerk)
                    {
                        _survivorInfos.AddRange(_dataService.GetAllData<SurvivorInfo>(x => x.Where(x => x.IdAssociation == _ME).Where(x =>
                                          x.IdPerk1 == survivorPerk.IdSurvivorPerk ||
                                          x.IdPerk2 == survivorPerk.IdSurvivorPerk ||
                                          x.IdPerk3 == survivorPerk.IdSurvivorPerk ||
                                          x.IdPerk4 == survivorPerk.IdSurvivorPerk)));
                        CalculateSurvivorPerkHeaderStats();
                        CalculateSurvivorExtendedStats();
                    } 
                }
                ,
                "Противник" => () =>
                {
                    _killerInfos.Clear();
                    _survivorInfos.Clear();
                    _currentAssociation = _OPPONENT;

                    if (SelectedPerk is KillerPerk killerPerk)
                    {
                        _killerInfos.AddRange(_dataService.GetAllData<KillerInfo>(x => x.Where(x => x.IdAssociation == _OPPONENT).Where(x =>
                                          x.IdPerk1 == killerPerk.IdKillerPerk ||
                                          x.IdPerk2 == killerPerk.IdKillerPerk ||
                                          x.IdPerk3 == killerPerk.IdKillerPerk ||
                                          x.IdPerk4 == killerPerk.IdKillerPerk)));
                        CalculateKillerPerkHeaderStats();
                        CalculateKillerExtendedStats();
                    }

                    if (SelectedPerk is SurvivorPerk survivorPerk)
                    {
                        _survivorInfos.AddRange(_dataService.GetAllData<SurvivorInfo>(x => x.Where(x => x.IdAssociation == _OPPONENT).Where(x =>
                                          x.IdPerk1 == survivorPerk.IdSurvivorPerk ||
                                          x.IdPerk2 == survivorPerk.IdSurvivorPerk ||
                                          x.IdPerk3 == survivorPerk.IdSurvivorPerk ||
                                          x.IdPerk4 == survivorPerk.IdSurvivorPerk)));
                        CalculateSurvivorPerkHeaderStats();
                        CalculateSurvivorExtendedStats();
                    }
                }
                ,
                _ => () => throw new Exception("Такой статистики не существует.")
            };
            action.Invoke();
        }

        #endregion

        #region Методы : Переключение элементов списка перков (По индексу)

        private void NextPerk()
        {
            SelectedPerkIndex++;
        }

        private void PreviousPerk()
        {
            SelectedPerkIndex--;
        }

        #endregion

        #region Метод : Обновление данных

        private void ReloadData()
        {
            DefineAndGerDataForCalculation();
        }

        #endregion

        /*--Расчеты---------------------------------------------------------------------------------------*/

        #region Метод : Открытие страницы сравнений

        private void OpenComparisonPage()
        {
            _pageNavigationService.NavigateTo("ComparisonPage", PerkStats);
        }

        #endregion

        #region Метод : Основная статистика

        private void CalculateKillerPerkHeaderStats()
        {
            PickRatePerk = CalculationPerk.PickRate(_killerInfos.Count, _dataService.Count<KillerInfo>(x => x.Where(x => x.IdAssociation == _currentAssociation)));
        }

        private void CalculateSurvivorPerkHeaderStats()
        {
            PickRatePerk = CalculationPerk.PickRate(_survivorInfos.Count, _dataService.Count<SurvivorInfo>(x => x.Where(x => x.IdAssociation == _currentAssociation)));
        }

        #endregion

        #region Методы : Расширение статистика

        private void CalculateKillerExtendedStats()
        {
            if (SelectedPerk is KillerPerk killerPerk)
            {
                PerkCharactersUse.Clear();
                List<PerkCharacterUse> perkUse = [];
                foreach (var killer in _dataManager.Killers.Skip(1))
                    perkUse.Add(CalculationPerk.PerkCharactersUse(_killerInfos, killer, killerPerk));

                foreach (var item in perkUse.OrderByDescending(x => x.AmountUsedPerk))
                    PerkCharactersUse.Add(item);
            }  
        }

        private void CalculateSurvivorExtendedStats()
        {
            if (SelectedPerk is SurvivorPerk survivorPerk)
            {
                PerkCharactersUse.Clear();
                List<PerkCharacterUse> perkUse = [];
                foreach (var survivor in _dataManager.Survivors.Skip(1))
                    perkUse.Add(CalculationPerk.PerkCharactersUse(_survivorInfos, survivor, survivorPerk));

                foreach (var item in perkUse.OrderByDescending(x => x.AmountUsedPerk))
                    PerkCharactersUse.Add(item);
            }
        }

        #endregion

        #region Методы : Создание PerkStat - добавлени его в список сравнения
        
        private void AddToComparison()
        {
            if (SelectedPerk is KillerPerk selectedKillerPerk)
            {
                if (PerkStats.Contains(PerkStats.FirstOrDefault(x => x.PerkID == selectedKillerPerk.IdKillerPerk)))
                    return;

                PerkStats.Add(new PerkStat
                {
                    PerkName = selectedKillerPerk.PerkName,
                    PerkImage = selectedKillerPerk.PerkImage,
                    PerkID = selectedKillerPerk.IdKillerPerk,
                    PickRate = PickRatePerk,
                    PerkCharacterUses = [.. PerkCharactersUse],
                });
            }
            if (SelectedPerk is SurvivorPerk selectedSurvivorPerk)
            {
                if (PerkStats.Contains(PerkStats.FirstOrDefault(x => x.PerkID == selectedSurvivorPerk.IdSurvivorPerk)))
                    return;

                PerkStats.Add(new PerkStat
                {
                    PerkName = selectedSurvivorPerk.PerkName,
                    PerkImage = selectedSurvivorPerk.PerkImage,
                    PerkID = selectedSurvivorPerk.IdSurvivorPerk,
                    PickRate = PickRatePerk,
                    PerkCharacterUses = [.. PerkCharactersUse],
                });
            }
        }

        private void AddAllToComparison()
        {
            if (SelectedPerk is KillerPerk)
            {
                foreach (var perk in Perks)
                {
                    if (perk is KillerPerk killerPerk)
                    {
                        if (PerkStats.Contains(PerkStats.FirstOrDefault(x => x.PerkID == killerPerk.IdKillerPerk)))
                            continue;

                        var killerInfos = _dataService.GetAllData<KillerInfo>(x => x.Where(x => x.IdAssociation == _currentAssociation).Where(x =>
                                              x.IdPerk1 == killerPerk.IdKillerPerk ||
                                              x.IdPerk2 == killerPerk.IdKillerPerk ||
                                              x.IdPerk3 == killerPerk.IdKillerPerk ||
                                              x.IdPerk4 == killerPerk.IdKillerPerk));

                        List<PerkCharacterUse> perkUsed = [];

                        foreach (var killer in _dataManager.Killers.Skip(1))
                        {
                            perkUsed.Add(CalculationPerk.PerkCharactersUse(killerInfos, killer, killerPerk));
                        }

                        PerkStats.Add(new PerkStat
                        {
                            PerkName = killerPerk.PerkName,
                            PerkImage = killerPerk.PerkImage,
                            PerkID = killerPerk.IdKillerPerk,
                            PickRate = CalculationPerk.PickRate(killerInfos.Count(), _dataService.Count<KillerInfo>(x => x.Where(x => x.IdAssociation == _currentAssociation))),
                            PerkCharacterUses = [.. perkUsed.OrderByDescending(x => x.AmountUsedPerk)],
                        });
                    }
                } 
            }

            if (SelectedPerk is SurvivorPerk)
            {
                foreach (var perk in Perks)
                {
                    if (perk is SurvivorPerk survivorPerk)
                    {
                        if (PerkStats.Contains(PerkStats.FirstOrDefault(x => x.PerkID == survivorPerk.IdSurvivorPerk)))
                            continue;

                        var survivorInfos = _dataService.GetAllData<SurvivorInfo>(x => x.Where(x => x.IdAssociation == _currentAssociation).Where(x =>
                                              x.IdPerk1 == survivorPerk.IdSurvivorPerk ||
                                              x.IdPerk2 == survivorPerk.IdSurvivorPerk ||
                                              x.IdPerk3 == survivorPerk.IdSurvivorPerk ||
                                              x.IdPerk4 == survivorPerk.IdSurvivorPerk));

                        List<PerkCharacterUse> perkUsed = [];

                        foreach (var survivor in _dataManager.Survivors.Skip(1))
                        {
                            perkUsed.Add(CalculationPerk.PerkCharactersUse(survivorInfos, survivor, survivorPerk));
                        }

                        PerkStats.Add(new PerkStat
                        {
                            PerkName = survivorPerk.PerkName,
                            PerkImage = survivorPerk.PerkImage,
                            PerkID = survivorPerk.IdSurvivorPerk,
                            PickRate = CalculationPerk.PickRate(survivorInfos.Count(), _dataService.Count<SurvivorInfo>(x => x.Where(x => x.IdAssociation == _currentAssociation))),
                            PerkCharacterUses = [.. perkUsed.OrderByDescending(x => x.AmountUsedPerk)],
                        });
                    }
                }
            }
        }

        #endregion

    }
}
