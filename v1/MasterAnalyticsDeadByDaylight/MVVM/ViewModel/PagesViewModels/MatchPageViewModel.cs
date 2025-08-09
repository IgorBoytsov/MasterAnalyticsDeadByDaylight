using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Services.DatabaseServices;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.WindowNavigation;
using MasterAnalyticsDeadByDaylight.Utils.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    internal class MatchPageViewModel : BaseViewModel, IUpdatable
    {
        private List<GameStatistic> _allGameMatch { get; set; } = [];

        private List<GameStatistic> _filteredMatch { get; set; } = [];

        public ObservableCollection<GameStatistic> GameMatchList { get; set; } = [];

        private readonly IServiceProvider _serviceProvider;

        private readonly IDataService _dataService;
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly LastRecordHelper _lastRecordHelper;

        public MatchPageViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dataService = _serviceProvider.GetService<IDataService>();
            _windowNavigationService = _serviceProvider.GetService<IWindowNavigationService>();
            _lastRecordHelper = serviceProvider.GetRequiredService<LastRecordHelper>();

            ConsiderKillerPrestige = false;
            ConsiderKillerScore = false;
            ConsiderDateMatch = false;

            SelectedSortingMatch = _NOT_INCLUDE;

            GetFilterData();
            GetGameStatisticData();
        }

        public void Update(object value)
        {
            //Условие приходит из сервиса навигации. Если приходит свойство bool, то происходит обновление записи без открытие страницы (Если страница не открыта, то она и не будет открыта).
            if (value is bool trueUpdateList)
            {
                var newMatch = _lastRecordHelper.GameStatisticLastRecord();                                                                              

                _allGameMatch.Insert(0, newMatch);
                CalculateTotalPages(_allGameMatch.Count);
                ApplySorting();
                ApplyFilter();
                LoadGameStatistics();
            }
        }

        #region Команды взоимодейсвтие с данными

        private RelayCommand _updateMatchCommand;
        public RelayCommand UpdateMatchCommand
        {
            get => _updateMatchCommand ??= new(obj =>
            {
                GetGameStatisticData();
                LoadGameStatistics();
            });
        }

        private RelayCommand _showMatchCommand;
        public RelayCommand ShowMatchCommand => _showMatchCommand ??= new RelayCommand(ShowMatch);

        private RelayCommand _showDetailedStatisticsCommand;
        public RelayCommand ShowDetailedStatisticsCommand { get => _showDetailedStatisticsCommand ??= new(obj => { ShowDetailedStatistics(); }); }

        #endregion

        private void ShowMatch(object parameter)
        {
            if (parameter is GameStatistic selectedGameMatch)
            {
                _windowNavigationService.OpenWindow("ShowDetailsMatchWindow", selectedGameMatch);
            }
        }

        private void ShowDetailedStatistics()
        {
            if (GameMatchList.Count != 0)
            {
                _windowNavigationService.OpenWindow("DetailedMatchStatisticsWindow", _filteredMatch);
            }
        }

        #region Фильтр Popup

        private const string _NOT_INCLUDE = "Не учитывать";

        #region Коллекции фильтрации     

        public ObservableCollection<Killer> Killers { get; set; } = [];

        public ObservableCollection<Platform> KillerPlatforms { get; set; } = [];

        public ObservableCollection<KillerAddon> KillerAddons { get; set; } = [];

        public ObservableCollection<Offering> KillerOfferings { get; set; } = [];

        public ObservableCollection<Offering> SurvivorOfferings { get; set; } = [];

        public ObservableCollection<PlayerAssociation> KillerPlayerAssociations { get; set; } = [];

        public ObservableCollection<PlayerAssociation> SurvivorPlayerAssociations { get; set; } = [];

        public ObservableCollection<GameMode> GameMods { get; set; } = [];

        public ObservableCollection<GameEvent> GameEvents { get; set; } = [];

        public ObservableCollection<Map> Maps { get; set; } = [];

        public ObservableCollection<Measurement> Measurements { get; set; } = [];

        public ObservableCollection<Patch> Patches { get; set; } = [];

        public List<string> KillerWinOrLoss { get; set; } = [_NOT_INCLUDE, "Победа", "Ничья", "Проигрыш"];

        public List<WhoPlacedMap> WhoPleaseMaps { get; set; } = [];

        public List<WhoPlacedMap> WhoPleaseMapWon { get; set; } = [];

        public List<string> CountKills { get; set; } = [_NOT_INCLUDE, "0", "1", "2", "3", "4"];

        public List<string> CountHooks { get; set; } = [_NOT_INCLUDE, "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];

        public List<string> NumberRecentGenerators { get; set; } = [_NOT_INCLUDE, "0", "1", "2", "3", "4", "5"];

        #endregion  

        #region Коллекции сортировки

        public List<string> SortingMatch { get; set; } =
          [
              _NOT_INCLUDE,
              "От старой дате к новой", "От новой дате к старой",
              "Убыванию длинны матча", "Возрастанию длинны матча",
          ];

        #endregion 

        #region Свойства выбора 

        #region Киллера

        private Killer _selectedKiller;
        public Killer SelectedKiller
        {
            get => _selectedKiller;
            set
            {
                _selectedKiller = value;
                GetKillerAddons();
                OnPropertyChanged();
            }
        }

        private Platform _selectedKillerPlatform;
        public Platform SelectedKillerPlatform
        {
            get => _selectedKillerPlatform;
            set
            {
                _selectedKillerPlatform = value;
                GetKillerAddons();
                OnPropertyChanged();
            }
        }

        private Offering _selectedKillerOffering;
        public Offering SelectedKillerOffering
        {
            get => _selectedKillerOffering;
            set
            {
                _selectedKillerOffering = value;
                OnPropertyChanged();
            }
        }

        private KillerAddon _selectedFirstKillerAddon;
        public KillerAddon SelectedFirstKillerAddon
        {
            get => _selectedFirstKillerAddon;
            set
            {
                _selectedFirstKillerAddon = value;
                OnPropertyChanged();
            }
        }

        private KillerAddon _selectedSecondKillerAddon;
        public KillerAddon SelectedSecondKillerAddon
        {
            get => _selectedSecondKillerAddon;
            set
            {
                _selectedSecondKillerAddon = value;
                OnPropertyChanged();
            }
        }

        private string _selectedKillerWinOrLoss;
        public string SelectedKillerWinOrLoss
        {
            get => _selectedKillerWinOrLoss;
            set
            {
                _selectedKillerWinOrLoss = value;
                OnPropertyChanged();
            }
        }

        private int _firstNumberKillerPrestige;
        public int FirstNumberKillerPrestige
        {
            get => _firstNumberKillerPrestige;
            set
            {
                _firstNumberKillerPrestige = value;
                OnPropertyChanged();
            }
        }

        private int _secondNumberKillerPrestige;
        public int SecondNumberKillerPrestige
        {
            get => _secondNumberKillerPrestige;
            set
            {
                _secondNumberKillerPrestige = value;
                OnPropertyChanged();
            }
        }

        private bool _considerKillerPrestige;
        public bool ConsiderKillerPrestige
        {
            get => _considerKillerPrestige;
            set
            {
                _considerKillerPrestige = value;
                OnPropertyChanged();
            }
        }

        private int _startScoreKiller;
        public int StartScoreKiller
        {
            get => _startScoreKiller;
            set
            {
                _startScoreKiller = value;
                OnPropertyChanged();
            }
        }

        private int _endScoreKiller;
        public int EndScoreKiller
        {
            get => _endScoreKiller;
            set
            {
                _endScoreKiller = value;
                OnPropertyChanged();
            }
        }

        private bool _considerKillerScore;
        public bool ConsiderKillerScore
        {
            get => _considerKillerScore;
            set
            {
                _considerKillerScore = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Выжившему

        #endregion

        #region Игре

        private Map _selectedMap;
        public Map SelectedMap
        {
            get => _selectedMap;
            set
            {
                _selectedMap = value;
                OnPropertyChanged();
            }
        }

        private Measurement _selectedMeasurement;
        public Measurement SelectedMeasurement
        {
            get => _selectedMeasurement;
            set
            {
                _selectedMeasurement = value;
                OnPropertyChanged();
            }
        }

        private GameMode _selectedGameMode;
        public GameMode SelectedGameMode
        {
            get => _selectedGameMode;
            set
            {
                _selectedGameMode = value;
                OnPropertyChanged();
            }
        }

        private GameEvent _selectedGameEvent;
        public GameEvent SelectedGameEvent
        {
            get => _selectedGameEvent;
            set
            {
                _selectedGameEvent = value;
                OnPropertyChanged();
            }
        }

        private Patch _selectedPatch;
        public Patch SelectedPatch
        {
            get => _selectedPatch;
            set
            {
                _selectedPatch = value;
                OnPropertyChanged();
            }
        }

        private WhoPlacedMap _selectedWhoPleaseMap;
        public WhoPlacedMap SelectedWhoPleaseMap
        {
            get => _selectedWhoPleaseMap;
            set
            {
                _selectedWhoPleaseMap = value;
                OnPropertyChanged();
            }
        }

        private WhoPlacedMap _selectedWhoPleaseMapWin;
        public WhoPlacedMap SelectedWhoPleaseMapWin
        {
            get => _selectedWhoPleaseMapWin;
            set
            {
                _selectedWhoPleaseMapWin = value;
                OnPropertyChanged();
            }
        }

        private string _selectedCountKill;
        public string SelectedCountKill
        {
            get => _selectedCountKill;
            set
            {
                _selectedCountKill = value;
                OnPropertyChanged();
            }
        }

        private string _selectedCountHook;
        public string SelectedCountHook
        {
            get => _selectedCountHook;
            set
            {
                _selectedCountHook = value;
                OnPropertyChanged();
            }
        }

        private string _selectedRecentGenerators;
        public string SelectedRecentGenerators
        {
            get => _selectedRecentGenerators;
            set
            {
                _selectedRecentGenerators = value;
                OnPropertyChanged();
            }
        }

        private bool _considerDateMatch;
        public bool ConsiderDateMatch
        {
            get => _considerDateMatch;
            set
            {
                _considerDateMatch = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Сортировка

        private string _selectedSortingMatch;
        public string SelectedSortingMatch
        {
            get => _selectedSortingMatch;
            set
            {
                _selectedSortingMatch = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion 

        #region Свойства Popup

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

        #region Команды 

        private RelayCommand _openPopupFilter;
        public RelayCommand OpenPopupFilter { get => _openPopupFilter ??= new(obj => { IsPopupFilterOpen = true; }); }

        private RelayCommand _applyChangeCommand;
        public RelayCommand ApplyChangeCommand
        {
            get => _applyChangeCommand ??= new(obj =>
            {
                IsPopupFilterOpen = false;
                CurrentPage = 1;
                ApplySorting();
                ApplyFilter();
                LoadGameStatistics();
            });
        }

        private RelayCommand _resetFilterCommand;
        public RelayCommand ResetFilterCommand
        {
            get => _resetFilterCommand ??= new(obj =>
            {
                ResetFilter();
            });
        }

        private void ResetFilter()
        {
            SelectedKiller = Killers.FirstOrDefault();
            SelectedKillerPlatform = KillerPlatforms.FirstOrDefault();
            SelectedKillerOffering = KillerOfferings.FirstOrDefault();
            SelectedKillerWinOrLoss = KillerWinOrLoss.FirstOrDefault();
            ConsiderKillerPrestige = false;
            ConsiderKillerScore = false;
            StartScoreKiller = 0;
            EndScoreKiller = 0;
            FirstNumberKillerPrestige = 0;
            SecondNumberKillerPrestige = 0;

            SelectedMap = Maps.FirstOrDefault();
            SelectedMeasurement = Measurements.FirstOrDefault();
            SelectedGameMode = GameMods.FirstOrDefault();
            SelectedGameEvent = GameEvents.FirstOrDefault();
            SelectedPatch = Patches.FirstOrDefault();
            SelectedWhoPleaseMap = WhoPleaseMaps.FirstOrDefault();
            SelectedWhoPleaseMapWin = WhoPleaseMapWon.FirstOrDefault();
            SelectedCountKill = CountKills.FirstOrDefault();
            SelectedCountHook = CountHooks.FirstOrDefault();
            SelectedRecentGenerators = NumberRecentGenerators.FirstOrDefault();
            ConsiderDateMatch = false;
            StartDate = _allGameMatch.LastOrDefault().DateTimeMatch;
            EndDate = DateTime.Now;
        }

        #endregion

        #region Получение данных для фильтрации

        private void GetFilterData()
        {
            GetKillers();
            GetKillerPlatforms();
            GetKillerOfferings();
            GetPlayerAssociations();
            GetGameMods();
            GetGameEvents();
            GetMaps();
            GetMeasurement();
            GetPatches();
            GetWhoPlacedMap();
            SelectedKillerWinOrLoss = KillerWinOrLoss.FirstOrDefault();
            SelectedCountKill = CountKills.FirstOrDefault();
            SelectedCountHook = CountHooks.FirstOrDefault();
            SelectedRecentGenerators = NumberRecentGenerators.FirstOrDefault();
        }

        private void GetKillers()
        {
            var killers = _dataService.GetAllData<Killer>(x => x.Skip(1));
            Killers.Add(new Killer() { KillerName = _NOT_INCLUDE });
            foreach (var killer in killers) { Killers.Add(killer); }
            SelectedKiller = Killers.FirstOrDefault();
        }

        private void GetKillerPlatforms()
        {
            var platforms = _dataService.GetAllData<Platform>();
            KillerPlatforms.Add(new Platform() { PlatformName = _NOT_INCLUDE });
            foreach (var platform in platforms) { KillerPlatforms.Add(platform); }
            SelectedKillerPlatform = KillerPlatforms.FirstOrDefault();
        }

        private void GetKillerOfferings()
        {
            var killerOfferings = _dataService.GetAllData<Offering>(x => x.Where(x => x.IdRole == 2 | x.IdRole == 5).OrderBy(x => x.IdRarity));
            var survivorOfferings = _dataService.GetAllData<Offering>(x => x.Where(x => x.IdRole == 2 | x.IdRole == 5).OrderBy(x => x.IdRarity));

            KillerOfferings.Add(new Offering() { OfferingName = _NOT_INCLUDE });
            SurvivorOfferings.Add(new Offering() { OfferingName = _NOT_INCLUDE });

            foreach (var offering in killerOfferings) { KillerOfferings.Add(offering); }
            foreach (var offering in survivorOfferings) { SurvivorOfferings.Add(offering); }

            SelectedKillerOffering = KillerOfferings.FirstOrDefault();
        }

        private void GetKillerAddons()
        {
            KillerAddons.Clear();

            var killerAddons = SelectedKiller.KillerName switch
            {
                _NOT_INCLUDE => _dataService.GetAllData<KillerAddon>(x => x.Where(x => x.AddonName == _NOT_INCLUDE)),
                _ => _dataService.GetAllData<KillerAddon>(x => x.Where(x => x.IdKiller == SelectedKiller.IdKiller).OrderBy(x => x.IdRarity))
            };
            KillerAddons.Add(new KillerAddon() { AddonName = _NOT_INCLUDE });
            foreach (var killerAddon in killerAddons)
            {
                KillerAddons.Add(killerAddon);
            }

            SelectedFirstKillerAddon = KillerAddons.FirstOrDefault();
            SelectedSecondKillerAddon = KillerAddons.FirstOrDefault();
        }

        private void GetPlayerAssociations()
        {
            var playerAssociations = _dataService.GetAllData<PlayerAssociation>();

            KillerPlayerAssociations.Add(new PlayerAssociation() { PlayerAssociationName = _NOT_INCLUDE });
            SurvivorPlayerAssociations.Add(new PlayerAssociation() { PlayerAssociationName = _NOT_INCLUDE });

            foreach (var playerAssociation in playerAssociations.Where(x => x.IdPlayerAssociation == 1 | x.IdPlayerAssociation == 3)) { KillerPlayerAssociations.Add(playerAssociation); }
            foreach (var playerAssociation in playerAssociations) { SurvivorPlayerAssociations.Add(playerAssociation); }
        }

        private void GetGameMods()
        {
            var gameMods = _dataService.GetAllData<GameMode>();
            GameMods.Add(new GameMode() { GameModeName = _NOT_INCLUDE });
            foreach (var gameMod in gameMods) { GameMods.Add(gameMod); }
            SelectedGameMode = GameMods.FirstOrDefault();
        }

        private void GetGameEvents()
        {
            var gameEvents = _dataService.GetAllData<GameEvent>();
            GameEvents.Add(new GameEvent() { GameEventName = _NOT_INCLUDE });
            foreach (var gameEvent in gameEvents) { GameEvents.Add(gameEvent); }
            SelectedGameEvent = GameEvents.FirstOrDefault();
        }

        private void GetMaps()
        {
            var maps = _dataService.GetAllData<Map>();
            Maps.Add(new Map() { MapName = _NOT_INCLUDE });
            foreach (var map in maps) { Maps.Add(map); }
            SelectedMap = Maps.FirstOrDefault();
        }

        private void GetMeasurement()
        {
            var measurements = _dataService.GetAllData<Measurement>();
            Measurements.Add(new Measurement() { MeasurementName = _NOT_INCLUDE });
            foreach (var measurement in measurements)
            {
                Measurements.Add(measurement);
            }
            SelectedMeasurement = Measurements.FirstOrDefault();
        }

        private void GetPatches()
        {
            var patches = _dataService.GetAllData<Patch>();
            Patches.Add(new Patch() { PatchNumber = _NOT_INCLUDE });
            foreach (var patch in patches) { Patches.Add(patch); }
            SelectedPatch = Patches.FirstOrDefault();
        }

        private void GetWhoPlacedMap()
        {
            var whoPlaceMap = _dataService.GetAllData<WhoPlacedMap>();
            WhoPleaseMaps.Add(new WhoPlacedMap() { WhoPlacedMapName = _NOT_INCLUDE });
            WhoPleaseMapWon.Add(new WhoPlacedMap() { WhoPlacedMapName = _NOT_INCLUDE });

            WhoPleaseMaps.AddRange(whoPlaceMap);
            WhoPleaseMapWon.AddRange(whoPlaceMap);

            SelectedWhoPleaseMap = WhoPleaseMaps.FirstOrDefault();
            SelectedWhoPleaseMapWin = WhoPleaseMapWon.FirstOrDefault();
        }

        #endregion 

        #endregion

        #region Пагинация

        private const int _ITEMS_PER_PAGE = 5;

        #region Свойства

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();

            }
        }

        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Команды

        private RelayCommand _nextPageCommand;
        public RelayCommand NextPageCommand => _nextPageCommand ??= new RelayCommand(obj =>
        {
            CurrentPage++;
            LoadGameStatistics();
        });

        private RelayCommand _previousPageCommand;
        public RelayCommand PreviousPageCommand => _previousPageCommand ??= new RelayCommand(obj =>
        {
            CurrentPage--;
            LoadGameStatistics();
        });

        #endregion

        #region Методы

        private void CalculateTotalPages(int totalItems)
        {
            TotalPages = (int)Math.Ceiling((double)totalItems / _ITEMS_PER_PAGE);
        }

        #endregion

        #endregion

        #region Методы получение данных

        private void GetGameStatisticData()
        {
            new Thread(obj =>
            {
                var match = _dataService.GetAllData<GameStatistic>(x => x
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

                .Where(x => x.IdKillerNavigation.IdAssociation == 1)
                .OrderByDescending(x => x.DateTimeMatch));

                App.Current.Dispatcher.Invoke(() =>
                {
                    _allGameMatch.Clear();
                    _allGameMatch.AddRange(match);

                    CalculateTotalPages(match.Count());
                    ApplySorting();
                    ApplyFilter();
                    LoadGameStatistics();
                });

            }).Start();
        }

        private void LoadGameStatistics()
        {
            var items = _filteredMatch.Skip((_currentPage - 1) * _ITEMS_PER_PAGE).Take(_ITEMS_PER_PAGE);

            GameMatchList.Clear();
            foreach (var item in items)
            {
                GameMatchList.Add(item);
            }
        }

        private void ApplySorting()
        {
            if (SelectedSortingMatch != _NOT_INCLUDE)
            {
                _allGameMatch = SelectedSortingMatch switch
                {
                    "От старой дате к новой" => _allGameMatch.OrderBy(x => x.DateTimeMatch).ToList(),
                    "От новой дате к старой" => _allGameMatch.OrderByDescending(x => x.DateTimeMatch).ToList(),
                    "Убыванию длинны матча" => _allGameMatch.OrderByDescending(x => x.GameTimeMatch).ToList(),
                    "Возрастанию длинны матча" => _allGameMatch.OrderBy(x => x.GameTimeMatch).ToList(),
                };
            }
        }

        private void ApplyFilter()
        {
            var filters = new List<Func<GameStatistic, bool>>();
            IEnumerable<GameStatistic> filteredMatch;

            if (SelectedKiller.KillerName != _NOT_INCLUDE)
                filters.Add(x => x.IdKillerNavigation.IdKillerNavigation.IdKiller == SelectedKiller.IdKiller);

            if (SelectedFirstKillerAddon.AddonName != _NOT_INCLUDE && SelectedSecondKillerAddon.AddonName != _NOT_INCLUDE)
                filters.Add(x =>
                            x.IdKillerNavigation.IdAddon1 == SelectedFirstKillerAddon.IdKillerAddon && x.IdKillerNavigation.IdAddon2 == SelectedSecondKillerAddon.IdKillerAddon ||
                            x.IdKillerNavigation.IdAddon1 == SelectedSecondKillerAddon.IdKillerAddon && x.IdKillerNavigation.IdAddon2 == SelectedFirstKillerAddon.IdKillerAddon);

            if (SelectedMeasurement.MeasurementName != _NOT_INCLUDE)
                filters.Add(x => x.IdMapNavigation.IdMeasurement == SelectedMeasurement.IdMeasurement);

            if (SelectedMap.MapName != _NOT_INCLUDE)
                filters.Add(x => x.IdMap == SelectedMap.IdMap);

            if (SelectedGameMode.GameModeName != _NOT_INCLUDE)
                filters.Add(x => x.IdGameMode == SelectedGameMode.IdGameMode);

            if (SelectedGameEvent.GameEventName != _NOT_INCLUDE)
                filters.Add(x => x.IdGameEvent == SelectedGameEvent.IdGameEvent);

            if (SelectedPatch.PatchNumber != _NOT_INCLUDE)
                filters.Add(x => x.IdPatch == SelectedPatch.IdPatch);

            if (SelectedKillerOffering.OfferingName != _NOT_INCLUDE)
                filters.Add(x => x.IdKillerNavigation.IdKillerOfferingNavigation.OfferingName == SelectedKillerOffering.OfferingName);
            //filters.Add(x => x.IdKillerNavigation.IdKillerOfferingNavigation.OfferingName.Equals(SelectedKillerOffering.OfferingName));

            if (SelectedKillerWinOrLoss != _NOT_INCLUDE)
            {
                filters.Add(SelectedKillerWinOrLoss switch
                {
                    "Победа" => x => x.CountKills is 3 or 4,
                    "Ничья" => x => x.CountKills == 2,
                    "Проигрыш" => x => x.CountKills is 1 or 0,
                    _ => _ => true
                });
            }

            if (SelectedWhoPleaseMap.WhoPlacedMapName != _NOT_INCLUDE)
                filters.Add(x => x.IdWhoPlacedMap == SelectedWhoPleaseMap.IdWhoPlacedMap);

            if (SelectedWhoPleaseMapWin.WhoPlacedMapName != _NOT_INCLUDE)
                filters.Add(x => x.IdWhoPlacedMapWin == SelectedWhoPleaseMapWin.IdWhoPlacedMap);

            if (SelectedCountKill != _NOT_INCLUDE)
                filters.Add(x => x.CountKills == int.Parse(SelectedCountKill));

            if (SelectedCountHook != _NOT_INCLUDE)
                filters.Add(x => x.CountHooks == int.Parse(SelectedCountHook));

            if (SelectedRecentGenerators != _NOT_INCLUDE)
                filters.Add(x => x.NumberRecentGenerators == int.Parse(SelectedRecentGenerators));

            if (SelectedKillerPlatform.PlatformName != _NOT_INCLUDE)
                filters.Add(x => x.IdKillerNavigation.IdPlatform == SelectedKillerPlatform.IdPlatform);

            if (ConsiderKillerPrestige != false)
                filters.Add(x => x.IdKillerNavigation.Prestige >= FirstNumberKillerPrestige && x.IdKillerNavigation.Prestige <= SecondNumberKillerPrestige);

            if (ConsiderKillerScore != false)
                filters.Add(x => x.IdKillerNavigation.KillerAccount >= StartScoreKiller && x.IdKillerNavigation.KillerAccount <= EndScoreKiller);

            if (ConsiderDateMatch != false)
                filters.Add(x => x.DateTimeMatch >= StartDate && x.DateTimeMatch <= EndDate);

            filteredMatch = filters.Aggregate(_allGameMatch.AsEnumerable(), (current, filter) => current.Where(filter));

            CalculateTotalPages(filteredMatch.Count());
            _filteredMatch.Clear();
            _filteredMatch.AddRange(filteredMatch);
        }

        #endregion
    }
}