using DBDAnalytics.Client.WPF.ViewModels.Pages;
using DBDAnalytics.Client.WPF.Views.Pages;
using Shared.WPF.Navigations.Pages;
using System.Windows.Controls;

namespace DBDAnalytics.Client.WPF.Factories.Pages
{
    internal sealed class MatchesPageFactory(Func<MatchesPageViewModel> vmFactory) : IPageFactory
    {
        private readonly Func<MatchesPageViewModel> _vmFactory = vmFactory;

        public Page CreatePage()
        {
            var vm = _vmFactory();
            var page = new MatchesPage { DataContext = vm };
            return page;
        }
    }
}