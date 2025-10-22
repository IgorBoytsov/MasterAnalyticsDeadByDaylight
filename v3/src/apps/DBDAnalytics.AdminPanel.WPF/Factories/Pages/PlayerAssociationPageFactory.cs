using DBDAnalytics.AdminPanel.WPF.ViewModels.Pages;
using DBDAnalytics.AdminPanel.WPF.Views.Pages;
using Shared.WPF.Navigations.Pages;
using System.Windows.Controls;

namespace DBDAnalytics.AdminPanel.WPF.Factories.Pages
{
    internal sealed class PlayerAssociationPageFactory(Func<PlayerAssociationPageViewModel> vmFactory) : IPageFactory
    {
        private readonly Func<PlayerAssociationPageViewModel> _vmFactory = vmFactory;

        public Page CreatePage()
        {
            var vm = _vmFactory();

            var page = new PlayerAssociationPage { DataContext = vm };  

            return page;
        }
    }
}