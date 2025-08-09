using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Enums;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase;
using DBDAnalytics.Application.UseCases.Abstraction.RoleCase;
using DBDAnalytics.Application.UseCases.Abstraction.StatisticCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase;
using DBDAnalytics.Domain.Enums;
using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Collections.ObjectModel;

namespace DBDAnalytics.WPF.ViewModels.PageVM
{
    internal class PerkDetailsVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IPageNavigationService _pageNavigationService;

        private readonly IGetDetailsMatchUseCase _getDetailsMatchUseCase;
        private readonly IGetRoleUseCase _getRoleUseCase;
        private readonly IGetKillerPerkCategoryUseCase _getKillerPerkCategoryUseCase;
        private readonly IGetSurvivorPerkCategoryUseCase _getSurvivorPerkCategoryUseCase;
        private readonly IGetKillerPerkUseCase _getKillerPerkUseCase;
        private readonly IGetSurvivorPerkUseCase _getSurvivorPerkUseCase;
        private readonly IGetKillerUseCase _getKillerUseCase;
        private readonly IGetSurvivorUseCase _getSurvivorUseCase;
        private readonly IGetAssociationUseCase _getAssociationUseCase;

        private readonly ICalculationGeneralService _calculationGeneralService;
        private readonly ICalculationPerkService _calculationPerkService;

        public PerkDetailsVM(IWindowNavigationService windowNavigationService,
                             IPageNavigationService pageNavigationService,
                             IGetDetailsMatchUseCase getDetailsMatchUseCase,
                             IGetRoleUseCase getRoleUseCase,
                             IGetKillerPerkCategoryUseCase getKillerPerkCategoryUseCase,
                             IGetSurvivorPerkCategoryUseCase getSurvivorPerkCategoryUseCase,
                             IGetKillerPerkUseCase getKillerPerkUseCase,
                             IGetSurvivorPerkUseCase getSurvivorPerkUseCase,
                             IGetKillerUseCase getKillerUseCase,
                             IGetSurvivorUseCase getSurvivorUseCase,
                             IGetAssociationUseCase getAssociationUseCase,
                             ICalculationGeneralService calculationGeneralService,
                             ICalculationPerkService calculationPerkService)
        {
            _windowNavigationService = windowNavigationService;
            _pageNavigationService = pageNavigationService;

            _getDetailsMatchUseCase = getDetailsMatchUseCase;
            _getRoleUseCase = getRoleUseCase;
            _getKillerPerkCategoryUseCase = getKillerPerkCategoryUseCase;
            _getSurvivorPerkCategoryUseCase = getSurvivorPerkCategoryUseCase;
            _getKillerPerkUseCase = getKillerPerkUseCase;
            _getSurvivorPerkUseCase = getSurvivorPerkUseCase;
            _getKillerUseCase = getKillerUseCase;
            _getSurvivorUseCase = getSurvivorUseCase;
            _getAssociationUseCase = getAssociationUseCase;

            _calculationGeneralService = calculationGeneralService;
            _calculationPerkService = calculationPerkService;

            IsCalculationAllData = true;
            IsCalculationSelectedAssociation = false;
            IsCalculationSelectedTimePeriod = false;
            IsCalculationTransmittedMatches = false;

            _killers.AddRange(GetKillers());
            _survivors.AddRange(GetSurvivors());
            GetPerks();

            GetPerkCategories();

            LoadRoles();     

            SelectedTimePeriod = PeriodTimes[2];
            SelectedCharacterPickPerkSorting = CharacterPickPerkSorting.FirstOrDefault();
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

        private List<DetailsMatchDTO> _transmittedMatches = [];

        private List<DetailsMatchDTO> _matchDetails = [];

        private List<DetailsMatchSurvivorDTO> _survivorsWithPerkDetails = [];

        private List<DetailsMatchKillerDTO> _killersWithPerkDetails = [];

        #endregion

        #region Коллекции : Данные для расчетов

        private List<KillerDTO> _killers = [];

        private List<SurvivorDTO> _survivors = [];

        private List<KillerPerkDTO> _allKillerPerks = [];

        private List<SurvivorPerkDTO> _allSurvivorPerks = [];

        public ObservableCollection<RoleDTO> GameRoles { get; set; } = [];

        public ObservableCollection<object> Perks { get; set; } = [];

        #endregion

        #region Колекции : Параметры для сортировок

        private List<KillerPerkCategoryDTO> _killerPerkCategories = [];
        private List<SurvivorPerkCategoryDTO> _survivorPerkCategories = [];

        public ObservableCollection<KeyValuePair<TimePeriod, string>> PeriodTimes { get; set; } = new()
        {
            new KeyValuePair<TimePeriod, string>(TimePeriod.Day,    "По дням"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Week,   "По неделям"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Month,  "По месяцам"),
            new KeyValuePair<TimePeriod, string>(TimePeriod.Year,   "По годам"),
        };

        public ObservableCollection<KeyValuePair<int, string>> PlayerAssociations { get; set; } = [];

        public ObservableCollection<object> PerkCategories { get; set; } = [];

        #endregion

        #region Колекции : Список сортировки

        public ObservableCollection<KeyValuePair<By, string>> CharacterPickPerkSorting { get; set; } =
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

        private KeyValuePair<By, string> _selectedCharacterPickPerkSorting;
        public KeyValuePair<By, string> SelectedCharacterPickPerkSorting
        {
            get => _selectedCharacterPickPerkSorting;
            set
            {
                _selectedCharacterPickPerkSorting = value;
                SortingCharacterPickPerk(value.Key);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Статистика в шапке

        private int _totalCharacter;

        private byte[] _perkImage;
        public byte[] PerkImage
        {
            get => _perkImage;
            set
            {
                _perkImage = value;
                OnPropertyChanged();
            }
        }

        private int _totalCountCharacterWithPerk;
        public int TotalCountCharacterWithPerk
        {
            get => _totalCountCharacterWithPerk;
            set
            {
                _totalCountCharacterWithPerk = value;
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

                LoadPerkCategory();
                SelectedPerkCategory = PerkCategories.FirstOrDefault();

                OnPropertyChanged();
            }
        }

        private object _selectedPerkCategory;
        public object SelectedPerkCategory
        {
            get => _selectedPerkCategory;
            set
            {
                _selectedPerkCategory = value;

                LoadPerks();

                OnPropertyChanged();
            }
        }

        private object _selectedPerk;
        public object SelectedPerk
        {
            get => _selectedPerk;
            set
            {
                _selectedPerk = value;

                if (value is KillerPerkDTO killerPerk) PerkImage = killerPerk.PerkImage;
                if (value is SurvivorPerkDTO survivorPerk) PerkImage = survivorPerk.PerkImage;

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

        private int _selectedPerkIndex;
        public int SelectedPrkIndex
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
                SelectedPrkIndex++;
            });
        }

        private RelayCommand _previousOfferingCommand;
        public RelayCommand PreviousOfferingCommand
        {
            get => _previousOfferingCommand ??= new(obj =>
            {
                SelectedPrkIndex--;
            });
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Получение списка матчей | выживших с предметами

        private void GetDetailsMatches()
        {
            if (SelectedPerk == null)
                return;

            _matchDetails.Clear();
            _survivorsWithPerkDetails.Clear();
            _killersWithPerkDetails.Clear();

            List<DetailsMatchDTO> matches = [];

            Action action = SelectedRole.IdRole switch
            {
                (int)Roles.Killer => () =>
                {
                    if (SelectedPerk is KillerPerkDTO selectedPerk)
                    {
                        var (DetailsMatch, TotalMatches) = Task.Run(() => _getDetailsMatchUseCase.GetDetailsMatch(selectedPerk.IdKillerPerk, (Associations)SelectedAssociation.Key, FilterParameter.KillerPerk)).Result;
                        matches = DetailsMatch;
                        _totalCharacter = TotalMatches;
                        _killersWithPerkDetails.AddRange(_calculationPerkService.GetKillers(matches, selectedPerk.IdKillerPerk, (Associations)SelectedAssociation.Key));
                        TotalCountCharacterWithPerk = _killersWithPerkDetails.Count;
                    }
                }
                ,
                (int)Roles.Survivor => () =>
                {
                    if (SelectedPerk is SurvivorPerkDTO selectedPerk)
                    {
                        var (DetailsMatch, TotalMatches) = Task.Run(() => _getDetailsMatchUseCase.GetDetailsMatch(selectedPerk.IdSurvivorPerk, (Associations)SelectedAssociation.Key, FilterParameter.SurvivorPerk)).Result;
                        matches = DetailsMatch;
                        _totalCharacter = TotalMatches;
                        _survivorsWithPerkDetails.AddRange(_calculationPerkService.GetSurvivors(matches, selectedPerk.IdSurvivorPerk, (Associations)SelectedAssociation.Key));
                        TotalCountCharacterWithPerk = _survivorsWithPerkDetails.Count;
                    }
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

        private void GetPerkCategories()
        {
            _survivorPerkCategories.Clear();
            _killerPerkCategories.Clear();

            _survivorPerkCategories.AddRange(Task.Run(_getSurvivorPerkCategoryUseCase.GetAllAsync).Result);
            _killerPerkCategories.AddRange(Task.Run(_getKillerPerkCategoryUseCase.GetAllAsync).Result);
        }

        private void GetPerks()
        {
            _allSurvivorPerks.Clear();
            _allKillerPerks.Clear();

            _allSurvivorPerks.AddRange(_getSurvivorPerkUseCase.GetAll());
            _allKillerPerks.AddRange(_getKillerPerkUseCase.GetAll());
        }

        private List<RoleDTO> GetRoles() => _getRoleUseCase.GetAll();

        private void LoadRoles()
        {
            GameRoles.Clear();

            foreach (var item in GetRoles().Where(x => x.IdRole != (int)Roles.General))
            {
                GameRoles.Add(item);
            }

            SelectedRole = GameRoles.FirstOrDefault();
        }

        private void LoadPerkCategory()
        {
            PerkCategories.Clear();

            if (SelectedRole.IdRole == (int)Roles.Killer)
            {
                foreach (var item in _killerPerkCategories)
                {
                    PerkCategories.Add(item);
                }
            }
            if (SelectedRole.IdRole == (int)Roles.Survivor)
            {
                foreach (var item in _survivorPerkCategories)
                {
                    PerkCategories.Add(item);
                }
            }
        }

        private void LoadPerks()
        {
            Perks.Clear();

            if (SelectedPerkCategory is KillerPerkCategoryDTO selectedKillerCategory)
            {
                foreach (var item in _allKillerPerks.Where(x => x.IdCategory == selectedKillerCategory.IdKillerPerkCategory))
                {
                    Perks.Add(item);
                }
            }

            if (SelectedPerkCategory is SurvivorPerkCategoryDTO selectedSurvivorCategory)
            {
                foreach (var item in _allSurvivorPerks.Where(x => x.IdCategory == selectedSurvivorCategory.IdSurvivorPerkCategory))
                {
                    Perks.Add(item);
                }
            }

            SelectedPerk = Perks.FirstOrDefault();
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

        private void SortingCharacterPickPerk(By key)
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
            PickRate = _calculationGeneralService.Percentage(TotalCountCharacterWithPerk, _totalCharacter);

            WinRate = SelectedRole.IdRole == (int)Roles.Killer ?
                _calculationGeneralService.Percentage(_matchDetails.Count(x => x.CountKill > 2), _matchDetails.Count) :
                _calculationGeneralService.Percentage(_survivorsWithPerkDetails.Count(x => x.IdTypeDeath == (int)TypeDeaths.Escape), _survivorsWithPerkDetails.Count);
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
                   _survivorsWithPerkDetails,
                   (details, survivor) => details.IdSurvivor == survivor.IdSurvivor,
                   survivor => survivor.IdSurvivor,
                   survivor => survivor.SurvivorImage,
                   survivor => survivor.SurvivorName);
            }
            else
            {
                list = _calculationGeneralService.DetailsCharacterPickItem(
                    _killers,
                   _killersWithPerkDetails,
                   (details, killer) => details.IdKiller == killer.IdKiller,
                   killer => killer.IdKiller,
                   killer => killer.KillerImage,
                   killer => killer.KillerName);
            }

            foreach (var item in list)
            {
                DetailsCharacterPickOffering.Add(item);
            }

            SortingCharacterPickPerk(SelectedCharacterPickPerkSorting.Key);
        }

        #endregion

    }
}