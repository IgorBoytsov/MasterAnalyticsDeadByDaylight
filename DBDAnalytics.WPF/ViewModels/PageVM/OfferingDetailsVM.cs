using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Enums;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCase;
using DBDAnalytics.Application.UseCases.Abstraction.RoleCase;
using DBDAnalytics.Application.UseCases.Abstraction.StatisticCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.Domain.Enums;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.Views.Pages;
using System.Collections.ObjectModel;
using System.Windows;

namespace DBDAnalytics.WPF.ViewModels.PageVM
{
    internal class OfferingDetailsVM : BaseVM, IUpdatable
    {
        private readonly IGetOfferingUseCase _getOfferingUseCase;

        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IGetDetailsMatchUseCase _getDetailsMatchUseCase;
        private readonly IGetAssociationUseCase _getAssociationUseCase;
        private readonly IGetRoleUseCase _getRoleUseCase;
        private readonly IGetSurvivorUseCase _getSurvivorUseCase;
        private readonly IGetKillerUseCase _getKillerUseCase;
        private readonly ICalculationGeneralService _calculationGeneralService;
        private readonly ICalculationItemService _calculationItemService;
        private readonly ICalculationOfferingService _calculationOfferingService;

        public OfferingDetailsVM(IWindowNavigationService windowNavigationService,
                                 IPageNavigationService pageNavigationService,
                                 IGetDetailsMatchUseCase getDetailsMatchUseCase,
                                 IGetOfferingUseCase getOfferingUseCase,
                                 IGetAssociationUseCase getAssociationUseCase,
                                 IGetRoleUseCase getRoleUseCase,
                                 IGetSurvivorUseCase getSurvivorUseCase,
                                 IGetKillerUseCase getKillerUseCase,
                                 ICalculationGeneralService calculationGeneralService,
                                 ICalculationOfferingService calculationOfferingService)
        {
            _windowNavigationService = windowNavigationService;
            _pageNavigationService = pageNavigationService;
            _getDetailsMatchUseCase = getDetailsMatchUseCase;
            _getOfferingUseCase = getOfferingUseCase;
            _getAssociationUseCase = getAssociationUseCase;
            _getRoleUseCase = getRoleUseCase;
            _getSurvivorUseCase = getSurvivorUseCase;
            _getKillerUseCase = getKillerUseCase;
            _calculationGeneralService = calculationGeneralService;
            _calculationOfferingService = calculationOfferingService;

            IsCalculationAllData = true;
            IsCalculationSelectedAssociation = false;
            IsCalculationSelectedTimePeriod = false;
            IsCalculationTransmittedMatches = false;

            _allOfferings.AddRange(GetOfferings());
            _killers.AddRange(GetKillers());
            _survivors.AddRange(GetSurvivors());

            SelectedTimePeriod = PeriodTimes[2];
            SelectedCharacterPickOfferingSorting = CharacterPickOfferingSorting.FirstOrDefault();

            LoadRoles();
            SelectedRole = GameRoles.FirstOrDefault();

            LoadOfferings();
            SelectedOffering = Offerings.FirstOrDefault();
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {

        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        /*--Поля--*/

        #region Поля : Разрешение на вызов методов в Selected свойствах

        private bool IsCalculationAllData;
        private bool IsCalculationSelectedTimePeriod;
        private bool IsCalculationSelectedAssociation;

        private bool IsCalculationTransmittedMatches;

        #endregion

        /*--Коллекции--*/

        #region Коллекция : Список матчей

        private List<DetailsMatchDTO> _transmittedMatches { get; set; } = [];

        private List<DetailsMatchDTO> _matchDetails { get; set; } = [];

        private List<DetailsMatchSurvivorDTO> _survivorsWithOfferingDetails { get; set; } = [];

        private List<DetailsMatchKillerDTO> _killersWithOfferingDetails { get; set; } = [];

        #endregion

        #region Коллекции : Данные для расчетов

        private List<KillerDTO> _killers = [];

        private List<SurvivorDTO> _survivors = [];

        private List<OfferingDTO> _allOfferings = [];

        public ObservableCollection<RoleDTO> GameRoles { get; set; } = [];

        public ObservableCollection<OfferingDTO> Offerings { get; set; } = [];

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

        #region Колекции : Список сортировки

        public ObservableCollection<KeyValuePair<By, string>> CharacterPickOfferingSorting { get; set; } =
        [
            new KeyValuePair<By, string>(By.PickRate, "Популярности"),
            new KeyValuePair<By, string>(By.ID, "Выходу персонажа"),
        ];

        #endregion

        #region Колекции : Расчеты

        public ObservableCollection<DetailsItemsTakenCharacter> DetailsCharacterPickOffering { get; set; } = [];

        #endregion

        /*--Свойства--*/

        #region Свойства : Выбор данных сортировки

        private KeyValuePair<By, string> _selectedCharacterPickOfferingSorting;
        public KeyValuePair<By, string> SelectedCharacterPickOfferingSorting
        {
            get => _selectedCharacterPickOfferingSorting;
            set
            {
                _selectedCharacterPickOfferingSorting = value;
                SortingCharacterPickOffering(value.Key);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Статистика в шапке

        private int _totalCharacter;

        private int _totalCountCharacterWithOffering;
        public int TotalCountCharacterWithOffering
        {
            get => _totalCountCharacterWithOffering;
            set
            {
                _totalCountCharacterWithOffering = value;
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

        private double _winRate;
        public double WinRate
        {
            get => _winRate;
            set
            {
                _winRate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Выбор данных фильтрации

        private RoleDTO _selectedRole;
        public RoleDTO SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;

                LoadAssociations();
                SelectedAssociation = PlayerAssociations.FirstOrDefault();

                LoadOfferings();
                
                OnPropertyChanged();
            }
        }

        private OfferingDTO _selectedOffering;
        public OfferingDTO SelectedOffering
        {
            get => _selectedOffering;
            set
            {
                _selectedOffering = value;
                GetDetailsMatches();

                CalculateHeaderStatistics();
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

                if (IsCalculationSelectedAssociation)
                {
                    GetDetailsMatches();

                    CalculateHeaderStatistics(); 
                    CalculateLoadoutDetailsStatistics();
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойство : Выбор индексов в списке киллеров

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
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Переключение индиексов в списке киллеров

        private RelayCommand _nextOfferingCommand;
        public RelayCommand NextOfferingCommand
        {
            get => _nextOfferingCommand ??= new(obj =>
            {
                SelectedOfferingIndex++;
            });
        }

        private RelayCommand _previousOfferingCommand;
        public RelayCommand PreviousOfferingCommand
        {
            get => _previousOfferingCommand ??= new(obj =>
            {
                SelectedOfferingIndex--;
            });
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Получение списка матчей | выживших с предметами

        private void GetDetailsMatches()
        {
            if (SelectedOffering == null)
                return;

            _matchDetails.Clear();
            _survivorsWithOfferingDetails.Clear();
            _killersWithOfferingDetails.Clear();

            List<DetailsMatchDTO> matches = [];

            Action action = SelectedRole.IdRole switch
            {
                (int)Roles.Killer => () =>
                {
                    var (DetailsMatch, TotalMatches) = Task.Run(() => _getDetailsMatchUseCase.GetDetailsMatch(SelectedOffering.IdOffering, (Associations)SelectedAssociation.Key, FilterParameter.KillerOffering)).Result;
                    matches = DetailsMatch;
                    _totalCharacter = TotalMatches;
                    _killersWithOfferingDetails.AddRange(_calculationOfferingService.GetKillers(matches, SelectedOffering.IdOffering, (Associations)SelectedAssociation.Key));
                    TotalCountCharacterWithOffering = _killersWithOfferingDetails.Count;
                }
                ,
                (int)Roles.Survivor => () =>
                {
                    var (DetailsMatch, TotalMatches) = Task.Run(() => _getDetailsMatchUseCase.GetDetailsMatch(SelectedOffering.IdOffering, (Associations)SelectedAssociation.Key, FilterParameter.SurvivorOffering)).Result;
                    matches = DetailsMatch;
                    _totalCharacter = TotalMatches;
                    _survivorsWithOfferingDetails.AddRange(_calculationOfferingService.GetSurvivors(matches, SelectedOffering.IdOffering, (Associations)SelectedAssociation.Key));
                    TotalCountCharacterWithOffering = _survivorsWithOfferingDetails.Count;
                }                
                ,
                _ => () => throw new Exception($"Такое ID - {SelectedRole.IdRole} не обрабатывается.")
            };
            action?.Invoke();
                
            _matchDetails.AddRange(matches);
            CountMatch = _matchDetails.Count;
        }

        #endregion

        #region Получение | Заполнение список для фильтрации

        private List<SurvivorDTO> GetSurvivors() => _getSurvivorUseCase.GetAll().Skip(1).ToList();

        private List<KillerDTO> GetKillers() => _getKillerUseCase.GetAll().Skip(1).ToList();

        private List<OfferingDTO> GetOfferings() => _getOfferingUseCase.GetAll();

        private List<RoleDTO> GetRoles() => _getRoleUseCase.GetAll();

        private void LoadRoles()
        {
            GameRoles.Clear();

            foreach (var item in GetRoles().Where(x => x.IdRole != (int)Roles.General))
            {
                GameRoles.Add(item);
            }
        }

        private void LoadOfferings()
        {
            Offerings.Clear();

            var unitedOfferings = new List<OfferingDTO>();

            unitedOfferings.AddRange(_allOfferings.Where(x => x.IdRole == SelectedRole.IdRole));
            unitedOfferings.AddRange(_allOfferings.Where(x => x.IdRole == (int)Roles.General));

            foreach (var item in unitedOfferings.OrderBy(x => x.IdRarity))
            {
                Offerings.Add(item);
            }

            SelectedOffering = Offerings.FirstOrDefault();
        }

        private void LoadAssociations()
        {
            PlayerAssociations.Clear();

            var associations = _getAssociationUseCase.GetAll().Select(association =>
            {
                string label = string.Empty;

                if (SelectedRole.IdRole == (int)Roles.Killer)
                {
                    label = association.IdPlayerAssociation switch
                    {
                        (int)Associations.Me => "Личная статистика",
                        (int)Associations.Opponent => "Статистика противника",
                        _ => string.Empty,
                    };
                }
                if (SelectedRole.IdRole == (int)Roles.Survivor)
                {
                    label = association.IdPlayerAssociation switch
                    {
                        (int)Associations.Me => "Личная статистика",
                        (int)Associations.Opponent => "Статистика противника",
                        (int)Associations.Friend => "Статистика друга",
                        (int)Associations.RandomPlayer => "Статистика рандомных игроков",
                        _ => string.Empty,
                    };
                }

                return new KeyValuePair<int, string>(association.IdPlayerAssociation, label);
            })
            .Where(x => x.Value != string.Empty)
            .ToList();

            foreach (var item in associations)
                PlayerAssociations.Add(item);
        }

        #endregion

        /*--Сортировка--*/

        #region Сортировка списка с детализацией выживших по выбранному предмету.

        private void SortingCharacterPickOffering(By key)
        {
            Action action = key switch
            {
                By.PickRate => () => DetailsCharacterPickOffering.ReplaceCollection(DetailsCharacterPickOffering.OrderByDescending(x => x.PickRate).ToList()),
                By.ID => () => DetailsCharacterPickOffering.ReplaceCollection(DetailsCharacterPickOffering.OrderBy(x => x.CharacterId).ToList()),
                _ => () => { }
            };
            action?.Invoke();
        }

        #endregion

        /*--Расчеты--*/

        #region Расчеты : Header

        private void CalculateHeaderStatistics()
        {
            PickRate = _calculationGeneralService.Percentage(TotalCountCharacterWithOffering, _totalCharacter);

            WinRate = SelectedRole.IdRole == (int)Roles.Killer ? 
                _calculationGeneralService.Percentage(_matchDetails.Count(x => x.CountKill > 2), _matchDetails.Count) : 
                _calculationGeneralService.Percentage(_survivorsWithOfferingDetails.Count(x => x.IdTypeDeath == (int)TypeDeaths.Escape), _survivorsWithOfferingDetails.Count);
        }

        #endregion

        #region Расчеты : Loadout

        private void CalculateLoadoutDetailsStatistics()
        {
            SurvivorPickItemStats();
        }

        private void SurvivorPickItemStats()
        {
            DetailsCharacterPickOffering.Clear();

            List<DetailsItemsTakenCharacter> list = [];

            if (SelectedRole.IdRole == (int)Roles.Survivor)
            {
                list = _calculationGeneralService.DetailsCharacterPickItem(
                    _survivors,
                   _survivorsWithOfferingDetails,
                   (details, survivor) => details.IdSurvivor == survivor.IdSurvivor,
                   survivor => survivor.IdSurvivor,
                   survivor => survivor.SurvivorImage,
                   survivor => survivor.SurvivorName);
            }
            else
            {
                list = _calculationGeneralService.DetailsCharacterPickItem(
                    _killers,
                   _killersWithOfferingDetails,
                   (details, killer) => details.IdKiller == killer.IdKiller,
                   killer => killer.IdKiller,
                   killer => killer.KillerImage,
                   killer => killer.KillerName);
            }

            foreach (var item in list)
            {
                DetailsCharacterPickOffering.Add(item);
            }

            SortingCharacterPickOffering(SelectedCharacterPickOfferingSorting.Key);
        }

        #endregion

    }
}