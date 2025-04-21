using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.ViewModels.PageVM;
using DBDAnalytics.WPF.Views.Pages;
using System.Windows.Controls;

namespace DBDAnalytics.WPF.Factories.PageFactories
{
    class MapDetailsFactory(Func<MapDetailsVM> viewModel) : IPageFactory
    {
        private readonly Func<MapDetailsVM> _viewModelFactory = viewModel;

        public Page CreatePage(object parameter = null, TypeParameter typeParameter = TypeParameter.None)
        {
            var viewModel = _viewModelFactory();
            viewModel.Update(parameter, typeParameter);

            return new MapDetails
            {
                DataContext = viewModel,
            };
        }
    }
}