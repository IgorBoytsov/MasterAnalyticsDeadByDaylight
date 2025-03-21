﻿using DBDAnalytics.Application.DTOs;
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

namespace DBDAnalytics.WPF.ViewModels.PageVM
{
    internal class DashBoardVM : BaseVM, IUpdatable
    {
        private readonly IPageNavigationService _pageNavigationService;

        private readonly IGetGameStatisticKillerViewingUseCase _getGameStatisticKillerViewingUseCase;
        private readonly IGetGameStatisticSurvivorViewingUseCase _getGameStatisticSurvivorViewingUseCase;
        private readonly IGetKillerUseCase _getKillerUseCase;
        private readonly IGetSurvivorUseCase _getSurvivorUseCase;
        private readonly IGetGameModeUseCase _getGameModeUseCase;
        private readonly IGetGameEventUseCase _getGameEventUseCase;
        private readonly IGetPatchUseCase _getPatchUseCase;
        private readonly IGetMatchAttributeUseCase _getMatchAttributeUseCase;

        public DashBoardVM(IPageNavigationService pageNavigationService,
                           IGetGameStatisticKillerViewingUseCase getGameStatisticKillerViewingUseCase,
                           IGetGameStatisticSurvivorViewingUseCase getGameStatisticSurvivorViewingUseCase,
                           IGetKillerUseCase getKillerUseCase,
                           IGetSurvivorUseCase getSurvivorUseCase,
                           IGetGameModeUseCase getGameModeUseCase,
                           IGetGameEventUseCase getGameEventUseCase,
                           IGetPatchUseCase getPatchUseCase,
                           IGetMatchAttributeUseCase getMatchAttributeUseCase)
        {
            _pageNavigationService = pageNavigationService;
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

        #region Закрытие страницы

        private RelayCommand _closePageCommand;
        public RelayCommand ClosePageCommand { get => _closePageCommand ??= new(obj =>{ _pageNavigationService.Close(PageName.DashBoard, FrameName.MainFrame); }); }

        #endregion

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
        public RelayCommand ResetKillerFilterCommand { get => _resetKillerFilterCommand ??= new(obj => { SetDefaultKillerFilter(); }); }

        private RelayCommand _resetSurvivorFilterCommand;
        public RelayCommand ResetSurvivorFilterCommand { get => _resetSurvivorFilterCommand ??= new(obj => { SetDefaultSurvivorFilter(); }); }

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

        /*--Методы----------------------------------------------------------------------------------------*/

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

    }
}