using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.Client.WPF.ViewModels.Components
{
    internal sealed class SurvivorViewModel(SurvivorSoloResponse model) : ObservableObject<SurvivorSoloResponse>(model)
    {
        public string Id => _model.Id;
        public string Name => _model.Name;
        public string? ImageKey => _model.ImageKey;

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

        public override void RevertChanges()
        {
            return;
        }

        public override SurvivorSoloResponse ToModel() => _model;

        public void SetImage(ImageSource? image) => Image = image;
    }
}