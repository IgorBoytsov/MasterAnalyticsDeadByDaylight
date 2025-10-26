using DBDAnalytics.Shared.Contracts.Responses.Maps;
using Shared.WPF.Helpers;
using Shared.WPF.ViewModels.Base;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Components
{
    class MapViewModel(MapResponse model) : ObservableObject<MapResponse>(model)
    {
        public string Id => _model.Id;
        public string MeasurementId => _model.MeasurementId;

        private string? _measurementLocalId;
        public string? MeasurementLocalId
        {
            get => _measurementLocalId;
            private set
            {
                SetProperty(ref _measurementLocalId, value);
            }
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

        private string? _imageKey = model.ImageKey;
        public string? ImageKey
        {
            get => _imageKey;
            private set => SetProperty(ref _imageKey, value);
        }

        private string? _imagePath;
        public string? ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
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

        private bool _hasSave;
        public bool HasSave
        {
            get => _hasSave;
            set => SetProperty(ref _hasSave, value);
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
            Name = _model.Name;
            OldId = _model.OldId;

            if (_originalImageBytes is not null)
            {
                using var stream = new MemoryStream(_originalImageBytes);
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                _image = bitmapImage;
                OnPropertyChanged(nameof(Image));
            }
            else
                Image = null;

            OnPropertyChanged(nameof(HasChanges));
        }

        public override void CommitChanges(MapResponse newModel)
        {
            base.CommitChanges(newModel);

            _originalImageBytes = ImageHelper.ToBytes(Image);

            OnPropertyChanged(nameof(HasChanges));
        }

        public override MapResponse ToModel()
            => _model with
            {
                Id = this.Id,
                OldId = this.OldId,
                Name = this.Name,
                ImageKey = this.ImageKey,
                MeasurementId = this.MeasurementId,
            };

        public void ChangeSaveStatus(bool hasSve) => HasSave = hasSve;

        public void SetMeasurementLocalId(string id) => MeasurementLocalId = id;

        public void SetImage(ImageSource? image, string imagePath)
        {
            ImagePath = imagePath;
            Image = image;
        }

        internal void SetImageKey(string value) => ImageKey = value;
    }
}