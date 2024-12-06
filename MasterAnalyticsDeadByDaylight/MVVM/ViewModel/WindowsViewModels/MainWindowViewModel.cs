using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.WindowNavigation;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Команды, свойства, методы для изменений размера окна

        private RelayCommand _closeAppCommand;
        public RelayCommand CloseAppCommand
        {
            get
            {
                return _closeAppCommand ??= new RelayCommand(obj =>
                {
                    CloseApp();
                });
            }
        }

        private RelayCommand _maximizeAppCommand;
        public RelayCommand MaximizeAppCommand
        {
            get
            {
                return _maximizeAppCommand ??= new RelayCommand(obj =>
                {
                    MaximizeApp();
                });
            }
        }

        private RelayCommand _restoreAppCommand;
        public RelayCommand RestoreAppCommand
        {
            get
            {
                return _restoreAppCommand ??= new RelayCommand(obj =>
                {
                    RestoreApp();
                });
            }
        }

        private RelayCommand _minimizeAppCommand;
        public RelayCommand MinimizeAppCommand
        {
            get
            {
                return _minimizeAppCommand ??= new RelayCommand(obj =>
                {
                    MinimizeApp();
                });
            }
        }

        private RelayCommand _deleteTxtBlock;
        public RelayCommand DeleteTxtBlock
        {
            get
            {
                return _deleteTxtBlock ??= new RelayCommand(obj =>
                {
                    MyControlVisibility = Visibility.Collapsed;
                });
            }
        }

        private void CloseApp()
        {
            //_context.Dispose();
            Application.Current.Shutdown();
        }

        private void MaximizeApp()
        {
            SystemCommands.MaximizeWindow(Application.Current.MainWindow);
        }

        private void RestoreApp()
        {
            SystemCommands.RestoreWindow(Application.Current.MainWindow);
        }

        private void MinimizeApp()
        {
            SystemCommands.MinimizeWindow(Application.Current.MainWindow);
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private Visibility _myControlVisibility;
        public Visibility MyControlVisibility
        {
            get { return _myControlVisibility; }
            set
            {
                _myControlVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Команды для открытие Window

        private RelayCommand _openAddMatchWindowCommand;
        public RelayCommand OpenAddMatchWindowCommand { get => _openAddMatchWindowCommand ??= new(obj => OpenAddMatchWindow()); }

        private RelayCommand _openAddKillerWindowCommand;
        public RelayCommand OpenAddKillerWindowCommand { get => _openAddKillerWindowCommand ??= new(obj => OpenAddKillerWindow()); }

        private RelayCommand _openAddSurvivorWindowCommand;
        public RelayCommand OpenAddSurvivorWindowCommand { get => _openAddSurvivorWindowCommand ??= new(obj => OpenAddSurvivorWindow()); }

        private RelayCommand _openAddPerkWindowCommand;
        public RelayCommand OpenAddPerkWindowCommand { get => _openAddPerkWindowCommand ??= new(obj => OpenAddPerkWindow()); }

        private RelayCommand _openAddMapWindowCommand;
        public RelayCommand OpenAddMapWindowCommand { get => _openAddMapWindowCommand ??= new(obj => OpenAddMapWindow()); }

        private RelayCommand _openAddOfferingWindowCommand;
        public RelayCommand OpenAddOfferingWindowCommand { get => _openAddOfferingWindowCommand ??= new(obj => OpenAddOfferingWindow()); }

        private RelayCommand _openAddItemWindowCommand;
        public RelayCommand OpenAddItemWindowCommand { get => _openAddItemWindowCommand ??= new(obj => OpenAddItemWindow()); }

        private RelayCommand _openAboutTheProgramWindowCommand;
        public RelayCommand OpenAboutTheProgramWindowCommand { get => _openAboutTheProgramWindowCommand ??= new(obj => OpenAboutTheProgramWindow()); }

        private RelayCommand _openHowToUseWindowCommand;
        public RelayCommand OpenHowToUseWindowCommand { get => _openHowToUseWindowCommand ??= new(obj => OpenHowToUseWindow()); }

        private RelayCommand _openReportCreationWindowCommand;
        public RelayCommand OpenReportCreationWindowCommand { get => _openReportCreationWindowCommand ??= new(obj => OpenReportCreationWindow()); }

        private RelayCommand _openAdditionalDataWindowCommand;
        public RelayCommand OpenAdditionalDataWindowCommand { get => _openAdditionalDataWindowCommand ??= new(obj => OpenAddAdditionalDataWindow()); }
        
        private RelayCommand _openDataBackupWindowCommand;
        public RelayCommand OpenDataBackupWindowCommand { get => _openDataBackupWindowCommand ??= new(obj => OpenDataBackupWindow()); }

        //private RelayCommand _openSettingsWindowCommand;
        //public RelayCommand OpenSettingsWindowCommand { get => _openSettingsWindowCommand ??= new(obj => OpenSettingsWindow()); }

        #endregion

        #region Методы для открытие Window

        private void OpenAddMatchWindow()
        {
            _windowNavigationService.OpenWindow("AddMatchWindow");
        }

        private void OpenAddKillerWindow()
        {
            _windowNavigationService.OpenWindow("AddKillerWindow");
        }

        private void OpenAddSurvivorWindow()
        {
            _windowNavigationService.OpenWindow("AddSurvivorWindow");
        }

        private void OpenAddPerkWindow()
        {
            _windowNavigationService.OpenWindow("AddPerkWindow");
        }

        private void OpenAddMapWindow()
        {
            _windowNavigationService.OpenWindow("AddMapWindow");
        }

        private void OpenAddOfferingWindow()
        {
            _windowNavigationService.OpenWindow("AddOfferingWindow");
        }

        private void OpenAboutTheProgramWindow()
        {
            _windowNavigationService.OpenWindow("AboutTheProgramWindow");
        }

        private void OpenHowToUseWindow()
        {
            _windowNavigationService.OpenWindow("HowToUseWindow");
        }

        private void OpenReportCreationWindow()
        {
            _windowNavigationService.OpenWindow("ReportCreationWindow");
        }

        private void OpenAddItemWindow()
        {
            _windowNavigationService.OpenWindow("AddItemWindow");
        }

        private void OpenAddAdditionalDataWindow()
        {
            _windowNavigationService.OpenWindow("AddAdditionalDataWindow");
        }

        private void OpenDataBackupWindow()
        {
            _windowNavigationService.OpenWindow("DataBackupWindow");
        }

        private void SetTitle()
        {
            Title = "Аналитика статистики";
        }

        #endregion  

        private readonly IServiceProvider _serviceProvider;

        private readonly IPageNavigationService _pageNavigationService;
        private readonly IWindowNavigationService _windowNavigationService;

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _pageNavigationService = _serviceProvider.GetService<IPageNavigationService>();
            _windowNavigationService = _serviceProvider.GetService<IWindowNavigationService>();

            SetTitle();
        }

        #region Команды навигации по страницам

        private RelayCommand _navigateToMatchPageCommand;
        public RelayCommand NavigateToMatchPageCommand { get => _navigateToMatchPageCommand ??= new(obj => { NavigateToMatchPage(); }); }

        private RelayCommand _navigateToKillerPageCommand;
        public RelayCommand NavigateToKillerPageCommand { get => _navigateToKillerPageCommand ??= new(obj => { NavigateToKillerPage(); }); }

        private RelayCommand _navigateToSurvivorPageCommand;
        public RelayCommand NavigateToSurvivorPageCommand { get => _navigateToSurvivorPageCommand ??= new(obj => { NavigateToSurvivorPage(); }); }

        private RelayCommand _navigateToMapPageCommand;
        public RelayCommand NavigateToMapPageCommand { get => _navigateToMapPageCommand ??= new(obj => { NavigateToMapPage(); }); }

        private RelayCommand _navigateToOfferingPageCommand;
        public RelayCommand NavigateToOfferingPageCommand { get => _navigateToOfferingPageCommand ??= new(obj => { NavigateToOfferingPage(); }); }

        private RelayCommand _navigateToPerkPageCommand;
        public RelayCommand NavigateToPerkPageCommand { get => _navigateToPerkPageCommand ??= new(obj => { NavigateToPerkPage(); }); }

        #endregion

        #region Методы навигации по страницам

        private void NavigateToMatchPage()
        {
            _pageNavigationService.NavigateTo("MatchPage");
            Title = "Аналитика статистики - Матчи";
        }

        private void NavigateToKillerPage()
        {
            _pageNavigationService.NavigateTo("KillerPage");
            Title = "Аналитика статистики - Убийцы";
        }

        private void NavigateToSurvivorPage()
        {
            _pageNavigationService.NavigateTo("SurvivorPage");
            Title = "Аналитика статистики - Выжившие";
        }

        private void NavigateToMapPage()
        {
            _pageNavigationService.NavigateTo("MapPage");
            Title = "Аналитика статистики - Карты";
        }

        private void NavigateToOfferingPage()
        {
            _pageNavigationService.NavigateTo("OfferingPage");
            Title = "Аналитика статистики - Карты";
        }

        private void NavigateToPerkPage()
        {
            _pageNavigationService.NavigateTo("PerkPage");
            Title = "Аналитика статистики - Карты";
        }

        #endregion
    }

}

