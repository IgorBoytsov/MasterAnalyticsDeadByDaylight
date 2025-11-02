using DBDAnalytics.Shared.Contracts.Responses.Offering;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.Client.WPF.ViewModels.Components
{
    internal sealed class OfferingViewModel(OfferingResponse model) : ObservableObject<OfferingResponse>(model)
    {
        public string Id => _model.Id;
        public string Name => _model.Name;
        public string ImageKey => _model.ImageKey;
        public int RoleId => _model.RoleId;

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
            throw new NotImplementedException();
        }

        public override OfferingResponse ToModel()
        {
            throw new NotImplementedException();
        }

        public void SetImage(ImageSource? image) => Image = image;
    }
}