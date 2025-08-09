using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Enums;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.UseCases.Abstraction.GameEventCase;
using DBDAnalytics.Application.UseCases.Abstraction.GameModeCase;
using DBDAnalytics.Application.UseCases.Abstraction.GameStatisticCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;
using DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase;
using DBDAnalytics.Application.UseCases.Abstraction.PatchCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;

namespace DBDAnalytics.WPF.ViewModels.PageVM
{
    internal class DashBoardVM : BaseVM, IUpdatable
    {
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IWindowNavigationService _windowNavigationService;

        private readonly IGetGameStatisticKillerViewingUseCase _getGameStatisticKillerViewingUseCase;
        private readonly IGetGameStatisticSurvivorViewingUseCase _getGameStatisticSurvivorViewingUseCase;
        private readonly IGetKillerUseCase _getKillerUseCase;
        private readonly IGetSurvivorUseCase _getSurvivorUseCase;
        private readonly IGetGameModeUseCase _getGameModeUseCase;
        private readonly IGetGameEventUseCase _getGameEventUseCase;
        private readonly IGetPatchUseCase _getPatchUseCase;
        private readonly IGetMatchAttributeUseCase _getMatchAttributeUseCase;

        public DashBoardVM(IPageNavigationService pageNavigationService,
                           IWindowNavigationService windowNavigationService,
                           IGetGameStatisticKillerViewingUseCase getGameStatisticKillerViewingUseCase,
                           IGetGameStatisticSurvivorViewingUseCase getGameStatisticSurvivorViewingUseCase,
                           IGetKillerUseCase getKillerUseCase,
                           IGetSurvivorUseCase getSurvivorUseCase,
                           IGetGameModeUseCase getGameModeUseCase,
                           IGetGameEventUseCase getGameEventUseCase,
                           IGetPatchUseCase getPatchUseCase,
                           IGetMatchAttributeUseCase getMatchAttributeUseCase) : base(windowNavigationService, pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;
            _windowNavigationService = windowNavigationService;
            _getGameStatisticKillerViewingUseCase = getGameStatisticKillerViewingUseCase;
            _getGameStatisticSurvivorViewingUseCase = getGameStatisticSurvivorViewingUseCase;
            _getKillerUseCase = getKillerUseCase;
            _getSurvivorUseCase = getSurvivorUseCase;
            _getGameModeUseCase = getGameModeUseCase;
            _getGameEventUseCase = getGameEventUseCase;
            _getPatchUseCase = getPatchUseCase;
            _getMatchAttributeUseCase = getMatchAttributeUseCase;

            GetFilteredData();

            SetDefaultKillerFilter();
            SetDefaultSurvivorFilter();

            GetKillerMatches();
            GetSurvivorMatches();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            if (typeParameter == TypeParameter.AddAndNotification)
            {
                if (parameter is GameStatisticKillerViewingDTO killerMatch)
                {
                    KillerMatches.Insert(0, killerMatch);
                }
                if (parameter is GameStatisticSurvivorViewingDTO survivorMatch)
                {
                    SurvivorMatches.Insert(0, survivorMatch);
                }
            }
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Коллекция : Список матчей

        public ObservableCollection<GameStatisticKillerViewingDTO> KillerMatches { get; set; } = [];

        public ObservableCollection<GameStatisticSurvivorViewingDTO> SurvivorMatches { get; set; } = [];

        #endregion

        #region Коллекции : Списки данных для фильтрацц

        public ObservableCollection<KillerDTO> Killers { get; set; } = [];

        public ObservableCollection<SurvivorDTO> Survivors { get; set; } = [];

        public ObservableCollection<GameModeDTO> GameModes { get; set; } = [];

        public ObservableCollection<GameEventDTO> GameEvents { get; set; } = [];

        public ObservableCollection<PatchDTO> Patches { get; set; } = [];

        public ObservableCollection<MatchAttributeDTO> MatchAttributes { get; set; } = [];

        #endregion

        #region Свойства : Выбор матча

        private GameStatisticKillerViewingDTO _selectedKillerMatch;
        public GameStatisticKillerViewingDTO SelectedKillerMatch
        {
            get => _selectedKillerMatch;
            set
            {
                _selectedKillerMatch = value;
                OnPropertyChanged();
            }
        }

        private GameStatisticSurvivorViewingDTO _selectedSurvivorMatch;
        public GameStatisticSurvivorViewingDTO SelectedSurvivorMatch
        {
            get => _selectedSurvivorMatch;
            set
            {
                _selectedSurvivorMatch = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойство : Открытие POPUP

        private bool _isOpenKillerFilter = false;
        public bool IsOpenKillerFilter
        {
            get => _isOpenKillerFilter;
            set
            {
                _isOpenKillerFilter = value;
                OnPropertyChanged();
            }
        }

        private bool _isOpenSurvivorFilter = false;
        public bool IsOpenSurvivorFilter
        {
            get => _isOpenSurvivorFilter;
            set
            {
                _isOpenSurvivorFilter = value;
                OnPropertyChanged();
            }
        }
       
        #endregion

        #region Свойства : Выбор значений для фильтрации Киллера

        private KillerDTO _selectedKillerFilter;
        public KillerDTO SelectedKillerFilter
        {
            get => _selectedKillerFilter;
            set
            {
                _selectedKillerFilter = value;
                OnPropertyChanged();
            }
        } 
        
        private int _selectedKillerFilterIndex;
        public int SelectedKillerFilterIndex
        {
            get => _selectedKillerFilterIndex;
            set
            {
                if (value >= 0 && value < Killers.Count)
                {
                    _selectedKillerFilterIndex = value;
                    SelectedKillerFilter = Killers[value];
                    OnPropertyChanged();
                }
                OnPropertyChanged();
            }
        }

        private GameModeDTO _selectedGameModeKillerFilter;
        public GameModeDTO SelectedGameModeKillerFilter
        {
            get => _selectedGameModeKillerFilter;
            set
            {
                _selectedGameModeKillerFilter = value;
                OnPropertyChanged();
            }
        }        
        
        private GameEventDTO _selectedGameEventKillerFilter;
        public GameEventDTO SelectedGameEventKillerFilter
        {
            get => _selectedGameEventKillerFilter;
            set
            {
                _selectedGameEventKillerFilter = value;
                OnPropertyChanged();
            }
        }
        
        private PatchDTO _selectedPatchKillerFilter;
        public PatchDTO SelectedPatchKillerFilter
        {
            get => _selectedPatchKillerFilter;
            set
            {
                _selectedPatchKillerFilter = value;
                OnPropertyChanged();
            }
        }

        private MatchAttributeDTO _selectedMatchAttributeKillerFilter;
        public MatchAttributeDTO SelectedMatchAttributeKillerFilter
        {
            get => _selectedMatchAttributeKillerFilter;
            set
            {
                _selectedMatchAttributeKillerFilter = value;
                OnPropertyChanged();
            }
        }

        private bool _isConsiderDateTimeKillerFilter;
        public bool IsConsiderDateTimeKillerFilter
        {
            get => _isConsiderDateTimeKillerFilter;
            set
            {
                _isConsiderDateTimeKillerFilter = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startTimeKillerFilter;
        public DateTime? StartTimeKillerFilter
        {
            get => _startTimeKillerFilter;
            set
            {
                _startTimeKillerFilter = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endTimeKillerFilter;
        public DateTime? EndTimeKillerFilter
        {
            get => _endTimeKillerFilter;
            set
            {
                _endTimeKillerFilter = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Выбор значений для фильтрации Выжившего

        private KillerDTO _selectedOpponentKillerSurvivorFilter;
        public KillerDTO SelectedOpponentKillerSurvivorFilter
        {
            get => _selectedOpponentKillerSurvivorFilter;
            set
            {
                _selectedOpponentKillerSurvivorFilter = value;
                OnPropertyChanged();
            }
        }        
        
        private SurvivorDTO _selectedSurvivorFilter;
        public SurvivorDTO SelectedSurvivorFilter
        {
            get => _selectedSurvivorFilter;
            set
            {
                _selectedSurvivorFilter = value;
                OnPropertyChanged();
            }
        }

        private int _selectedSurvivorFilterIndex;
        public int SelectedSurvivorFilterIndex
        {
            get => _selectedSurvivorFilterIndex;
            set
            {
                if (value >= 0 && value < Survivors.Count)
                {
                    _selectedSurvivorFilterIndex = value;
                    SelectedSurvivorFilter = Survivors[value];
                    OnPropertyChanged();
                }
                OnPropertyChanged();
            }
        }

        private GameModeDTO _selectedGameModeSurvivorFilter;
        public GameModeDTO SelectedGameModeSurvivorFilter
        {
            get => _selectedGameModeSurvivorFilter;
            set
            {
                _selectedGameModeSurvivorFilter = value;
                OnPropertyChanged();
            }
        }

        private GameEventDTO _selectedGameEventSurvivorFilter;
        public GameEventDTO SelectedGameEventSurvivorFilter
        {
            get => _selectedGameEventSurvivorFilter;
            set
            {
                _selectedGameEventSurvivorFilter = value;
                OnPropertyChanged();
            }
        }

        private PatchDTO _selectedPatchSurvivorFilter;
        public PatchDTO SelectedPatchSurvivorFilter
        {
            get => _selectedPatchSurvivorFilter;
            set
            {
                _selectedPatchSurvivorFilter = value;
                OnPropertyChanged();
            }
        }

        private MatchAttributeDTO _selectedMatchAttributeSurvivorFilter;
        public MatchAttributeDTO SelectedMatchAttributeSurvivorFilter
        {
            get => _selectedMatchAttributeSurvivorFilter;
            set
            {
                _selectedMatchAttributeSurvivorFilter = value;
                OnPropertyChanged();
            }
        }

        private bool _isConsiderDateTimeSurvivorFilter;
        public bool IsConsiderDateTimeSurvivorFilter
        {
            get => _isConsiderDateTimeSurvivorFilter;
            set
            {
                _isConsiderDateTimeSurvivorFilter = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startTimeSurvivorFilter;
        public DateTime? StartTimeSurvivorFilter
        {
            get => _startTimeSurvivorFilter;
            set
            {
                _startTimeSurvivorFilter = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endTimeSurvivorFilter;
        public DateTime? EndTimeSurvivorFilter
        {
            get => _endTimeSurvivorFilter;
            set
            {
                _endTimeSurvivorFilter = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        /*--Управление страницей--*/

        #region Закрытие страницы

        private RelayCommand _closePageCommand;
        public RelayCommand ClosePageCommand { get => _closePageCommand ??= new(obj =>{ _pageNavigationService.Close(PageName.DashBoard, FrameName.MainFrame); }); }

        #endregion

        /*--Фильтрация--*/

        #region Открытие Popup

        private RelayCommand _openKillerFilterPopupCommand;
        public RelayCommand OpenKillerFilterPopupCommand { get => _openKillerFilterPopupCommand ??= new(obj => { IsOpenKillerFilter = true; }); }

        private RelayCommand _openSurvivorFilterPopupCommand;
        public RelayCommand OpenSurvivorFilterPopupCommand { get => _openSurvivorFilterPopupCommand ??= new(obj => { IsOpenSurvivorFilter = true; }); }

        #endregion

        #region Применение фильтра Киллеров и Выживших

        private RelayCommand _applyKillerFilterCommand;
        public RelayCommand ApplyKillerFilterCommand 
        { 
            get => _applyKillerFilterCommand ??= new(obj => 
            { 
                GetKillerMatches();

                IsOpenKillerFilter = false;
            }); 
        }

        private RelayCommand _applySurvivorFilterCommand;
        public RelayCommand ApplySurvivorFilterCommand
        { 
            get => _applySurvivorFilterCommand ??= new(obj => 
            {
                GetSurvivorMatches();

                IsOpenSurvivorFilter = false;
            }); 
        }

        #endregion

        #region Сброс фильтров до дефолтных значений у Киллеров и Выживших

        private RelayCommand _resetKillerFilterCommand;
        public RelayCommand ResetKillerFilterCommand 
        { 
            get => _resetKillerFilterCommand ??= new(obj => 
            { 
                SetDefaultKillerFilter();
                GetKillerMatches();
            }); 
        }

        private RelayCommand _resetSurvivorFilterCommand;
        public RelayCommand ResetSurvivorFilterCommand 
        { 
            get => _resetSurvivorFilterCommand ??= new(obj => 
            { 
                SetDefaultSurvivorFilter();
                GetSurvivorMatches();
            }); 
        }

        #endregion

        #region Переключение индиексов в списке киллеров

        private RelayCommand _nextKillerCommand;
        public RelayCommand NextKillerFilterCommand
        {
            get => _nextKillerCommand ??= new(obj =>
            {
                SelectedKillerFilterIndex++;
                GetKillerMatches();
            });
        }

        private RelayCommand _previousKillerCommand;
        public RelayCommand PreviousKillerFilterCommand
        {
            get => _previousKillerCommand ??= new(obj =>
            {
                SelectedKillerFilterIndex--;
                GetKillerMatches();
            });
        }

        #endregion

        #region Переключение индиексов в списке выживших

        private RelayCommand _nextSurvivorCommand;
        public RelayCommand NextSurvivorFilterCommand
        {
            get => _nextSurvivorCommand ??= new(obj =>
            {
                SelectedSurvivorFilterIndex++;
                GetSurvivorMatches();
            });
        }

        private RelayCommand _previousSurvivorCommand;
        public RelayCommand PreviousSurvivorFilterCommand
        {
            get => _previousSurvivorCommand ??= new(obj =>
            {
                SelectedSurvivorFilterIndex--;
                GetSurvivorMatches();
            });
        }

        #endregion

        /*--Управление списком \ элементами списка--*/

        #region Детализация матча

        private RelayCommand _showMatchDetailsCommand;
        public RelayCommand ShowMatchDetailsCommand => _showMatchDetailsCommand ??= new RelayCommand(ShowMatchDetails);

        #endregion

        #region Расчеты по списку матчей 

        private RelayCommand _killerDetailsForKillersCommand;
        public RelayCommand KillerDetailsForKillersCommand
        {
            get => _killerDetailsForKillersCommand ??= new(obj =>
            {
                KillersDetailsForKillers();
            });
        }

        private RelayCommand _killerDetailsForSurvivorsCommand;
        public RelayCommand KillerDetailsForSurvivorsCommand
        {
            get => _killerDetailsForSurvivorsCommand ??= new(obj =>
            {
                KillersDetailsForSurvivors();
            });
        }

        #endregion

        #region Сортировка

        private RelayCommand _sortKillerMatchCommand;
        public RelayCommand SortKillerMatchCommand => _sortKillerMatchCommand ??= new RelayCommand(SortKillerMatch);

        private RelayCommand _sortSurvivorMatchCommand;
        public RelayCommand SortSurvivorMatchCommand => _sortSurvivorMatchCommand ??= new RelayCommand(SortSurvivorMatch);

        #endregion

        #region Фильтрация

        private RelayCommand _filterKillerMatchCommand;
        public RelayCommand FilterKillerMatchCommand => _filterKillerMatchCommand ??= new RelayCommand(FilterKillerMatch);

        private RelayCommand _filterSurvivorMatchCommand;
        public RelayCommand FilterSurvivorMatchCommand => _filterSurvivorMatchCommand ??= new RelayCommand(FilterSurvivorMatch);

        #endregion

        #region Реверс списков

        private RelayCommand _reversKillerMatches;
        public RelayCommand ReversKillerMatches { get => _reversKillerMatches ??= new(obj => { KillerMatches.ReverseInPlace(); }); }

        private RelayCommand _reversSurvivorMatches;
        public RelayCommand ReversSurvivorMatches { get => _reversSurvivorMatches ??= new(obj => { SurvivorMatches.ReverseInPlace(); }); }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        /*--Фильтрация получаемых данных--*/

        #region Создание фильтра | Установка дефолтных значений

        private GameStatisticKillerFilterDTO GetObjectKillerFilter() => new GameStatisticKillerFilterDTO
        {
            IdKiller = SelectedKillerFilter?.IdKiller == 0 ? null : SelectedKillerFilter.IdKiller,
            IdGameMode = SelectedGameModeKillerFilter?.IdGameMode == 0 ? null : SelectedGameModeKillerFilter.IdGameMode,
            IdGameEvent = SelectedGameEventKillerFilter?.IdGameEvent == 0 ? null : _selectedGameEventKillerFilter.IdGameEvent,

            IsConsiderDateTime = this.IsConsiderDateTimeKillerFilter,
            StartTime = this.StartTimeKillerFilter,
            EndTime = this.EndTimeKillerFilter,

            IdPatch = SelectedPatchKillerFilter?.IdPatch == 0 ? null : SelectedPatchKillerFilter.IdPatch,
            IdMatchAttribute = SelectedMatchAttributeKillerFilter?.IdMatchAttribute == 0 ? null : SelectedMatchAttributeKillerFilter.IdMatchAttribute,
        };

        private void SetDefaultKillerFilter()
        {
            SelectedKillerFilter = Killers.FirstOrDefault();
            SelectedGameModeKillerFilter = GameModes.FirstOrDefault();
            SelectedGameEventKillerFilter = GameEvents.FirstOrDefault();

            IsConsiderDateTimeKillerFilter = true;
            var startDate = _getGameStatisticKillerViewingUseCase.GetLastDateMatch();
            StartTimeKillerFilter = startDate;
            EndTimeKillerFilter = startDate.Value.AddDays(-7);

            SelectedPatchKillerFilter = Patches.FirstOrDefault();
            SelectedMatchAttributeKillerFilter = MatchAttributes.FirstOrDefault();
        }

        private GameStatisticSurvivorFilterDTO GetObjectSurvivorFilter() => new GameStatisticSurvivorFilterDTO
        {
            IdOpponentKiller = SelectedOpponentKillerSurvivorFilter?.IdKiller == 0 ? null : SelectedOpponentKillerSurvivorFilter.IdKiller,
            IdSurvivor = SelectedSurvivorFilter?.IdSurvivor == 0 ? null : SelectedSurvivorFilter.IdSurvivor,

            IdGameMode = SelectedGameModeSurvivorFilter?.IdGameMode == 0 ? null : SelectedGameModeSurvivorFilter.IdGameMode,
            IdGameEvent = SelectedGameEventSurvivorFilter?.IdGameEvent == 0 ? null : _selectedGameEventSurvivorFilter.IdGameEvent,

            IsConsiderDateTime = this.IsConsiderDateTimeSurvivorFilter,
            StartTime = this.StartTimeSurvivorFilter,
            EndTime = this.EndTimeSurvivorFilter,

            IdPatch = SelectedPatchSurvivorFilter?.IdPatch == 0 ? null : SelectedPatchSurvivorFilter.IdPatch,
            IdMatchAttribute = SelectedMatchAttributeSurvivorFilter?.IdMatchAttribute == 0 ? null : SelectedMatchAttributeSurvivorFilter.IdMatchAttribute,
        };

        private void SetDefaultSurvivorFilter()
        {
            SelectedOpponentKillerSurvivorFilter = Killers.FirstOrDefault();
            SelectedSurvivorFilter = Survivors.FirstOrDefault();

            SelectedGameModeSurvivorFilter = GameModes.FirstOrDefault();
            SelectedGameEventSurvivorFilter = GameEvents.FirstOrDefault();

            IsConsiderDateTimeSurvivorFilter = true;
            var startDate = _getGameStatisticSurvivorViewingUseCase.GetLastDateMatch();
            StartTimeSurvivorFilter = startDate;
            EndTimeSurvivorFilter = startDate.Value.AddDays(-7);

            SelectedPatchSurvivorFilter = Patches.FirstOrDefault();
            SelectedMatchAttributeSurvivorFilter = MatchAttributes.FirstOrDefault();
        }

        #endregion

        #region Получение списков матчей

        private async void GetKillerMatches()
        {
            KillerMatches.Clear();

            var filter = GetObjectKillerFilter();

            foreach (var item in await _getGameStatisticKillerViewingUseCase.GetKillerViewsFilteredAsync(filter))
            {
                KillerMatches.Add(item);
            }
        }        
        
        private async void GetSurvivorMatches()
        {
            SurvivorMatches.Clear();

            var filter = GetObjectSurvivorFilter();

            foreach (var item in await _getGameStatisticSurvivorViewingUseCase.GetSurvivorViewsAsync(filter))
            {
                SurvivorMatches.Add(item);
            }
        }

        #endregion

        #region Получение данных для фильтрации

        private void GetFilteredData()
        {
            GetKillerFilter();
            GetSurvivorFilter();
            GetGameModeFilter();
            GetGameEventFilter();
            GetPatchFilter();
            GetMatchAttributeFilter();
        }

        private void GetKillerFilter()
        {
            var killers = _getKillerUseCase.GetAll();

            Killers.Add(new KillerDTO { IdKiller = 0, KillerName = "Не учитывать" });

            foreach (var item in killers.Skip(1))
                Killers.Add(item);
        }        
        
        private void GetSurvivorFilter()
        {
            var survivors = _getSurvivorUseCase.GetAll();

            Survivors.Add(new SurvivorDTO { IdSurvivor = 0, SurvivorName = "Не учитывать" });

            foreach (var item in survivors.Skip(1))
                Survivors.Add(item);
        }

        private void GetGameModeFilter()
        {
            var gameModes =  _getGameModeUseCase.GetAll();

            GameModes.Add(new GameModeDTO { IdGameMode = 0, GameModeName = "Не учитывать" });

            foreach (var item in gameModes)
                GameModes.Add(item);
        }

        private void GetGameEventFilter()
        {
            var gameModes = _getGameEventUseCase.GetAll();

            GameEvents.Add(new GameEventDTO { IdGameEvent = 0, GameEventName = "Не учитывать" });

            foreach (var item in gameModes)
                GameEvents.Add(item);
        }

        private void GetPatchFilter()
        {
            var gameModes = _getPatchUseCase.GetAll();

            Patches.Add(new PatchDTO { IdPatch = 0, PatchNumber = "Не учитывать" });

            foreach (var item in gameModes)
                Patches.Add(item);
        }

        private void GetMatchAttributeFilter()
        {
            var matchAttribute = _getMatchAttributeUseCase.GetAll(false);

            MatchAttributes.Add(new MatchAttributeDTO { IdMatchAttribute = 0, AttributeName = "Не учитывать" });

            foreach (var item in matchAttribute)
                MatchAttributes.Add(item);
        }

        #endregion

        /*--Управление списком \ элементами списка--*/

        #region Детализация матча

        private void ShowMatchDetails(object parameter)
        {
            if (parameter is GameStatisticKillerViewingDTO selectedKillerMatch)
            {
                _windowNavigationService.OpenWindow(WindowName.PreviewMatch, selectedKillerMatch.IdGameStatistic);
            }
            if (parameter is GameStatisticSurvivorViewingDTO selectedSurvivorMatch)
            {
                _windowNavigationService.OpenWindow(WindowName.PreviewMatch, selectedSurvivorMatch.IdGameStatistic);
            }
        }

        #endregion

        #region Расчеты по списку матчей 

        private void KillersDetailsForKillers()
        {
            List<int> ids = [];

            ids = KillerMatches.Select(x => x.IdGameStatistic).ToList();

            NotificationTransmittingValue(PageName.KillerDetails, FrameName.MainFrame, ids, TypeParameter.Killers);
        }

        private void KillersDetailsForSurvivors()
        {
            List<int> ids = [];

            ids = SurvivorMatches.Select(x => x.IdGameStatistic).ToList();

            NotificationTransmittingValue(PageName.KillerDetails, FrameName.MainFrame, ids, TypeParameter.Killers);
        }

        #endregion

        #region Применение сортировки

        private void SortKillerMatch(object parameter)
        {
            if (parameter is By sortParameter)
            {
                Action action = sortParameter switch
                {
                    By.DateTime         => () => KillerMatches = new ObservableCollection<GameStatisticKillerViewingDTO>(KillerMatches.OrderByDescending(x => x.DateMatch)),
                    By.Killer           => () => KillerMatches = new ObservableCollection<GameStatisticKillerViewingDTO>(KillerMatches.OrderBy(x => x.IdKiller)),
                    By.Map              => () => KillerMatches = new ObservableCollection<GameStatisticKillerViewingDTO>(KillerMatches.OrderBy(x => x.MapName)),
                    By.DurationMatch    => () => KillerMatches = new ObservableCollection<GameStatisticKillerViewingDTO>(KillerMatches.OrderByDescending(x => TimeOnly.ParseExact(x.MatchTime, "HH:mm:ss", CultureInfo.InvariantCulture))),

                    By.CountKills       => () => KillerMatches = new ObservableCollection<GameStatisticKillerViewingDTO>(KillerMatches.OrderByDescending(x => x.CountKill)),
                    By.CountHooks       => () => KillerMatches = new ObservableCollection<GameStatisticKillerViewingDTO>(KillerMatches.OrderByDescending(x => x.CountHook)),
                    By.RecentGenerators => () => KillerMatches = new ObservableCollection<GameStatisticKillerViewingDTO>(KillerMatches.OrderByDescending(x => x.CountRecentGenerator)),

                    By.Win              => () => KillerMatches = new ObservableCollection<GameStatisticKillerViewingDTO>(KillerMatches.OrderByDescending(x => x.CountKill > 2).ThenByDescending(x => x.CountKill == 2)),
                    By.Draw             => () => KillerMatches = new ObservableCollection<GameStatisticKillerViewingDTO>(KillerMatches.OrderByDescending(x => x.CountKill == 2).ThenByDescending(x => x.CountKill < 2)),
                    By.Lose             => () => KillerMatches = new ObservableCollection<GameStatisticKillerViewingDTO>(KillerMatches.OrderByDescending(x => x.CountKill < 2).ThenByDescending(x => x.CountKill == 2)),
                    _ => () => throw new Exception("Такого вида сортировки нету.")
                };
                action?.Invoke();

                OnPropertyChanged(nameof(KillerMatches));
            }
        }

        private void SortSurvivorMatch(object parameter)
        {
            if (parameter is By sortParameter)
            {
                Action action = sortParameter switch
                {
                    By.DateTime         => () => SurvivorMatches = new ObservableCollection<GameStatisticSurvivorViewingDTO>(SurvivorMatches.OrderByDescending(x => x.DateMatch)),
                    By.Survivor         => () => SurvivorMatches = new ObservableCollection<GameStatisticSurvivorViewingDTO>(SurvivorMatches.OrderBy(x => x.IdSurvivor)),
                    By.Map              => () => SurvivorMatches = new ObservableCollection<GameStatisticSurvivorViewingDTO>(SurvivorMatches.OrderBy(x => x.MapName)),
                    By.DurationMatch    => () => SurvivorMatches = new ObservableCollection<GameStatisticSurvivorViewingDTO>(SurvivorMatches.OrderByDescending(x => TimeOnly.ParseExact(x.MatchTime, "HH:mm:ss", CultureInfo.InvariantCulture))),

                    By.CountKills       => () => SurvivorMatches = new ObservableCollection<GameStatisticSurvivorViewingDTO>(SurvivorMatches.OrderByDescending(x => x.CountKill)),
                    By.CountHooks       => () => SurvivorMatches = new ObservableCollection<GameStatisticSurvivorViewingDTO>(SurvivorMatches.OrderByDescending(x => x.CountHook)),
                    By.RecentGenerators => () => SurvivorMatches = new ObservableCollection<GameStatisticSurvivorViewingDTO>(SurvivorMatches.OrderByDescending(x => x.CountRecentGenerator)),
                                                 
                    By.Win              => () => SurvivorMatches = new ObservableCollection<GameStatisticSurvivorViewingDTO>(SurvivorMatches.OrderByDescending(x => x.CountKill > 2).ThenByDescending(x => x.CountKill == 2)),
                    By.Draw             => () => SurvivorMatches = new ObservableCollection<GameStatisticSurvivorViewingDTO>(SurvivorMatches.OrderByDescending(x => x.CountKill == 2).ThenByDescending(x => x.CountKill < 2)),
                    By.Lose             => () => SurvivorMatches = new ObservableCollection<GameStatisticSurvivorViewingDTO>(SurvivorMatches.OrderByDescending(x => x.CountKill < 2).ThenByDescending(x => x.CountKill == 2)),

                    _ => () => throw new Exception("Такого вида сортировки нету.")
                };
                action?.Invoke();

                OnPropertyChanged(nameof(SurvivorMatches));
            }
        }

        #endregion

        #region Применение фильтрации

        private void FilterKillerMatch(object parameter)
        { 
            //Обычный TODO : Заменить MessageBox на кастомное окно или аналог.
            if (SelectedKillerMatch == null)
            {
                MessageBox.Show("Не выбран матч для применение фильтрации");
                return;
            }

            if (parameter is By sortParameter)
            {
                Action action = sortParameter switch
                {
                    By.DateTimeFrom => () =>
                    {
                        StartTimeKillerFilter = SelectedKillerMatch.DateMatch;
                        IsConsiderDateTimeKillerFilter = true;
                        GetKillerMatches();
                    }
                    ,
                    By.DateTimeBefore => () =>
                    {
                        EndTimeKillerFilter = SelectedKillerMatch.DateMatch;
                        IsConsiderDateTimeKillerFilter = true;
                        GetKillerMatches();
                    }
                    ,
                    By.Killer => () =>
                    {
                        SelectedKillerFilter = Killers.FirstOrDefault(x => x.IdKiller == SelectedKillerMatch.IdKiller);
                        GetKillerMatches();
                    }
                    ,
                    _ => () => throw new Exception("Такого вида фильтрации нету.")
                };
                action?.Invoke();

                OnPropertyChanged(nameof(KillerMatches));
            }
        }        
        
        private void FilterSurvivorMatch(object parameter)
        { 
            //Обычный TODO : Заменить MessageBox на кастомное окно или аналог.
            if (SelectedSurvivorMatch == null)
            {
                MessageBox.Show("Не выбран матч для применение фильтрации");
                return;
            }

            if (parameter is By sortParameter)
            {
                Action action = sortParameter switch
                {
                    By.DateTimeFrom => () =>
                    {
                        StartTimeSurvivorFilter = SelectedSurvivorMatch.DateMatch;
                        IsConsiderDateTimeSurvivorFilter = true;
                        GetSurvivorMatches();
                    }
                    ,
                    By.DateTimeBefore => () =>
                    {
                        EndTimeSurvivorFilter = SelectedSurvivorMatch.DateMatch;
                        IsConsiderDateTimeSurvivorFilter = true;
                        GetSurvivorMatches();
                    }
                    ,
                    By.Survivor => () =>
                    {
                        SelectedSurvivorFilter = Survivors.First(x => x.IdSurvivor == SelectedSurvivorMatch.IdSurvivor);
                        GetSurvivorMatches();
                    }
                    ,
                    _ => () => throw new Exception("Такого вида фильтрации нету.")
                };
                action?.Invoke();

                OnPropertyChanged(nameof(SurvivorMatches));
            }
        }

        #endregion

    }
}