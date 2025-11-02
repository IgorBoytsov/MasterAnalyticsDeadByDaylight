using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.Client.WPF.ViewModels.Components
{
    internal sealed class KillerAddonViewModel(KillerAddonResponse model) : ObservableObject<KillerAddonResponse>(model)
    {
        public string Id => _model.Id;
        public string KillerId => _model.KillerId;
        public string Name => _model.Name;
        public string? ImageKey => _model.ImageKey;

        private ImageSource? _image;
        public ImageSource? Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private bool _isPinned;
        public bool IsPinned
        {
            get => _isPinned;
            set => SetProperty(ref _isPinned, value);
        }

        public override void RevertChanges()
        {
            return;
        }

        public override KillerAddonResponse ToModel() => _model;

        public void SetImage(ImageSource? image) => Image = image;
    }
}