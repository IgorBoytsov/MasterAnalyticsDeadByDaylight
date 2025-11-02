using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Base;

namespace Shared.WPF.ViewModels.Components
{
    public sealed class DisplayCollectionController : BaseViewModel, IDisplayCollectionController
    {
        private DisplayCollectionType _currentType;
        public DisplayCollectionType CurrentType
        {
            get => _currentType;
            private set
            {
                SetProperty(ref _currentType, value);
                SetTypeCommand?.RaiseCanExecuteChanged();
            }
        }

        public DisplayCollectionController(DisplayCollectionType current)
        {
            CurrentType = current;
            SetTypeCommand = new RelayCommand<DisplayCollectionType>(Execute_SetType, CanExecute_SetType);
        }

        public RelayCommand<DisplayCollectionType> SetTypeCommand { get; private set; }

        private void Execute_SetType(DisplayCollectionType value) => CurrentType = value;

        private bool CanExecute_SetType(DisplayCollectionType value) => CurrentType != value;
    }
}