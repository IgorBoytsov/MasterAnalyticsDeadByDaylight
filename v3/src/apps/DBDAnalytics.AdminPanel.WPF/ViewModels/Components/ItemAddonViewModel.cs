using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.WPF.Helpers;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Components
{
    internal sealed class ItemAddonViewModel(ItemAddonResponse model, IReadOnlyCollection<RarityResponse> allRarities) : ObservableObject<ItemAddonResponse>(model)
    {
        private readonly IReadOnlyCollection<RarityResponse> _allRarities = allRarities;

        public string Id => _model.Id;
        public string ItemId => _model.ItemId;
        public int? RarityId => _model.RarityId;

        private string? _localItemId;
        public string? LocalItemId
        {
            get => _localItemId;
            private set => SetProperty(ref _localItemId, value);
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

        private RarityResponse? _rarity = null!;
        public RarityResponse? Rarity
        {
            get => _rarity;
            set => SetProperty(ref _rarity, value);
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

            Rarity = _allRarities.FirstOrDefault(r => r.Id == _model.RarityId)!;

            if (_originalImageBytes is not null)
            {
                Image = ImageHelper.ToBitmapImageFromBytes(_originalImageBytes);
                OnPropertyChanged(nameof(Image));
            }
            else
                Image = null;
        }

        public override void CommitChanges(ItemAddonResponse newModel)
        {
            base.CommitChanges(newModel);

            _originalImageBytes = ImageHelper.ToBytes(Image);

            OnPropertyChanged(nameof(HasChanges));
        }

        public override ItemAddonResponse ToModel()
            => _model with
            {
                OldId = this.OldId,
                Name = this.Name,
                RarityId = this.Rarity?.Id,
            };

        public void SetImage(ImageSource? imageSource, string? path)
        {
            Image = imageSource;
            ImagePath = path;
        }

        public void SetRarity(int? rarityId) => Rarity = _allRarities.FirstOrDefault(r => r.Id == rarityId)!;

        internal void SetItemLocalId(string? localId) => LocalItemId = localId;

        internal void SetImageKey(string value) => ImageKey = value;
    }
}