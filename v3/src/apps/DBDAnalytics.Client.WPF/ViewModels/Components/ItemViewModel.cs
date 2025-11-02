using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.Client.WPF.ViewModels.Components
{
    internal sealed class ItemViewModel(ItemSoloResponse model) : ObservableObject<ItemSoloResponse>(model)
    {
        public string Id => _model.Id;
        public string Name => _model.Name;
        public string? ImageKey => _model.ImageKey;

        private ImageSource? _image;
        public ImageSource? Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private bool _isPinned = false;
        public bool IsPinned
        {
            get => _isPinned;
            set => SetProperty(ref _isPinned, value);
        }

        public override void RevertChanges()
        {
            return;
        }

        public override ItemSoloResponse ToModel() => _model;

        internal void SetImage(ImageSource? image) => Image = image;
    }
}