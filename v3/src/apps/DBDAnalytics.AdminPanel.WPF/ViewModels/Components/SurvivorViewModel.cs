using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.WPF.Helpers;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Components
{
    internal sealed class SurvivorViewModel(SurvivorSoloResponse model) : ObservableObject<SurvivorSoloResponse>(model)
    {
        public string Id => _model.Id;

        private string? _localId;
        public string? LocalId
        {
            get => _localId;
            private set => SetProperty(ref _localId, value);
        }

        private int _oldId = model.OldId;
        public int OldId
        {
            get => _oldId;
            set
            {
                if (SetProperty(ref _oldId, value))
                    OnPropertyChanged(nameof(HasChanges));
            }
        }

        private string _name = model.Name;
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                    OnPropertyChanged(nameof(HasChanges));
            }
        }

        private byte[]? _originalImageBytes;

        private ImageSource? _image;
        public ImageSource? Image
        {
            get => _image;
            set
            {
                if (SetProperty(ref _image, value))
                {
                    _originalImageBytes ??= ImageHelper.ToBytes(value);

                    OnPropertyChanged(nameof(HasChanges));
                }
            }
        }

        private string? _imageKey = model.ImageKey;
        public string? ImageKey
        {
            get => _imageKey;
            set
            {
                SetProperty(ref _imageKey, value);
            }
        }

        private string? _imagePath;
        public string? ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }

        public bool HasChanges
        {
            get
            {
                var currentImageBytes = ImageHelper.ToBytes(Image);

                bool imageChanged = !(_originalImageBytes ?? []).SequenceEqual(currentImageBytes ?? []);

                return imageChanged
                    || _model.OldId != OldId
                    || _model.Name != Name;
            }
        }

        public override void RevertChanges()
        {
            OldId = _model.OldId;
            Name = _model.Name;

            if (_originalImageBytes is not null)
            {
                Image = ImageHelper.ToBitmapImageFromBytes(_originalImageBytes);
                OnPropertyChanged(nameof(Image));
            }
            else
                Image = null;
        }

        public override void CommitChanges(SurvivorSoloResponse newModel)
        {
            base.CommitChanges(newModel);

            _originalImageBytes = ImageHelper.ToBytes(Image);

            OnPropertyChanged(nameof(HasChanges));
        }

        public override SurvivorSoloResponse ToModel()
            => _model with
            {
                OldId = this.OldId,
                Name = this.Name
            };

        public void SetImage(ImageSource? imageSource, string? path)
        {
            Image = imageSource;
            ImagePath = path;
        }

        public void SetLocalID(Guid id) => LocalId = id.ToString();

        internal void SetImageKey(string value) => ImageKey = value;
    }
}