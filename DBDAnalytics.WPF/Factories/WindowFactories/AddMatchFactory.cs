using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.ViewModels.WindowVM;
using DBDAnalytics.WPF.Views.Windows;
using System.Windows;

namespace DBDAnalytics.WPF.Factories.WindowFactories
{
    internal class AddMatchFactory(Func<AddMatchVM> viewModelFactory) : IWindowFactory
    {
        private readonly Func<AddMatchVM> _viewModelFactory = viewModelFactory;

        public Window CreateWindow(object parameter = null, TypeParameter typeParameter = TypeParameter.None)
        {
            var viewModel = _viewModelFactory();
            viewModel.Update(parameter, typeParameter);

            return new AddMatchWindow
            {
                DataContext = viewModel,
            };
        }
    }
}