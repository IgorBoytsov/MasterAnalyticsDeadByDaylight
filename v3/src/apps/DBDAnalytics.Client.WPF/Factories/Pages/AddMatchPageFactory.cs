using DBDAnalytics.Client.WPF.ViewModels.Pages;
using DBDAnalytics.Client.WPF.Views.Pages;
using Shared.WPF.Navigations.Pages;
using System.Windows.Controls;

namespace DBDAnalytics.Client.WPF.Factories.Pages
{
    internal sealed class AddMatchPageFactory(Func<AddMatchPageViewModel> vmFactory) : IPageFactory
    {
        private readonly Func<AddMatchPageViewModel> _vmFactory = vmFactory;

        public Page CreatePage()
        {
            var vm = _vmFactory();
            var page = new AddMatchPage { DataContext = vm };
            return page;   
        }
    }
}