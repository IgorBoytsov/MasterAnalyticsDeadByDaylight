using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using DBDAnalytics.WPF.ViewModels.WindowVM;
using DBDAnalytics.WPF.Views.Windows;
using System.Windows;

namespace DBDAnalytics.WPF.Factories.WindowFactories
{
    internal class InteractionGameEventFactory(Func<InteractionGameEventVM> viewModelFactory) : IWindowFactory
    {
        private readonly Func<InteractionGameEventVM> _viewModelFactory = viewModelFactory;

        public Window CreateWindow(object? parameter = null, TypeParameter typeParameter = TypeParameter.None)
        {
            var viewModel = _viewModelFactory();
            viewModel.Update(parameter, typeParameter);

            return new InteractionGameEventWindow
            {
                DataContext = viewModel,
            };
        }
    }
}
