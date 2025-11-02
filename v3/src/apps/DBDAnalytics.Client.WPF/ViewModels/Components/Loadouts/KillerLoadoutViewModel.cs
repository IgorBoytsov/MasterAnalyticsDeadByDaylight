using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Base;
using Shared.WPF.ViewModels.Components;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace DBDAnalytics.Client.WPF.ViewModels.Components.Loadouts
{
    internal sealed class KillerLoadoutViewModel : BaseViewModel
    {
        /*--Константы-------------------------------------------------------------------------------------*/

        private const int MaxSelectedAddons = 2;
        private const int MaxSelectedPerks = 4;

        /*--Конструктор-----------------------------------------------------------------------------------*/

        public KillerLoadoutViewModel()
        {
            InitializeCommand();
        }

        /*--Коллекции-------------------------------------------------------------------------------------*/

        public ObservableCollection<KillerPerkViewModel> SelectedPerks { get; private set; } = [];
        public ObservableCollection<KillerAddonViewModel> SelectedAddons { get; private set; } = [];

        /*--Свойства--------------------------------------------------------------------------------------*/

        public IDisplayCollectionController AddonsViewController { get; } = new DisplayCollectionController(DisplayCollectionType.Grid);
        public IDisplayCollectionController PerksViewController { get; } = new DisplayCollectionController(DisplayCollectionType.Grid);
        public IDisplayCollectionController OfferingsViewController { get; } = new DisplayCollectionController(DisplayCollectionType.Grid);

        private PlatformResponse? _platform;
        public PlatformResponse? Platform
        {
            get => _platform;
            set => SetProperty(ref _platform, value);
        }

        private BitmapImage? _killerMatchImage;
        public BitmapImage? KillerMatchImage
        {
            get => _killerMatchImage;
            set => SetProperty(ref _killerMatchImage, value);
        }

        private PlayerAssociationResponse? _playerAssociation;
        public PlayerAssociationResponse? PlayerAssociation
        {
            get => _playerAssociation;
            set => SetProperty(ref _playerAssociation, value);
        }

        #region Киллер, перки, аддоны

        private KillerViewModel? _selectedKiller;
        public KillerViewModel? SelectedKiller
        {
            get => _selectedKiller;
            set => SetProperty(ref _selectedKiller, value);
        }

        private OfferingViewModel? _selectedOffering;
        public OfferingViewModel? SelectedOffering
        {
            get => _selectedOffering;
            set => SetProperty(ref _selectedOffering, value);
        }

        #endregion

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
            ToggleAddonSelectionCommand = new RelayCommand<KillerAddonViewModel>(Execute_ToggleAddonSelectionCommand, CanExecute_ToggleAddonSelectionCommand);
            TogglePerkSelectionCommand = new RelayCommand<KillerPerkViewModel>(Execute_TogglePerkSelectionCommand, CanExecute_TogglePerkSelectionCommand);
            ToggleOfferingSelectionCommand = new RelayCommand<OfferingViewModel>(Execute_ToggleOfferingSelectionCommand, CanExecute_ToggleOfferingSelectionCommand);
        }

        #region Команда [ToggleAddonSelectionCommand]: Закрепить\Открепить улучшение киллера

        public RelayCommand<KillerAddonViewModel>? ToggleAddonSelectionCommand { get; private set; }

        private void Execute_ToggleAddonSelectionCommand(KillerAddonViewModel model)
        {
            if (SelectedAddons.Any(a => a.Id == model.Id))
            {
                model.IsPinned = false;
                SelectedAddons.Remove(model);
            }
            else if (SelectedAddons.Count < MaxSelectedAddons)
            {
                model.IsPinned = true;
                SelectedAddons.Add(model);
            }
        }

        private bool CanExecute_ToggleAddonSelectionCommand(KillerAddonViewModel model) => true;

        #endregion

        #region Команда [TogglePerkSelectionCommand]: Закрепить\Открепить перки киллера

        public RelayCommand<KillerPerkViewModel>? TogglePerkSelectionCommand { get; private set; }

        private void Execute_TogglePerkSelectionCommand(KillerPerkViewModel model)
        {
            if (SelectedPerks.Any(p => p.Id == model.Id))
            {
                model.IsPinned = false;
                SelectedPerks.Remove(model);
            }
            else if (SelectedPerks.Count < MaxSelectedPerks)
            {
                model.IsPinned = true;
                SelectedPerks.Add(model);
            }
        }

        private bool CanExecute_TogglePerkSelectionCommand(KillerPerkViewModel model) => model is not null;

        #endregion

        #region Команда [ToggleOfferingSelectionCommand]: Закрепить\Открепить перки киллера

        public RelayCommand<OfferingViewModel>? ToggleOfferingSelectionCommand { get; private set; }

        private void Execute_ToggleOfferingSelectionCommand(OfferingViewModel model)
        {
            if (SelectedOffering == model)
            {
                SelectedOffering.IsPinned = false;
                SelectedOffering = null;
            }
            else
            {
                if (SelectedOffering is not null)
                    SelectedOffering.IsPinned = false;

                model.IsPinned = true;
                SelectedOffering = model;
            }
        }

        private bool CanExecute_ToggleOfferingSelectionCommand(OfferingViewModel model) => model is not null;

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

        public void SetKiller(KillerViewModel? killer)
        {
            SelectedKiller = killer;

            foreach (var addon in SelectedAddons)
                addon.IsPinned = false;

            if (SelectedOffering is not null)
            {
                SelectedOffering.IsPinned = false;
                SelectedOffering = null;
            }

            SelectedAddons.Clear();
            SelectedPerks.Clear();
        }
    }
}