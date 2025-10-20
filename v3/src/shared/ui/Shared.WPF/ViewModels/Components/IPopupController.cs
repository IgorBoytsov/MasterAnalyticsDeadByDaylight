using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Shared.WPF.ViewModels.Components
{
    public interface IPopupController : INotifyPropertyChanged
    {
        bool IsOpen { get; set; }
        bool StaysOpen { get; set; }
        UIElement? PlacementTarget { get; set; }

        ICommand? ShowCommand { get; }
        ICommand? HideCommand { get; }

        CustomPopupPlacementCallback? CustomPlacementCallback { get; }
        double Height { get; set; }
        double MaxHeight { get; set; }
        void UpdatePopupSize(double containerActualHeight, double contentActualHeight = 0);
    }
}