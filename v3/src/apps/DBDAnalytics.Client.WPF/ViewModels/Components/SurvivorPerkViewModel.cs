using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.Client.WPF.ViewModels.Components
{
    internal sealed class SurvivorPerkViewModel(SurvivorPerkResponse model) : ObservableObject<SurvivorPerkResponse>(model)
    {
        public string Id => _model.Id;
        public string SurvivorId => _model.SurvivorId;
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

        private bool _isPinnedFirst = false;
        public bool IsPinnedFirst
        {
            get => _isPinnedFirst;
            set => SetProperty(ref _isPinnedFirst, value);
        }

        private bool _isPinnedSecond = false;
        public bool IsPinnedSecond
        {
            get => _isPinnedSecond;
            set => SetProperty(ref _isPinnedSecond, value);
        }

        private bool _isPinnedThird = false;
        public bool IsPinnedThird
        {
            get => _isPinnedThird;
            set => SetProperty(ref _isPinnedThird, value);
        }

        private bool _isPinnedFourth = false;
        public bool IsPinnedFourth
        {
            get => _isPinnedFourth;
            set => SetProperty(ref _isPinnedFourth, value);
        }

        public override void RevertChanges()
        {
            return;
        }

        public override SurvivorPerkResponse ToModel() => _model;

        public void SetImage(ImageSource? image) => Image = image;
    }
}