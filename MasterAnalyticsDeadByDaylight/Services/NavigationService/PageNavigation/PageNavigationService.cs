using MasterAnalyticsDeadByDaylight.MVVM.View.Pages;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation
{
    public class PageNavigationService : IPageNavigationService
    {
        private readonly Frame _frame;

        private static readonly Page Match = new MatchPage();
        private static readonly Page KillerStat = new KillerPage();
        private static readonly Page MyKillerStat = new KillerPageMyDetailedStatisticsPage();
        private static readonly Page OpponentKillerStat = new KillerPageOpponentDetailedStatisticsPage();
        private static readonly Page Survivor = new SurvivorPage();
        private static readonly Page Map = new MapPage();

        public PageNavigationService(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public void NavigateTo(string pageName, object parameter = null)
        {
            switch (pageName)
            {
                case "MatchPage":
                    _frame.Navigate(Match);
                    break;

                case "KillerPage":
                    _frame.Navigate(KillerStat);
                    break;

                case "MyKiller":
                    _frame.Navigate(MyKillerStat);
                    break;

                case "OpponentKiller":
                    _frame.Navigate(OpponentKillerStat);
                    break;

                case "SurvivorPage":
                    _frame.Navigate(Survivor);
                    break;

                case "MapPage":
                    _frame.Navigate(Map);
                    break;

                default:
                    throw new ArgumentException($"Страница '{pageName}' не найдена.");
            }
        }
    }
}
