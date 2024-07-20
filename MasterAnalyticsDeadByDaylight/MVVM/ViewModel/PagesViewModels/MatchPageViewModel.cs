using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels
{
    internal class MatchPageViewModel : BaseViewModel
    {
        #region Колекции 

        public ObservableCollection<GameStatistic> GameMatchList { get; set; }

        #endregion

        #region Свойства

        //private GameStatistic _selectedGameMatch;
        //public GameStatistic SelectedGameMatch
        //{
        //    get => _selectedGameMatch;
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _selectedGameMatch = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        #endregion

        public MatchPageViewModel() 
        {
            GameMatchList = [];
            GetGameStatisticData();
        }

        #region Команды

        private RelayCommand _updateMatchCommand;
        public RelayCommand UpdateMatchCommand { get => _updateMatchCommand ??= new(obj => { GetGameStatisticData(); }); }

        private RelayCommand _showMatchCommand;
        public RelayCommand ShowMatchCommand => _showMatchCommand ??= new RelayCommand(ShowMatch);

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

                    .OrderByDescending(match => match.DateTimeMatch)

                    .ToList();

                    Application.Current.Dispatcher.Invoke(() =>
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


        private void ShowMatch(object parameter)
        {
            if (parameter is GameStatistic selectedGameMatch)
            {
                MessageBox.Show(selectedGameMatch.IdGameStatistic.ToString());
            }
        }

        #endregion
    }
}
