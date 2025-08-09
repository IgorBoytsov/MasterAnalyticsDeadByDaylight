using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.ViewModels.WindowVM;
using DBDAnalytics.WPF.Views.Windows;
using System.Windows;

namespace DBDAnalytics.WPF.Factories.WindowFactories
{
    internal class InteractionKillerPerkCategoryFactory(Func<InteractionKillerPerkCategoryVM> viewModelFactory) : IWindowFactory
    {
        private readonly Func<InteractionKillerPerkCategoryVM> _viewModelFactory = viewModelFactory;

        public Window CreateWindow(object parameter = null, TypeParameter typeParameter = TypeParameter.None)
        {
            var viewModel = _viewModelFactory();
            viewModel.Update(parameter, typeParameter);

            return new InteractionKillerPerkCategoryWindow
            {
                DataContext = viewModel,
            };
        }
    }
}
