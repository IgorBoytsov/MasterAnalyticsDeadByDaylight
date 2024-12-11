using MasterAnalyticsDeadByDaylight.MVVM.View.Pages;
using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation
{
    public class PageNavigationService : IPageNavigationService
    {
        private Frame _frame;

        private Dictionary<string, Page> _pages = new();

        private readonly IServiceProvider _serviceProvider;

        public PageNavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SetFrame(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public void NavigateTo(string pageName, object parameter = null, bool onlyUpdate = false)
        {
            if (_pages.TryGetValue(pageName, out var pageExist))
            {
                if (pageExist.DataContext is IUpdatable viewModel)
                {
                    viewModel.Update(parameter);
                    _frame.Navigate(pageExist);
                }
                return;
            }

            if (!onlyUpdate)
            {
                Open(pageName, parameter);
            }
        }

        private void Open(string pageName, object parameter = null)
        {
            Action action = pageName switch
            {
                "KillerPage" => () =>
                {
                    var viewModel = new KillerPageViewModel(_serviceProvider);
                    var page = new KillerPage()
                    {
                        DataContext = viewModel,
                    };
                    _pages.TryAdd(pageName, page);
                    _frame.Navigate(page);                 
                }
                ,
                "MapPage" => () =>
                {
                    var viewModel = new MapPageViewModel(_serviceProvider);
                    var page = new MapPage()
                    {
                        DataContext = viewModel,
                    };
                    _frame.Navigate(page);
                    _pages.TryAdd(pageName, page);
                }
                ,
                "MatchPage" => () =>
                {
                    var viewModel = new MatchPageViewModel(_serviceProvider);
                    var page = new MatchPage()
                    {
                        DataContext = viewModel,
                    };
                    _frame.Navigate(page);
                    _pages.TryAdd(pageName, page);
                }
                ,
                "OfferingPage" => () =>
                {
                    var viewModel = new OfferingPageViewModel(_serviceProvider);
                    var page = new OfferingPage()
                    {
                        DataContext = viewModel,
                    };
                    _frame.Navigate(page);
                    _pages.TryAdd(pageName, page);
                }
                ,
                "PerkPage" => () =>
                {
                    var viewModel = new PerkPageViewModel(_serviceProvider);
                    var page = new PerkPage()
                    {
                        DataContext = viewModel,
                    };
                    _frame.Navigate(page);
                    _pages.TryAdd(pageName, page);
                }
                ,
                "SurvivorPage" => () =>
                {
                    var viewModel = new SurvivorPageViewModel(_serviceProvider);
                    var page = new SurvivorPage()
                    {
                        DataContext = viewModel,
                    };
                    _frame.Navigate(page);
                    _pages.TryAdd(pageName, page);
                }
                ,
                _ => () => throw new Exception("Окно отсутствует")
            };
            action?.Invoke();
        }

        //private Frame _frame;

        //private readonly Page KillerStat = new KillerPage();
        //private readonly Page Map = new MapPage();
        //private readonly Page Match = new MatchPage();
        //private readonly Page Offering = new OfferingPage();
        //private readonly Page Perk = new PerkPage();
        //private readonly Page Survivor = new SurvivorPage();

        //public void SetFrame(Frame frame)
        //{
        //    _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        //}

        //public void NavigateTo(string pageName, object parameter = null)
        //{
        //    switch (pageName)
        //    {
        //        case "MatchPage":
        //            _frame.Navigate(Match);
        //            break;

        //        case "KillerPage":
        //            _frame.Navigate(KillerStat);
        //            break;

        //        case "SurvivorPage":
        //            _frame.Navigate(Survivor);
        //            break;

        //        case "MapPage":
        //            _frame.Navigate(Map);
        //            break;

        //        case "OfferingPage":
        //            _frame.Navigate(Offering);
        //            break;

        //        case "PerkPage":
        //            _frame.Navigate(Perk);
        //            break;

        //        default:
        //            throw new ArgumentException($"Страница '{pageName}' не найдена.");
        //    }
        //}
    }
}