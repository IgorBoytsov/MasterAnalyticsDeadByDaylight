using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace Shared.WPF.ViewModels.Components
{
    public class PopupController : BaseViewModel, IPopupController 
    {
        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set => SetProperty(ref _isOpen, value);
        }

        private bool _staysOpen = false;
        public bool StaysOpen
        {
            get => _staysOpen;
            set => SetProperty(ref _staysOpen, value);
        }

        private UIElement? _placementTarget;
        public UIElement? PlacementTarget
        {
            get => _placementTarget;
            set => SetProperty(ref _placementTarget, value);
        }

        public ICommand ShowCommand { get; private set; }
        public ICommand HideCommand { get; private set; }

        public PopupController()
        {
            ShowCommand = new RelayCommand<object>(Execute_Show);
            HideCommand = new RelayCommand(Execute_Hide);
        }

        private void Execute_Show(object parameter)
        {
            if (parameter is UIElement target)
            {
                PlacementTarget = target;
                IsOpen = true;
            }
        }

        private void Execute_Hide()
        {
            IsOpen = false;
            PlacementTarget = null;
        }
    }
}