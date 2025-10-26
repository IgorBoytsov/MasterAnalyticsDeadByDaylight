using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using Shared.WPF.Helpers;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Components
{
    internal sealed class SurvivorPerkViewModel(SurvivorPerkResponse model, IReadOnlyCollection<SurvivorPerkCategoryResponse> _allCategories) : ObservableObject<SurvivorPerkResponse>(model)
    {
        private readonly IReadOnlyCollection<SurvivorPerkCategoryResponse> _allCategories = _allCategories;

        public string Id => _model.Id;
        public string SurvivorId => _model.SurvivorId;

        private string? _localSurvivorId;
        public string? LocalSurvivorId
        {
            get => _localSurvivorId;
            private set => SetProperty(ref _localSurvivorId, value);
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

        private SurvivorPerkCategoryResponse? _category;
        public SurvivorPerkCategoryResponse? Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
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

            Category = _allCategories.FirstOrDefault(c => c.Id == _model.CategoryId);

            if (_originalImageBytes is not null)
            {
                Image = ImageHelper.ToBitmapImageFromBytes(_originalImageBytes);
                OnPropertyChanged(nameof(Image));
            }
            else
                Image = null;
        }

        public override void CommitChanges(SurvivorPerkResponse newModel)
        {
            base.CommitChanges(newModel);

            _originalImageBytes = ImageHelper.ToBytes(Image);

            OnPropertyChanged(nameof(HasChanges));
        }

        public override SurvivorPerkResponse ToModel()
            => _model with
            {
                OldId = this.OldId,
                Name = this.Name,
                CategoryId = this.Category?.Id,
            };

        public void SetImage(ImageSource? imageSource, string? path)
        {
            Image = imageSource;
            ImagePath = path;
        }

        public void SetCategory(int? categoryId) => Category = _allCategories.FirstOrDefault(c => c.Id == categoryId);

        internal void SetSurvivorLocalId(string? localId) => LocalSurvivorId = localId;

        internal void SetImageKey(string value) => ImageKey = value;
    }
}