using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Enums;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Application.UseCases.Abstraction.StatisticCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase;
using DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase;
using DBDAnalytics.Domain.Enums;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Collections.ObjectModel;

namespace DBDAnalytics.WPF.ViewModels.PageVM
{
    internal class SurvivorDetailsVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IPageNavigationService _pageNavigationService;

        private readonly IGetDetailsMatchUseCase _getDetailsMatchUseCase;

        private readonly IGetSurvivorUseCase _getSurvivorUseCase;
        private readonly IGetSurvivorPerkUseCase _getSurvivorPerkUseCase;
        private readonly IGetAssociationUseCase _getAssociationUseCase;
        private readonly IWhoPlacedMapService _getWhoPlacedMapService;
        private readonly IGetTypeDeathUseCase _getTypeDeathUseCase;

        private readonly ICalculationGeneralService _calculationGeneralService;
        private readonly ICalculationSurvivorService _calculationSurvivorService;

        public SurvivorDetailsVM(IWindowNavigationService windowNavigationService,
                                 IPageNavigationService pageNavigationService,
                                 IGetDetailsMatchUseCase getDetailsMatchUseCase,
                                 IGetSurvivorUseCase getSurvivorUseCase,
                                 IGetSurvivorPerkUseCase getSurvivorPerkUseCase,
                                 IGetAssociationUseCase getAssociationUseCase,
                                 IWhoPlacedMapService getWhoPlacedMapService,
                                 IGetTypeDeathUseCase getTypeDeathUseCase,
                                 ICalculationGeneralService calculationGeneralService,
                                 ICalculationSurvivorService calculationSurvivorService)
        {
            _windowNavigationService = windowNavigationService;
            _pageNavigationService = pageNavigationService;
            _getDetailsMatchUseCase = getDetailsMatchUseCase;

            _getSurvivorUseCase = getSurvivorUseCase;
            _getSurvivorPerkUseCase = getSurvivorPerkUseCase;
            _getAssociationUseCase = getAssociationUseCase;
            _getWhoPlacedMapService = getWhoPlacedMapService;
            _getTypeDeathUseCase = getTypeDeathUseCase;

            _calculationGeneralService = calculationGeneralService;
            _calculationSurvivorService = calculationSurvivorService;

            IsCalculationSelectedTimePeriod = false;
            IsCalculationSelectedAssociation = false;
            IsCalculationSelectedAssociation = false;

            TextStatistic = new TextStatistic();

            GetTypeDeaths();
            GetSurvivorPerks();
            GetAssociations();
            SelectedAssociation = PlayerAssociations.FirstOrDefault();

            GetSurvivors();
            SelectedSurvivor = Survivors.FirstOrDefault();

            SelectedTimePeriod = PeriodTimes[2];
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        /*--Поля--*/

        private static int _idSurvivor;
        private static int _idAssociation;

        #region Поля : Разрешение на вызов методов в Selected свойствах

        private bool IsCalculationSelectedTimePeriod;
        private bool IsCalculationSelectedAssociation;

        #endregion

        /*--Коллекции--*/

        #region Коллекция : Список матчей

        private List<DetailsMatchDTO> _transmittedMatches { get; set; } = [];

        private List<DetailsMatchDTO> _matchDetails { get; set; } = [];

        private List<DetailsMatchSurvivorDTO> _survivorsDetails { get; set; } = [];

        #endregion

        #region Коллекция : Список Выживших

        public ObservableCollection<SurvivorDTO> Survivors { get; set; } = [];

        #endregion

        #region Колекции : Параметры для сортировок

        public ObservableCollection<KeyValuePair<TimePeriod, string>> PeriodTimes { get; set; } = new()
        {
            new KeyValuePair<TimePeriod, string>(TimePeriod.Day,    "По дням"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Week,   "По неделям"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Month,  "По месяцам"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Year,   "По годам"),
        };

        public ObservableCollection<KeyValuePair<int, string>> PlayerAssociations { get; set; } = [];

        #endregion

        #region Коллекции : Дополнительные данные

        private List<TypeDeathDTO> _typeDeaths { get; set; } = [];

        private List<SurvivorPerkDTO> _survivorPerks { get; set; } = [];

        #endregion

        #region Колекции : Расчеты

        public ObservableCollection<LabeledValue> DetailsTypeDeaths { get; set; } = [];

        public ObservableCollection<LoadoutPopularity> PerkPopularity { get; set; } = [];

        public ObservableCollection<QuadruplePerksPopularity<SurvivorPerkDTO>> QuadruplePerkPopularity { get; set; } = [];

        #endregion

        /*--Свойства--*/

        #region Свойства : Выбор параметров

        private SurvivorDTO _selectedSurvivor;
        public SurvivorDTO SelectedSurvivor
        {
            get => _selectedSurvivor;
            set
            {
                _selectedSurvivor = value;
                _idSurvivor = value.IdSurvivor;
                GetDetailsMatches();

                CalculateHeaderStatistics();
                CalculateDetailsStatistics();
                CalculateLoadoutDetailsStatistics();

                OnPropertyChanged();

                IsCalculationSelectedTimePeriod = true;
                IsCalculationSelectedAssociation = true;
            }
        }

        private KeyValuePair<TimePeriod, string> _selectedTimePeriod;
        public KeyValuePair<TimePeriod, string> SelectedTimePeriod
        {
            get => _selectedTimePeriod;
            set
            {
                _selectedTimePeriod = value;

                if (IsCalculationSelectedTimePeriod)
                {

                }

                OnPropertyChanged();
            }
        }

        private KeyValuePair<int, string> _selectedAssociation;
        public KeyValuePair<int, string> SelectedAssociation
        {
            get => _selectedAssociation;
            set
            {
                _selectedAssociation = value;
                _idAssociation = value.Key;

                if (IsCalculationSelectedAssociation)
                {
                    GetDetailsMatches();

                    CalculateHeaderStatistics();
                    CalculateDetailsStatistics();
                    CalculateLoadoutDetailsStatistics();
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Статистика в шапке

        private int _totalCountSurvivorInMatches;

        private int _totalCountCurrentSurvivor;
        public int TotalCountCurrentSurvivor
        {
            get => _totalCountCurrentSurvivor;
            set
            {
                _totalCountCurrentSurvivor = value;
                OnPropertyChanged();
            }
        }

        private int _escapedCountCurrentSurvivor;
        public int EscapedCountCurrentSurvivor
        {
            get => _escapedCountCurrentSurvivor;
            set
            {
                _escapedCountCurrentSurvivor = value;
                OnPropertyChanged();
            }
        }

        private int _deathCountCurrentSurvivor;
        public int DeathCountCurrentSurvivor
        {
            get => _deathCountCurrentSurvivor;
            set
            {
                _deathCountCurrentSurvivor = value;
                OnPropertyChanged();
            }
        }

        private int _countMatch;
        public int CountMatch
        {
            get => _countMatch;
            set
            {
                _countMatch = value;
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

        private double _anonymousRate;
        public double AnonymousRate
        {
            get => _anonymousRate;
            set
            {
                _anonymousRate = value;
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

        #region Свойства : Индексы выживших

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
            }
        }

        #endregion

        #region Свойство : Статистика в виде списка

        private TextStatistic _textStatistic;
        public TextStatistic TextStatistic
        {
            get => _textStatistic;
            set
            {
                _textStatistic = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Выбор статистики перков

        private LoadoutPopularity _selectedPerkPopularity;
        public LoadoutPopularity SelectedPerkPopularity
        {
            get => _selectedPerkPopularity;
            set
            {
                _selectedPerkPopularity = value;
                OnPropertyChanged();
            }
        }

        private QuadruplePerksPopularity<SurvivorPerkDTO> _selectedQuadrupleSurvivorPerksPopularity;
        public QuadruplePerksPopularity<SurvivorPerkDTO> SelectedQuadrupleSurvivorPerksPopularity
        {
            get => _selectedQuadrupleSurvivorPerksPopularity;
            set
            {
                _selectedQuadrupleSurvivorPerksPopularity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Переключение индиексов в списке карт

        private RelayCommand _nextSurvivorCommand;
        public RelayCommand NextSurvivorCommand
        {
            get => _nextSurvivorCommand ??= new(obj =>
            {
                SelectedSurvivorIndex++;
            });
        }

        private RelayCommand _previousSurvivorCommand;
        public RelayCommand PreviousSurvivorCommand
        {
            get => _previousSurvivorCommand ??= new(obj =>
            {
                SelectedSurvivorIndex--;
            });
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Получение списка матчей

        private void GetDetailsMatches()
        {
            _matchDetails.Clear();
            _survivorsDetails.Clear();

            var (DetailsMatch, TotalMatches) = Task.Run(() => _getDetailsMatchUseCase.GetDetailsMatch(SelectedSurvivor.IdSurvivor, (Associations)SelectedAssociation.Key, FilterParameter.Survivors)).Result;
            _matchDetails.AddRange(DetailsMatch);

            var survivors = _calculationSurvivorService.GetSurvivors(_matchDetails, SelectedSurvivor.IdSurvivor, (Associations)SelectedAssociation.Key);
            _survivorsDetails.AddRange(survivors);

            _totalCountSurvivorInMatches = TotalMatches;
        }

        #endregion

        #region Получение | Заполнение данных для сортировки/фильтрации

        private void GetSurvivors()
        {
            foreach (var item in _getSurvivorUseCase.GetAll().Skip(1))
                Survivors.Add(item);
        }

        private void GetAssociations()
        {
            var associations = _getAssociationUseCase.GetAll().Select(x =>
            {
                string label = x.IdPlayerAssociation switch
                {
                    (int)Associations.Me => "Личная статистика",
                    (int)Associations.Opponent => "Статистика противника",
                    (int)Associations.Friend => "Статистика друга",
                    (int)Associations.RandomPlayer => "Статистика рандомных игроков",
                    _ => string.Empty,
                };

                return new KeyValuePair<int, string>(x.IdPlayerAssociation, label);
            })
                .Where(x => x.Value != string.Empty)
                .ToList();

            foreach (var item in associations)
                PlayerAssociations.Add(item);
        }

        private void GetTypeDeaths() => _typeDeaths.AddRange(_getTypeDeathUseCase.GetAll());

        private void GetSurvivorPerks() => _survivorPerks.AddRange(_getSurvivorPerkUseCase.GetAll());

        #endregion

        /*--Расчеты--*/

        #region Расчеты : Header

        private void CalculateHeaderStatistics()
        {
            CountMatch = _matchDetails.Count;
            TotalCountCurrentSurvivor = _survivorsDetails.Count;

            PickRate = _calculationGeneralService.Percentage(_survivorsDetails.Count, _totalCountSurvivorInMatches);
            EscapeRate = _calculationGeneralService.Percentage(_survivorsDetails.Count(x => x.IdTypeDeath == (int)TypeDeaths.Escape), TotalCountCurrentSurvivor);
            AnonymousRate = _calculationGeneralService.Percentage(_survivorsDetails.Count(x => x.Anonymous == true), TotalCountCurrentSurvivor);
            BotRate = _calculationGeneralService.Percentage(_survivorsDetails.Count(x => x.Bot == true), TotalCountCurrentSurvivor);
        }

        #endregion

        #region Расчеты : Details

        private void CalculateDetailsStatistics()
        {
            DetailingTypeDeathsStats();
        }

        private void DetailingTypeDeathsStats()
        {
            DetailsTypeDeaths.Clear();

            foreach (var item in _calculationSurvivorService.DetailingTypeDeath(_survivorsDetails, _typeDeaths))
                DetailsTypeDeaths.Add(item);
        }

        #endregion

        #region Расчеты : Loadout

        private void CalculateLoadoutDetailsStatistics()
        {
            PerkPopularityStats();
            QuadruplePerkPopularityStats();
        }

        private void PerkPopularityStats()
        {
            PerkPopularity.Clear();

            var list = _calculationGeneralService.CalculatePopularity<DetailsMatchSurvivorDTO, SurvivorPerkDTO>(
                _survivorsDetails,
                _survivorPerks,
                (match, perk) =>
                    match.IdFirstPerk == perk.IdSurvivorPerk ||
                    match.IdSecondPerk == perk.IdSurvivorPerk ||
                    match.IdThirdPerk == perk.IdSurvivorPerk ||
                    match.IdFourthPerk == perk.IdSurvivorPerk,
                perk => perk.PerkName,
                perk => perk.PerkImage,
                match => match.IdTypeDeath == (int)TypeDeaths.Escape
            );

            foreach (var item in list)
                PerkPopularity.Add(item);

            SelectedPerkPopularity = PerkPopularity.FirstOrDefault();
        }

        private void QuadruplePerkPopularityStats()
        {
            QuadruplePerkPopularity.Clear();

            var list = _calculationGeneralService.QuadrupleItemPopularity<DetailsMatchSurvivorDTO, SurvivorPerkDTO>(
                _survivorsDetails,
                _survivorPerks,
                perkId => _survivorPerks.FirstOrDefault(p => p.IdSurvivorPerk == perkId),
                survivor => (survivor.IdFirstPerk, survivor.IdSecondPerk, survivor.IdThirdPerk, survivor.IdFourthPerk),
                comboKey =>
                {
                    return survivor =>
                    {
                        var survivorPerks = new List<int?>
                        {
                            survivor.IdFirstPerk,
                            survivor.IdSecondPerk,
                            survivor.IdThirdPerk,
                            survivor.IdFourthPerk
                        };

                        if (survivorPerks.Any(id => !id.HasValue))
                            return false;

                        var survivorPerkIdsSet = survivorPerks.Select(id => id!.Value).ToHashSet();

                        var targetPerkIdsSet = new HashSet<int> { comboKey.FirstItemID, comboKey.SecondItemID, comboKey.ThirdItemID, comboKey.FourthItemID };

                        return survivorPerkIdsSet.Count == 4 && targetPerkIdsSet.SetEquals(survivorPerkIdsSet);
                    };
                },
                survivor => survivor.IdTypeDeath == (int)TypeDeaths.Escape
            );

            foreach (var item in list.OrderByDescending(x => x.PickRate))
                QuadruplePerkPopularity.Add(item);

            SelectedQuadrupleSurvivorPerksPopularity = QuadruplePerkPopularity.FirstOrDefault();
        }

        #endregion
    }
}