using Microsoft.Xaml.Behaviors;
using Shared.WPF.ViewModels.Base;
using System.Windows;

namespace Shared.WPF.Behaviors
{
    public sealed class AsyncInitializeBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            base.OnDetaching();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.DataContext is IAsyncInitializable viewModel)
                await viewModel.InitializeAsync();
        }
    }
}