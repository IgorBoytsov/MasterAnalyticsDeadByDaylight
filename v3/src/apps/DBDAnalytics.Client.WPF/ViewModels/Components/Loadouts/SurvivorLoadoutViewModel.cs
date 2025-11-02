using DBDAnalytics.Client.WPF.Enums;
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DBDAnalytics.Client.WPF.ViewModels.Components.Loadouts
{
    internal sealed class SurvivorLoadoutViewModel : BaseViewModel
    {
        /*--Константы-------------------------------------------------------------------------------------*/

        private const int MaxSelectedAddons = 2;
        private const int MaxSelectedPerks = 4;

        /*--Конструктор-----------------------------------------------------------------------------------*/

        public SurvivorLoadoutViewModel(SurvivorTarget target)
        {
            Target = target;

            ItemAddonsView = new CollectionViewSource { Source = _itemAddons }.View;
            PerksView = new CollectionViewSource { Source = _perks }.View;
            OfferingsView = new CollectionViewSource { Source = _offerings }.View;

            InitializeCommand();
        }

        /*--Коллекции-------------------------------------------------------------------------------------*/

        public ICollectionView PerksView { get; private set; }
        private ObservableCollection<SurvivorPerkViewModel> _perks { get; } = [];

        public ICollectionView ItemAddonsView { get; private set; }
        private ObservableCollection<ItemAddonViewModel> _itemAddons { get; } = [];

        public ICollectionView OfferingsView { get; private set; }
        private ObservableCollection<OfferingViewModel> _offerings { get; } = [];

        public ObservableCollection<SurvivorPerkViewModel> SelectedPerks { get; } = [];
        public ObservableCollection<ItemAddonViewModel> SelectedAddons { get; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IDisplayCollectionController PerksViewController { get; } = new DisplayCollectionController(DisplayCollectionType.Grid);
        public IDisplayCollectionController ItemAddonsViewController { get; } = new DisplayCollectionController(DisplayCollectionType.Grid);
        public IDisplayCollectionController OfferingsViewController { get; } = new DisplayCollectionController(DisplayCollectionType.Grid);

        public SurvivorTarget Target { get; }

        private SurvivorViewModel? _selectedSurvivor;
        public SurvivorViewModel? SelectedSurvivor
        {
            get => _selectedSurvivor;
            set => SetProperty(ref _selectedSurvivor, value);
        }

        private ItemViewModel? _selectedItem;
        public ItemViewModel? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    foreach (var addon in SelectedAddons)
                    {
                        Action action = Target switch
                        {
                            SurvivorTarget.First => () => addon.IsPinnedFirst = false,
                            SurvivorTarget.Second => () => addon.IsPinnedSecond = false,
                            SurvivorTarget.Third => () => addon.IsPinnedThird = false,
                            SurvivorTarget.Fourth => () => addon.IsPinnedFourth = false,
                            _ => throw new Exception($"Данный тип не поддерживается")
                        };

                        action?.Invoke();
                    }

                    SelectedAddons.Clear();
                }
            }
        }

        private OfferingViewModel? _selectedOffering;
        public OfferingViewModel? SelectedOffering
        {
            get => _selectedOffering;
            set => SetProperty(ref _selectedOffering, value);
        }

        private TypeDeathResponse? _typeDeath;
        public TypeDeathResponse? TypeDeath
        {
            get => _typeDeath;
            set => SetProperty(ref _typeDeath, value);
        }

        private PlatformResponse? _platform;
        public PlatformResponse? Platform
        {
            get => _platform;
            set => SetProperty(ref _platform, value);
        }

        private PlayerAssociationResponse? _playerAssociation;
        public PlayerAssociationResponse? PlayerAssociation
        {
            get => _playerAssociation;
            set => SetProperty(ref _playerAssociation, value);
        }

        private BitmapImage? _matchImage;
        public BitmapImage? MatchImage
        {
            get => _matchImage;
            set => SetProperty(ref _matchImage, value);
        }

        #region Доп инфа

        private bool _isPlatform;
        public bool IsPlatform
        {
            get => _isPlatform;
            set => SetProperty(ref _isPlatform, value);
        }

        private bool _isAnanyms;
        public bool IsAnanyms
        {
            get => _isAnanyms;
            set
            {
                SetProperty(ref _isAnanyms, value);
            }
        }

        private bool _isBot;
        public bool IsBot
        {
            get => _isBot;
            set
            {
                SetProperty(ref _isBot, value);
            }
        }

        private bool _meOrOpponent;
        public bool MeOrOpponent
        {
            get => _meOrOpponent;
            set
            {
                SetProperty(ref _meOrOpponent, value);
            }
        }

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                SetProperty(ref _score, value);
            }
        }

        private int _prestige;
        public int Prestige
        {
            get => _prestige;
            set
            {
                SetProperty(ref _prestige, value);
            }
        }

        private int _experience;
        public int Experience
        {
            get => _experience;
            set
            {
                SetProperty(ref _experience, value);
            }
        }

        private int _numberOfBloodDrops;
        public int NumberOfBloodDrops
        {
            get => _numberOfBloodDrops;
            set
            {
                SetProperty(ref _numberOfBloodDrops, value);
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        private void InitializeCommand()
        {
            TogglePerkSelectionCommand = new RelayCommand<SurvivorPerkViewModel>(Execute_TogglePerkSelectionCommand);
            ToggleAddonSelectionCommand = new RelayCommand<ItemAddonViewModel>(Execute_ToggleAddonSelectionCommand);
            ToggleOfferingSelectionCommand = new RelayCommand<OfferingViewModel>(Execute_ToggleOfferingSelectionCommand);
        }

        #region  Команда [TogglePerkSelectionCommand]

        public RelayCommand<SurvivorPerkViewModel>? TogglePerkSelectionCommand { get; private set; }

        private void Execute_TogglePerkSelectionCommand(SurvivorPerkViewModel model)
        {
            if (SelectedPerks.Contains(model))
            {
                Action action = Target switch
                {
                    SurvivorTarget.First => () => model.IsPinnedFirst = false,
                    SurvivorTarget.Second => () => model.IsPinnedSecond = false,
                    SurvivorTarget.Third => () => model.IsPinnedThird = false,
                    SurvivorTarget.Fourth => () => model.IsPinnedFourth = false,
                    _ => throw new Exception($"Данный тип не поддерживается")
                };

                action?.Invoke();
                SelectedPerks.Remove(model);
            }
            else if (SelectedPerks.Count < MaxSelectedPerks)
            {
                Action action = Target switch
                {
                    SurvivorTarget.First => () => model.IsPinnedFirst = true,
                    SurvivorTarget.Second => () => model.IsPinnedSecond = true,
                    SurvivorTarget.Third => () => model.IsPinnedThird = true,
                    SurvivorTarget.Fourth => () => model.IsPinnedFourth = true,
                    _ => throw new Exception($"Данный тип не поддерживается")
                };

                action?.Invoke();
                SelectedPerks.Add(model);
            }

            PerksView?.Refresh();
        }

        #endregion

        #region Команда [ToggleAddonSelectionCommand]

        public RelayCommand<ItemAddonViewModel>? ToggleAddonSelectionCommand { get; private set; }

        private void Execute_ToggleAddonSelectionCommand(ItemAddonViewModel model)
        {
            if (SelectedItem is null) return;

            if (SelectedAddons.Contains(model))
            {
                Action action = Target switch
                {
                    SurvivorTarget.First => () => model.IsPinnedFirst = false,
                    SurvivorTarget.Second => () => model.IsPinnedSecond = false,
                    SurvivorTarget.Third => () => model.IsPinnedThird = false,
                    SurvivorTarget.Fourth => () => model.IsPinnedFourth = false,
                    _ => throw new Exception($"Данный тип не поддерживается")
                };

                action?.Invoke();

                SelectedAddons.Remove(model);
            }
            else if (SelectedAddons.Count < MaxSelectedAddons)
            {
                Action action = Target switch
                {
                    SurvivorTarget.First => () => model.IsPinnedFirst = true,
                    SurvivorTarget.Second => () => model.IsPinnedSecond = true,
                    SurvivorTarget.Third => () => model.IsPinnedThird = true,
                    SurvivorTarget.Fourth => () => model.IsPinnedFourth = true,
                    _ => throw new Exception($"Данный тип не поддерживается")
                };

                action?.Invoke();

                SelectedAddons.Add(model);
            }

            ItemAddonsView?.Refresh();
        }

        #endregion

        #region Команда [ToggleOfferingSelectionCommand]

        public RelayCommand<OfferingViewModel>? ToggleOfferingSelectionCommand { get; private set; }

        private void Execute_ToggleOfferingSelectionCommand(OfferingViewModel model)
        {
            if (SelectedOffering == model)
            {
                Action action = Target switch
                {
                    SurvivorTarget.First => () => SelectedOffering.IsPinnedFirst = false,
                    SurvivorTarget.Second => () => SelectedOffering.IsPinnedSecond = false,
                    SurvivorTarget.Third => () => SelectedOffering.IsPinnedThird = false,
                    SurvivorTarget.Fourth => () => SelectedOffering.IsPinnedFourth = false,
                    _ => throw new Exception($"Данный тип не поддерживается")
                };

                action?.Invoke();
                SelectedOffering = null;
            }
            else
            {
                if (SelectedOffering is not null)
                {
                    Action actionSelected = Target switch
                    {
                        SurvivorTarget.First => () => SelectedOffering.IsPinnedFirst = false,
                        SurvivorTarget.Second => () => SelectedOffering.IsPinnedSecond = false,
                        SurvivorTarget.Third => () => SelectedOffering.IsPinnedThird = false,
                        SurvivorTarget.Fourth => () => SelectedOffering.IsPinnedFourth = false,
                        _ => throw new Exception($"Данный тип не поддерживается")
                    };

                    actionSelected?.Invoke();
                }

                Action action = Target switch
                {
                    SurvivorTarget.First => () => model.IsPinnedFirst = true,
                    SurvivorTarget.Second => () => model.IsPinnedSecond = true,
                    SurvivorTarget.Third => () => model.IsPinnedThird = true,
                    SurvivorTarget.Fourth => () => model.IsPinnedFourth = true,
                    _ => throw new Exception($"Данный тип не поддерживается")
                };

                action?.Invoke();

                SelectedOffering = model;
            }

            OfferingsView?.Refresh();
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        #region Заполнение списков

        public void LoadItemAddons(ICollection<ItemAddonViewModel> itemAddons)
        {
            _itemAddons.Clear();

            foreach (var itemAddon in itemAddons)
                _itemAddons.Add(itemAddon);
        }

        public void LoadPerks(ICollection<SurvivorPerkViewModel> perks)
        {
            _perks.Clear();

            foreach (var perk in perks)
                _perks.Add(perk);
        }

        public void LoadOfferings(ICollection<OfferingViewModel> offerings)
        {
            foreach (var offering in offerings)
                _offerings.Add(offering);
        }

        #endregion
    }
}