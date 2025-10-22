using DBDAnalytics.AdminPanel.WPF.ViewModels.Pages;
using DBDAnalytics.AdminPanel.WPF.Views.Pages;
using Shared.WPF.Navigations.Pages;
using System.Windows.Controls;

namespace DBDAnalytics.AdminPanel.WPF.Factories.Pages
{
    internal sealed class RolePageFactory(Func<RolePageViewModel> vmFacrtory) : IPageFactory
    {
        private readonly Func<RolePageViewModel> _vmFactory = vmFacrtory;

        public Page CreatePage()
        {
            var vm = _vmFactory();

            var page = new RolePage { DataContext = vm };

            return page;
        }
    }
}