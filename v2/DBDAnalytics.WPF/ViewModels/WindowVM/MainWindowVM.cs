using DBDAnalytics.WPF.Command;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;

namespace DBDAnalytics.WPF.ViewModels.WindowVM
{
    internal class MainWindowVM : BaseVM, IUpdatable
    {
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IPageNavigationService _pageNavigationService;

        public MainWindowVM(IWindowNavigationService windowNavigationService,
                            IPageNavigationService pageNavigationService) : base(windowNavigationService)
        {
            _windowNavigationService = windowNavigationService;
            _pageNavigationService = pageNavigationService;
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            
        }
        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        #region Свойство : Title

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/

        #region Команда открытие окна. Пункт меню <<Добавить>>. Блок 1. Show()

        private RelayCommand _openAddMatchWindow;
        public RelayCommand OpenAddMatchWindow
        {
            get => _openAddMatchWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.AddMatch, parameter: null, TypeParameter.None, IsOpenDialog: false);
            });
        }

        #endregion

        #region Команды открытие окон. Пункт меню <<Добавить>>. Блок 2. ShowDialog()

        private RelayCommand _openInteractionAssociationWindow;
        public RelayCommand OpenInteractionAssociationWindow
        {
            get => _openInteractionAssociationWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionAssociation, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }

        private RelayCommand _openInteractionGameEventWindow;
        public RelayCommand OpenInteractionGameEventWindow
        {
            get => _openInteractionGameEventWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionGameEvent, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }

        private RelayCommand _openInteractionGameModeWindow;
        public RelayCommand OpenInteractionGameModeWindow
        {
            get => _openInteractionGameModeWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionGameMode, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }
        
        private RelayCommand _openInteractionPatchWindow;
        public RelayCommand OpenInteractionPatchWindow
        {
            get => _openInteractionPatchWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionPatch, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }

        private RelayCommand _openInteractionPlatformWindow;
        public RelayCommand OpenInteractionPlatformWindow
        {
            get => _openInteractionPlatformWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionPlatform, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }
        
        private RelayCommand _openInteractionRarityWindow;
        public RelayCommand OpenInteractionRarityWindow
        {
            get => _openInteractionRarityWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionRarity, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }

        private RelayCommand _openInteractionRoleWindow;
        public RelayCommand OpenInteractionRoleWindow
        {
            get => _openInteractionRoleWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionRole, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }
        
        private RelayCommand _openInteractionTypeDeathWindow;
        public RelayCommand OpenInteractionTypeDeathWindow
        {
            get => _openInteractionTypeDeathWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionTypeDeath, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }

        private RelayCommand _openInteractionWhoPlacedMapWindow;
        public RelayCommand OpenInteractionWhoPlacedMapWindow
        {
            get => _openInteractionWhoPlacedMapWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionWhoPlacedMap, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }

        #endregion

        #region Команды открытие окон. Пункт меню <<Добавить>>. Блок 3. ShowDialog() 

        private RelayCommand _openInteractionKillerPerkCategoryWindow;
        public RelayCommand OpenInteractionKillerPerkCategoryWindow
        {
            get => _openInteractionKillerPerkCategoryWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionKillerPerkCategory, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }

        private RelayCommand _openInteractionSurvivorPerkCategoryWindow;
        public RelayCommand OpenInteractionSurvivorPerkCategoryWindow
        {
            get => _openInteractionSurvivorPerkCategoryWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionSurvivorPerkCategory, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }

        private RelayCommand _openInteractionOfferingCategoryWindow;
        public RelayCommand OpenInteractionOfferingCategoryWindow
        {
            get => _openInteractionOfferingCategoryWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionOfferingCategory, parameter: null, TypeParameter.None, IsOpenDialog: true);
            });
        }

        #endregion

        #region Команды открытие окон. Пункт меню <<Добавить>>. Блок 4. Show() 

        private RelayCommand _openInteractionKillerWindow;
        public RelayCommand OpenInteractionKillerWindow
        {
            get => _openInteractionKillerWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionKiller, parameter: null, TypeParameter.None, IsOpenDialog: false);
            });
        } 
        
        private RelayCommand _openInteractionSurvivorWindow;
        public RelayCommand OpenInteractionSurvivorWindow
        {
            get => _openInteractionSurvivorWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionSurvivor, parameter: null, TypeParameter.None, IsOpenDialog: false);
            });
        }
        
        private RelayCommand _openInteractionItemWindow;
        public RelayCommand OpenInteractionItemWindow
        {
            get => _openInteractionItemWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionItem, parameter: null, TypeParameter.None, IsOpenDialog: false);
            });
        }

        private RelayCommand _openInteractionMeasurementWindow;
        public RelayCommand OpenInteractionMeasurementWindow
        {
            get => _openInteractionMeasurementWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionMeasurement, parameter: null, TypeParameter.None, IsOpenDialog: false);
            });
        }
        
        private RelayCommand _openInteractionOfferingWindow;
        public RelayCommand OpenInteractionOfferingWindow
        {
            get => _openInteractionOfferingWindow ??= new(obj =>
            {
                _windowNavigationService.OpenWindow(WindowName.InteractionOffering, parameter: null, TypeParameter.None, IsOpenDialog: false);
            });
        }

        #endregion

        #region Команды открытие страниц на боковой панели.

        private RelayCommand _navigateDashBoardPageCommand;
        public RelayCommand NavigateDashBoardPageCommand
        {
            get => _navigateDashBoardPageCommand ??= new(obj =>
            {
                _pageNavigationService.Navigate(PageName.DashBoard, FrameName.MainFrame);
            });
        }

        private RelayCommand _navigateKillerDetailsPageCommand;
        public RelayCommand NavigateKillerDetailsPageCommand
        {
            get => _navigateKillerDetailsPageCommand ??= new(obj =>
            {
                _pageNavigationService.Navigate(PageName.KillerDetails, FrameName.MainFrame);
            });
        }

        private RelayCommand _navigateMapDetailsPageCommand;
        public RelayCommand NavigateMapDetailsPageCommand
        {
            get => _navigateMapDetailsPageCommand ??= new(obj =>
            {
                _pageNavigationService.Navigate(PageName.MapDetails, FrameName.MainFrame);
            });
        }

        private RelayCommand _navigateSurvivorDetailsPageCommand;
        public RelayCommand NavigateSurvivorDetailsPageCommand
        {
            get => _navigateSurvivorDetailsPageCommand ??= new(obj =>
            {
                _pageNavigationService.Navigate(PageName.SurvivorDetails, FrameName.MainFrame);
            });
        }

        private RelayCommand _navigateItemDetailsPageCommand;
        public RelayCommand NavigateItemDetailsPageCommand
        {
            get => _navigateItemDetailsPageCommand ??= new(obj =>
            {
                _pageNavigationService.Navigate(PageName.ItemDetails, FrameName.MainFrame);
            });
        }

        private RelayCommand _navigateOfferingDetailsPageCommand;
        public RelayCommand NavigateOfferingDetailsPageCommand
        {
            get => _navigateOfferingDetailsPageCommand ??= new(obj =>
            {
                _pageNavigationService.Navigate(PageName.OfferingDetails, FrameName.MainFrame);
            });
        }

        private RelayCommand _navigatePerkDetailsPageCommand;
        public RelayCommand NavigatePerkDetailsPageCommand
        {
            get => _navigatePerkDetailsPageCommand ??= new(obj =>
            {
                _pageNavigationService.Navigate(PageName.PerkDetails, FrameName.MainFrame);
            });
        }

        #endregion

        /*--Методы----------------------------------------------------------------------------------------*/

    }
}