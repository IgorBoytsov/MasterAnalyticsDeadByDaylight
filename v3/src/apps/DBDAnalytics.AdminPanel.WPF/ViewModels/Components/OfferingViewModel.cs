using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using Shared.WPF.Helpers;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Components
{
    internal class OfferingViewModel(
        OfferingResponse model,
        IReadOnlyCollection<RoleResponse> allRoles,
        IReadOnlyCollection<RarityResponse> allRarities,
        IReadOnlyCollection<OfferingCategoryResponse> allCategories) : ObservableObject<OfferingResponse>(model)
    {
        private readonly IReadOnlyCollection<RoleResponse> _allRoles = allRoles;
        private readonly IReadOnlyCollection<RarityResponse> _allRarities = allRarities;
        private readonly IReadOnlyCollection<OfferingCategoryResponse> _allCategories = allCategories;

        private byte[]? _originalImageBytes;

        public string Id => _model.Id;
        public int RoleId => _model.RoleId;
        public int? RarityId => _model.RarityId;
        public int? CategoryId => _model.CategoryId;

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
            set => SetProperty(ref _imageKey, value);
        }

        private string _imagePath = null!;
        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }

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

        private RoleResponse _role = null!;
        public RoleResponse Role
        {
            get => _role;
            set
            {
                if(SetProperty(ref _role, value))
                    OnPropertyChanged(nameof(HasChanges));
            }
        }

        private RarityResponse? _rarity = null!;
        public RarityResponse? Rarity
        {
            get => _rarity;
            set
            {
                if (SetProperty(ref _rarity, value))
                    OnPropertyChanged(nameof(HasChanges));
            }
        }

        private OfferingCategoryResponse? _category = null!;
        public OfferingCategoryResponse? Category
        {
            get => _category;
            set
            {
                if(SetProperty(ref _category, value))
                    OnPropertyChanged(nameof(HasChanges));
            }
        }

        public bool HasChanges
        {
            get
            {
                var currentImageBytes = ImageHelper.ToBytes(Image);
                bool imageChanged = !(_originalImageBytes ?? []).SequenceEqual(currentImageBytes ?? []);

                return imageChanged
                       || _model.OldId != OldId
                       || _model.Name != Name
                       || _model.RoleId != Role.Id
                       || _model.RarityId != Rarity?.Id
                       || _model.CategoryId != Category?.Id;
            }
        }

        public override void RevertChanges()
        {
            OldId = _model.OldId;
            Name = _model.Name;

            Role = _allRoles.FirstOrDefault(r => r.Id == _model.RoleId)!;
            Rarity = _allRarities.FirstOrDefault(r => r.Id == _model.RarityId);
            Category = _allCategories.FirstOrDefault(c => c.Id == _model.CategoryId);

            if (_originalImageBytes is not null)
            {
                _image = ImageHelper.ToBitmapImageFromBytes(_originalImageBytes);
                OnPropertyChanged(nameof(Image));
            }
            else
                Image = null;

            OnPropertyChanged(nameof(HasChanges));
        }

        public override void CommitChanges(OfferingResponse newModel)
        {
            base.CommitChanges(newModel);

            _originalImageBytes = ImageHelper.ToBytes(Image);

            OnPropertyChanged(nameof(HasChanges));
        }

        public override OfferingResponse ToModel()
            => _model with
            {
                Id = this.Id,
                OldId = this.OldId,
                Name = this.Name,
                ImageKey = this.ImageKey,
                RoleId = this.Role.Id,
                RarityId = this.Rarity?.Id,
                CategoryId = this.Category?.Id,
            };

        public void SetImage(ImageSource? image, string imagePath)
        {
            Image = image;
            ImagePath = imagePath;
        }

        public void SetRole(RoleResponse role)
        {
            if (role is null)
                throw new Exception("Не удалось установить роль");

            Role = role;
        }

        public void SetRarity(RarityResponse? rarity) => Rarity = rarity!;

        public void SetCategory(OfferingCategoryResponse? ofc) => Category = ofc!;

        public void SetImageKey(string? imageKey) => ImageKey = imageKey;
    }
}