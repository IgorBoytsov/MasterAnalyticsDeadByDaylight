using DBDAnalytics.AdminPanel.WPF.ViewModels.Pages;
using DBDAnalytics.AdminPanel.WPF.Views.Pages;
using Shared.WPF.Navigations.Pages;
using System.Windows.Controls;

namespace DBDAnalytics.AdminPanel.WPF.Factories.Pages
{
    internal sealed class GameModePageFactory(Func<GameModePageViewModel> vmFactory) : IPageFactory
    {
        private readonly Func<GameModePageViewModel> _vmFactory = vmFactory;

        public Page CreatePage()
        {
            var vm = _vmFactory();

            var page = new GameModePage { DataContext = vm };

            return page;
        }
    }
}