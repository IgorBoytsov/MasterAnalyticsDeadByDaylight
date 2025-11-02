using Shared.WPF.Commands;
using Shared.WPF.Enums;

namespace Shared.WPF.ViewModels.Components
{
    public interface IDisplayCollectionController
    {
        public DisplayCollectionType CurrentType { get; }
        RelayCommand<DisplayCollectionType> SetTypeCommand { get; }
    }
}