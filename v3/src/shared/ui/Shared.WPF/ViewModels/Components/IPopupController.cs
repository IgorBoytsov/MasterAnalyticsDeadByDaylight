using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Shared.WPF.ViewModels.Components
{
    public interface IPopupController : INotifyPropertyChanged
    {
        bool IsOpen { get; set; }
        bool StaysOpen { get; set; }
        UIElement? PlacementTarget { get; set; }

        ICommand ShowCommand { get; }
        ICommand HideCommand { get; }
    }
}