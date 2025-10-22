using DBDAnalytics.AdminPanel.WPF.ViewModels.Pages;
using DBDAnalytics.AdminPanel.WPF.Views.Pages;
using Shared.WPF.Navigations.Pages;
using System.Windows.Controls;

namespace DBDAnalytics.AdminPanel.WPF.Factories.Pages
{
    internal sealed class KillerPerkCategoryPageFactory(Func<KillerPerkCategoryPageViewModel> vmFactory) : IPageFactory
    {
        private readonly Func<KillerPerkCategoryPageViewModel> _vmFactory = vmFactory;

        public Page CreatePage()
        {
            var vm = _vmFactory();

            var page = new KillerPerkCategoryPage { DataContext = vm, };

            return page;
        }
    }
}