using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Base;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Shared.WPF.ViewModels.Components
{
    public class PopupController : BaseViewModel, IPopupController 
    {
        private readonly Func<bool>? _condition;

        public PopupController(PopupPlacementMode placementMode = PopupPlacementMode.Default, Func<bool>? condition = null)
        {
            if (placementMode == PopupPlacementMode.CustomRightUp)
                CustomPlacementCallback = PlacePopupRightUp;

            if (condition != null)
                _condition = condition;

            InitializeCommand();
        }

        /*--Свойства--------------------------------------------------------------------------------------*/

        #region Свойства [IsOpen, StaysOpen]

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

        #endregion

        #region Свойство [PlacementTarget] - UI элемент, к которому идет привязка Popup

        private UIElement? _placementTarget;
        public UIElement? PlacementTarget
        {
            get => _placementTarget;
            set => SetProperty(ref _placementTarget, value);
        }

        #endregion

        /*--Команды---------------------------------------------------------------------------------------*/
       
        private void InitializeCommand()
        {
            ShowCommand = new RelayCommand<UIElement>(Execute_Show, CanExecute_Show);
            HideCommand = new RelayCommand(Execute_Hide);
        }

        #region Команда [ShowCommand]: Открытия Popup

        public ICommand? ShowCommand { get; private set; }

        private void Execute_Show(UIElement target)
        {
            PlacementTarget = target;
            IsOpen = true;
        }

        private bool CanExecute_Show(UIElement target)
        {
            if (_condition is null) return true;
            return _condition();
        }

        #endregion

        #region Команда [HideCommand]: Закрытия Popup

        public ICommand? HideCommand { get; private set; }

        private void Execute_Hide()
        {
            IsOpen = false;
            PlacementTarget = null;
        }

        #endregion

        /*--Дополнительная логика-------------------------------------------------------------------------*/
       
        #region Логика динамических размеров и позиционирования

        private double _height = double.NaN;
        public double Height 
        { 
            get => _height; 
            set => SetProperty(ref _height, value); 
        }

        private double _maxHeight = double.PositiveInfinity;
        public double MaxHeight 
        { 
            get => _maxHeight; 
            set => SetProperty(ref _maxHeight, value); 
        }

        public CustomPopupPlacementCallback? CustomPlacementCallback { get; private set; }

        public void UpdatePopupSize(double containerActualHeight, double contentActualHeight = 0)
        {
            if (containerActualHeight > 100)
                MaxHeight = containerActualHeight - 100;

            if (contentActualHeight > 0)
                Height = contentActualHeight + 100;
        }

        private CustomPopupPlacement[] PlacePopupRightUp(Size popupSize, Size targetSize, Point offset)
        {
            if (PlacementTarget is FrameworkElement target)
            {
                double xOffset = target.ActualWidth;
                double yOffset = 0;
                return [new CustomPopupPlacement(new Point(xOffset + 10, (yOffset - popupSize.Height) + 25), PopupPrimaryAxis.Vertical)];
            }
            return [new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Vertical)];
        }

        #endregion
    }
}