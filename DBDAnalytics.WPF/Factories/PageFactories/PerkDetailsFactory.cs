using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.ViewModels.PageVM;
using DBDAnalytics.WPF.Views.Pages;
using System.Windows.Controls;

namespace DBDAnalytics.WPF.Factories.PageFactories
{
    internal class PerkDetailsFactory(Func<PerkDetailsVM> viewModel) : IPageFactory
    {
        private readonly Func<PerkDetailsVM> _viewModelFactory = viewModel;

        public Page CreatePage(object parameter = null, TypeParameter typeParameter = TypeParameter.None)
        {
            var viewModel = _viewModelFactory();
            viewModel.Update(parameter, typeParameter);

            return new PerkDetails
            {
                DataContext = viewModel,
            };
        }
    }
}