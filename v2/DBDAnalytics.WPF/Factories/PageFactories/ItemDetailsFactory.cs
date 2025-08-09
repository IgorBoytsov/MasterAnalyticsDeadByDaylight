using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.ViewModels.PageVM;
using DBDAnalytics.WPF.Views.Pages;
using System.Windows.Controls;

namespace DBDAnalytics.WPF.Factories.PageFactories
{
    internal class ItemDetailsFactory(Func<ItemDetailsVM> viewModel) : IPageFactory
    {
        private readonly Func<ItemDetailsVM> _viewModelFactory = viewModel;

        public Page CreatePage(object parameter = null, TypeParameter typeParameter = TypeParameter.None)
        {
            var viewModel = _viewModelFactory();
            viewModel.Update(parameter, typeParameter);

            return new ItemDetails
            {
                DataContext = viewModel,
            };
        }
    }
}