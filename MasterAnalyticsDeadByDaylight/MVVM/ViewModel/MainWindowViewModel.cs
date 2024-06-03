using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow;
using MasterAnalyticsDeadByDaylight.MVVM.View.Windows.ModalWindow;
using MasterAnalyticsDeadByDaylight.Services.NavigationService.WindowNavigation;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
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

        public string Titel { get; set; }

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

        private RelayCommand _openErrorWindowCommand;
        public RelayCommand OpenErrorWindowCommand { get => _openErrorWindowCommand ??= new(obj => OpenErrorWindow(null)); }

        private RelayCommand _openNotificationWindowCommand;
        public RelayCommand OpenNotificationWindowCommand { get => _openNotificationWindowCommand ??= new(obj => OpenNotificationWindow(null)); }

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

        #endregion

        #region Методы для открытие Window

        private static void OpenErrorWindow(string error)
        {
            ErrorWindow errorWindow = new();
            errorWindow.ShowDialog();
        }

        private static void OpenNotificationWindow(string message)
        {
            NotificationWindow notification = new();
            notification.ShowDialog();
        }

        private void OpenAddMatchWindow()
        {
            AddMatchWindow addMatchWindow = new AddMatchWindow();
            addMatchWindow.Show();
        }

        private static void OpenAddKillerWindow()
        {
            AddKillerWindow addKillerWindow = new AddKillerWindow();
            addKillerWindow.ShowDialog();
        }

        private static void OpenAddSurvivorWindow()
        {
            AddSurvivorWindow addSurvivorWindow = new AddSurvivorWindow();
            addSurvivorWindow.ShowDialog();
        }

        private static void OpenAddPerkWindow()
        {
            AddPerkWindow addPerkWindow = new AddPerkWindow();
            addPerkWindow.ShowDialog();
        }

        private static void OpenAddMapWindow()
        {
            AddMapWindow addMapWindow = new AddMapWindow();
            addMapWindow.ShowDialog();
        }

        private static void OpenAddOfferingWindow()
        {
            AddOfferingWindow addOfferingWindow = new AddOfferingWindow();
            addOfferingWindow.ShowDialog();
        }

        private static void OpenAboutTheProgramWindow()
        {
            AboutTheProgramWindow aboutTheProgramWindow = new AboutTheProgramWindow();
            aboutTheProgramWindow.ShowDialog();
        }

        private static void OpenHowToUseWindow()
        {
            HowToUseWindow howToUseWindow = new HowToUseWindow();
            howToUseWindow.Show();
        }

        private static void OpenReportCreationWindow()
        {
            ReportCreationWindow reportCreationWindow = new ReportCreationWindow();
            reportCreationWindow.ShowDialog();
        }

        private static void OpenAddItemWindow()
        {
            AddItemWindow addItemWindow = new AddItemWindow();
            addItemWindow.ShowDialog();
        }

        private static void OpenAddAdditionalDataWindow()
        {
            AddAdditionalDataWindow addAdditionalDataWindow = new AddAdditionalDataWindow();
            addAdditionalDataWindow.ShowDialog();
        }

        private void SetTitle()
        {
            Titel = "Аналитика статистики";
        }

        #endregion  

        public MainWindowViewModel()
        {
            GameMatchList = [];
            SetTitle();
            GetGameStatisticData();
        }

        #region Матчи Киллера

        #region Колекции 

        public ObservableCollection<GameStatistic> GameMatchList { get; set; }

        #endregion

        #region Команды

        private RelayCommand _updateMatchCommand;
        public RelayCommand UpdateMatchCommand { get => _updateMatchCommand ??= new(obj => { GetGameStatisticData(); }); }

        #endregion

        #region Методы Получение данных

        private void GetGameStatisticData()
        {
            new Thread(() =>
            {
                using (MasterAnalyticsDeadByDaylightDbContext context = new())
                {
                    var games = 
                    context.GameStatistics
      
                    .Include(killerInfo => killerInfo.IdKillerNavigation)
                    .ThenInclude(killer => killer.IdKillerNavigation)

                    .Include(killer => killer.IdKillerNavigation)
                    .ThenInclude(KillerPerk => KillerPerk.IdPerk1Navigation)

                    .Include(killer => killer.IdKillerNavigation)
                    .ThenInclude(KillerPerk => KillerPerk.IdPerk2Navigation)

                    .Include(killer => killer.IdKillerNavigation)
                    .ThenInclude(KillerPerk => KillerPerk.IdPerk3Navigation)

                    .Include(killer => killer.IdKillerNavigation)
                    .ThenInclude(KillerPerk => KillerPerk.IdPerk4Navigation)

                    .Include(killer => killer.IdKillerNavigation)
                    .ThenInclude(KillerPerk => KillerPerk.IdAddon1Navigation)

                    .Include(killer => killer.IdKillerNavigation)
                    .ThenInclude(KillerPerk => KillerPerk.IdAddon2Navigation)

                    .Include(killer => killer.IdKillerNavigation)
                    .ThenInclude(KillerPerk => KillerPerk.IdAssociationNavigation)

                    .Include(killer => killer.IdKillerNavigation)
                    .ThenInclude(KillerPerk => KillerPerk.IdPlatformNavigation)

                    .Include(killer => killer.IdKillerNavigation)
                    .ThenInclude(KillerPerk => KillerPerk.IdKillerOfferingNavigation)

                    .Include(survivor => survivor.IdSurvivors1Navigation)
                    .ThenInclude(survivor => survivor.IdSurvivorNavigation)

                    .Include(survivor => survivor.IdSurvivors2Navigation)
                    .ThenInclude(survivor => survivor.IdSurvivorNavigation)

                    .Include(survivor => survivor.IdSurvivors3Navigation)
                    .ThenInclude(survivor => survivor.IdSurvivorNavigation)

                    .Include(survivor => survivor.IdSurvivors4Navigation)
                    .ThenInclude(survivor => survivor.IdSurvivorNavigation)

                    .ToList();
  
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        GameMatchList.Clear();
                        foreach (var entity in games)
                        {
                            GameMatchList.Add(entity);
                        }
                    });
                }
            }).Start();
        }

        #endregion


        #endregion
    }

}

