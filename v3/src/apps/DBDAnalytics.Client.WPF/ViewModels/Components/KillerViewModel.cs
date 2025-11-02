using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.Client.WPF.ViewModels.Components
{
    internal sealed class KillerViewModel(KillerSoloResponse model) : ObservableObject<KillerSoloResponse>(model)
    {
        public string Id => _model.Id;
        public string Name => _model.Name;
        public string? KillerImageKey => _model.KillerImageKey;
        public string? AbilityImageKey => _model.AbilityImageKey;

        private bool _isPinned = false;
        public bool IsPinned
        {
            get => _isPinned;
            set => SetProperty(ref _isPinned, value);
        }

        private ImageSource? _image;
        public ImageSource? Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public override KillerSoloResponse ToModel() => _model;

        public override void RevertChanges()
        {
            return;
        }

        public void SetImage(ImageSource? image) => Image = image;
    }
}