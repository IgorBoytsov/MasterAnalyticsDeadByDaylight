using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Enums;
using DBDAnalytics.Application.Extensions;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Application.UseCases.Abstraction.ItemAddonCase;
using DBDAnalytics.Application.UseCases.Abstraction.ItemCase;
using DBDAnalytics.Application.UseCases.Abstraction.StatisticCase;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;
using DBDAnalytics.Domain.Enums;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Collections.ObjectModel;

namespace DBDAnalytics.WPF.ViewModels.PageVM
{
    internal class ItemDetailsVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IPageNavigationService _pageNavigationService;

        private readonly IGetDetailsMatchUseCase _getDetailsMatchUseCase;

        private readonly IGetAssociationUseCase _getAssociationUseCase;
        private readonly IGetSurvivorUseCase _getSurvivorUseCase;
        private readonly IGetItemUseCase _getItemUseCase;
        private readonly IGetItemAddonUseCase _getItemAddonUseCase;

        private readonly ICalculationGeneralService _calculationGeneralService;
        private readonly ICalculationItemService _calculationItemService;

        public ItemDetailsVM(IWindowNavigationService windowNavigationService,
                             IPageNavigationService pageNavigationService,
                             IGetDetailsMatchUseCase getDetailsMatchUseCase,
                             IGetAssociationUseCase getAssociationUseCase,
                             IGetSurvivorUseCase getSurvivorUseCase,
                             IGetItemUseCase getItemUseCase,
                             IGetItemAddonUseCase getItemAddonUseCase,
                             ICalculationGeneralService calculationGeneralService,
                             ICalculationItemService calculationItemService)
        {
            _windowNavigationService = windowNavigationService;
            _pageNavigationService = pageNavigationService;

            _getDetailsMatchUseCase = getDetailsMatchUseCase;

            _getAssociationUseCase = getAssociationUseCase;
            _getSurvivorUseCase = getSurvivorUseCase;
            _getItemUseCase = getItemUseCase;
            _getItemAddonUseCase = getItemAddonUseCase;

            _calculationGeneralService = calculationGeneralService;
            _calculationItemService = calculationItemService;

            IsCalculationAllData = true;
            IsCalculationSelectedAssociation = false;
            IsCalculationSelectedTimePeriod = false;
            IsCalculationTransmittedMatches = false;

            _itemsAddons.AddRange(GetItemsAddons());
            _survivors.AddRange(GetSurvivors());

            SelectedTimePeriod = PeriodTimes[2];
            SelectedSurvivorPickItemSorting = SurvivorPickItemSorting.FirstOrDefault();

            LoadAssociations();
            SelectedAssociation = PlayerAssociations.FirstOrDefault();

            LoadItems();
            SelectedItem = Items.FirstOrDefault();
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

        private List<DetailsMatchSurvivorDTO> _survivorsDetails { get; set; } = [];

        #endregion

        #region Коллекции : Данные для расчетов

        private List<ItemAddonDTO> _itemsAddons = [];

        private List<SurvivorDTO> _survivors = [];

        public ObservableCollection<ItemDTO> Items { get; set; } = [];

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

        #region Колекции : Расчеты

        public ObservableCollection<LoadoutPopularity> ItemsAddonsPopularity { get; set; } = [];

        public ObservableCollection<DoubleAddonsPopularity<ItemAddonDTO>> DoubleAddonsPopularity { get; set; } = [];

        public ObservableCollection<DetailsItemsTakenCharacter> DetailsSurvivorsPickItem { get; set; } = [];

        #endregion

        #region Колекции : Список сортировки

        public ObservableCollection<KeyValuePair<By, string>> SurvivorPickItemSorting { get; set; } =
        [
            new KeyValuePair<By, string>(By.PickRate, "Популярности"),
            new KeyValuePair<By, string>(By.ID, "Выходу выжившего"),
        ];

        #endregion

        /*--Свойства--*/

        #region Свойства : Выбор данных сортировки

        private KeyValuePair<By, string> _selectedSurvivorPickItemSorting;
        public KeyValuePair<By, string> SelectedSurvivorPickItemSorting
        {
            get => _selectedSurvivorPickItemSorting;
            set
            {
                _selectedSurvivorPickItemSorting = value;
                SortingSurvivorPickItem(value.Key);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства : Выбор данных фильтрации

        private ItemDTO _selectedItem;
        public ItemDTO SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
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

        #region Свойства : Статистика в шапке

        private int _totalSurvivor;

        private int _totalCountSurvivorWithItem;
        public int TotalCountSurvivorWithItem
        {
            get => _totalCountSurvivorWithItem;
            set
            {
                _totalCountSurvivorWithItem = value;
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

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Получение списка матчей | выживших с предметами

        private void GetDetailsMatches()
        {
            _matchDetails.Clear();
            _survivorsDetails.Clear();

            var (DetailsMatch, TotalMatches) = Task.Run(() => _getDetailsMatchUseCase.GetDetailsMatch(SelectedItem.IdItem, (Associations)SelectedAssociation.Key, FilterParameter.Item)).Result;
            _matchDetails.AddRange(DetailsMatch);
            _totalSurvivor = TotalMatches;
            CountMatch = _matchDetails.Count;

            var survivorWitchItem = _calculationItemService.GetSurvivors(_matchDetails, SelectedItem.IdItem, (Associations)SelectedAssociation.Key);

            _survivorsDetails.AddRange(survivorWitchItem);
            TotalCountSurvivorWithItem = survivorWitchItem.Count;
        }

        #endregion

        #region Получение | Заполнение список для фильтрации

        private List<SurvivorDTO> GetSurvivors() => _getSurvivorUseCase.GetAll().Skip(1).ToList();

        private List<ItemDTO> GetItems() => [.. _getItemUseCase.GetAll()];

        private List<ItemAddonDTO> GetItemsAddons() => [.. _getItemAddonUseCase.GetAll()];

        private void LoadItems()
        {
            Items.Clear();

            foreach (var item in GetItems())
            {
                Items.Add(item);
            }
        }

        private void LoadAssociations()
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

        #endregion

        /*--Сортировка--*/

        #region Сортировка списка с детализацией выживших по выбранному предмету.

        private void SortingSurvivorPickItem(By key)
        {
            Action action = key switch
            {
                By.PickRate => () => DetailsSurvivorsPickItem.ReplaceCollection(DetailsSurvivorsPickItem.OrderByDescending(x => x.PickRate).ToList()),
                By.ID => () => DetailsSurvivorsPickItem.ReplaceCollection(DetailsSurvivorsPickItem.OrderBy(x => x.CharacterId).ToList()),
                _ => () => { }
            };
            action?.Invoke();
        }

        #endregion

        /*--Расчеты--*/

        #region Расчеты : Header

        private void CalculateHeaderStatistics()
        {
            PickRate = _calculationGeneralService.Percentage(_survivorsDetails.Count, _totalSurvivor);
            EscapeRate = _calculationGeneralService.Percentage(_survivorsDetails.Count(x => x.IdTypeDeath == (int)TypeDeaths.Escape), _survivorsDetails.Count);
        }

        #endregion

        #region Расчеты : Loadout

        private void CalculateLoadoutDetailsStatistics()
        {
            AddonPopularityStats();
            DoubleAddonPopularityStats();
            SurvivorPickItemStats();
        }

        private void AddonPopularityStats()
        {
            ItemsAddonsPopularity.Clear();

            var list = _calculationGeneralService.CalculatePopularity<DetailsMatchSurvivorDTO, ItemAddonDTO>(
                _survivorsDetails,
                _itemsAddons.Where(item => item.IdItem == SelectedItem.IdItem).ToList(),
                (survivor, addon) =>
                    survivor.IdFirstAddon == addon.IdItemAddon ||
                    survivor.IdSecondAddon == addon.IdItemAddon,
                addon => addon.ItemAddonName,
                addon => addon.ItemAddonImage,
                survivor => survivor.IdTypeDeath == (int)TypeDeaths.Escape
            );

            foreach (var item in list)
                ItemsAddonsPopularity.Add(item);

        }

        private void DoubleAddonPopularityStats()
        {
            DoubleAddonsPopularity.Clear();

            var list = _calculationGeneralService.DoubleItemPopularity<DetailsMatchSurvivorDTO, ItemAddonDTO>(
                _survivorsDetails,
                _itemsAddons.Where(addon => addon.IdItem == SelectedItem.IdItem).ToList(),
                addonId => _itemsAddons.FirstOrDefault(a => a.IdItemAddon == addonId),
                survivor => (survivor.IdFirstAddon, survivor.IdSecondAddon),
                pairKey =>
                {
                    return survivor =>
                    {
                        if (!survivor.IdFirstAddon.HasValue || !survivor.IdSecondAddon.HasValue)
                            return false;

                        int firstAddonId = survivor.IdFirstAddon.Value;
                        int secondAddonId = survivor.IdSecondAddon.Value;

                        int minAddonId = Math.Min(firstAddonId, secondAddonId);
                        int maxAddonId = Math.Max(firstAddonId, secondAddonId);

                        return minAddonId == pairKey.FirstItemID && maxAddonId == pairKey.SecondItemID;
                    };
                },
                survivor => survivor.IdTypeDeath == (int)TypeDeaths.Escape
            );

            foreach (var item in list.OrderByDescending(x => x.Count))
                DoubleAddonsPopularity.Add(item);
        }

        private void SurvivorPickItemStats()
        {
            DetailsSurvivorsPickItem.Clear();

            var list = _calculationGeneralService.DetailsCharacterPickItem(
                _survivors,
                _survivorsDetails,
                (details, survivor) => details.IdSurvivor == survivor.IdSurvivor,
                survivor => survivor.IdSurvivor,
                survivor => survivor.SurvivorImage,
                survivor => survivor.SurvivorName);

            foreach (var item in list)
            {
                DetailsSurvivorsPickItem.Add(item);
            }

            SortingSurvivorPickItem(SelectedSurvivorPickItemSorting.Key);
        }

        #endregion

    }
}