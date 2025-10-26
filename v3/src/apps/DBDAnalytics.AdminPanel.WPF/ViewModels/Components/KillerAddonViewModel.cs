using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.WPF.Helpers;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Components
{
    internal sealed class KillerAddonViewModel(KillerAddonResponse model) : ObservableObject<KillerAddonResponse>(model)
    {
        public string Id => _model.Id;
        public string KillerId => _model.KillerId;

        private string? _killerLocalId;
        public string? KillerLocalId
        {
            get => _killerLocalId;
            private set
            {
                SetProperty(ref _killerLocalId, value);
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

        private RarityResponse? _rarity;
        public RarityResponse? Rarity
        {
            get => _rarity;
            set
            {
                if (SetProperty(ref _rarity, value))
                    OnPropertyChanged(nameof(HasChanges));
            }
        }

        public bool HasChanges
        {
            get
            {
                var currentImageBytes = ImageHelper.ToBytes(Image);
                bool abilityChanged = !(_originalImageBytes ?? []).SequenceEqual(currentImageBytes ?? []);

                return abilityChanged
                    || _model.OldId != OldId
                    || _model.Name != Name;
            }
        }

        public override void CommitChanges(KillerAddonResponse newModel)
        {
            base.CommitChanges(newModel);

            _originalImageBytes = ImageHelper.ToBytes(Image);

            OnPropertyChanged(nameof(HasChanges));
        }

        public override void RevertChanges()
        {
            Name = _model.Name;
            OldId = _model.OldId;

            if (_originalImageBytes is not null)
            {
                Image = ImageHelper.ToBitmapImageFromBytes(_originalImageBytes);
                OnPropertyChanged(nameof(Image));
            }
            else
                Image = null;

            OnPropertyChanged(nameof(HasChanges));
        }

        public override KillerAddonResponse ToModel()
            => _model with
            {
                OldId = this.OldId,
                Name = this.Name
            };

        internal void SetImage(ImageSource? imageSource, string? path)
        {
            Image = imageSource;
            ImagePath = path;
        }

        internal void SetKillerLocalID(string? localId) => KillerLocalId = localId;

        internal void SetImageKey(string value) => ImageKey = value;
    }
}