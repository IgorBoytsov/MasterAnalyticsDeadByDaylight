using DBDAnalytics.AdminPanel.WPF.ViewModels.Pages;
using DBDAnalytics.AdminPanel.WPF.Views.Pages;
using Shared.WPF.Navigations.Pages;
using System.Windows.Controls;

namespace DBDAnalytics.AdminPanel.WPF.Factories.Pages
{
    internal sealed class WhoPlacedMapPageFactory(Func<WhoPlacedMapPageViewModel> vmFactory) : IPageFactory
    {
        private readonly Func<WhoPlacedMapPageViewModel> _vmFactory = vmFactory;

        public Page CreatePage()
        {
            var vm = _vmFactory();

            var page = new WhoPlacedMapPage { DataContext = vm };

            return page;
        }
    }
}