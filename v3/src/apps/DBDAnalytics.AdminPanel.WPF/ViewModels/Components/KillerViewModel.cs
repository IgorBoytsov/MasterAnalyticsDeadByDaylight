using DBDAnalytics.Shared.Contracts.Responses.Killers;
using Shared.WPF.Helpers;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Components
{
    internal sealed class KillerViewModel(KillerSoloResponse model) : ObservableObject<KillerSoloResponse>(model)
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

        private byte[]? _originalKillerImageBytes;

        private ImageSource? _killerImage;
        public ImageSource? KillerImage
        {
            get => _killerImage;
            set
            {
                if (SetProperty(ref _killerImage, value))
                {
                    _originalKillerImageBytes ??= ImageHelper.ToBytes(value);

                    OnPropertyChanged(nameof(HasChanges));
                }
            }
        }

        private string? _killerImageKey = model.KillerImageKey;
        public string? KillerImageKey
        {
            get => _killerImageKey;
            set
            {
                SetProperty(ref _killerImageKey, value);
            }
        }

        private string? _killerImagePath;
        public string? KillerImagePath
        {
            get => _killerImagePath;
            set => SetProperty(ref _killerImagePath, value);
        }

        private byte[]? _originalAbilityImageBytes;

        private ImageSource? _abilityImage;
        public ImageSource? AbilityImage
        {
            get => _abilityImage;
            set
            {
                if (SetProperty(ref _abilityImage, value))
                {
                    _originalAbilityImageBytes ??= ImageHelper.ToBytes(value);

                    OnPropertyChanged(nameof(HasChanges));
                }
            }
        }

        private string? _abilityImageKey = model.AbilityImageKey;
        public string? AbilityImageKey
        {
            get => _abilityImageKey;
            set
            {
                SetProperty(ref _abilityImageKey, value);
            }
        }

        private string? _abilityImagePath;
        public string? AbilityImagePath
        {
            get => _abilityImagePath;
            set => SetProperty(ref _abilityImagePath, value);
        }

        public bool HasChanges
        {
            get
            {
                var currentKillerImageBytes = ImageHelper.ToBytes(KillerImage);
                var currentAbilityImageBytes = ImageHelper.ToBytes(AbilityImage);

                bool killerImageChanged = !(_originalKillerImageBytes ?? []).SequenceEqual(currentKillerImageBytes ?? []);
                bool abilityImageChanged = !(_originalAbilityImageBytes ?? []).SequenceEqual(currentAbilityImageBytes ?? []);

                return killerImageChanged 
                    || abilityImageChanged
                    || _model.OldId != OldId
                    || _model.Name != Name;
            }
        }

        public override void RevertChanges()
        {
            Name = _model.Name;
            OldId = _model.OldId;

            if (_originalKillerImageBytes is not null)
            {
                _killerImage = ImageHelper.ToBitmapImageFromBytes(_originalKillerImageBytes);
                OnPropertyChanged(nameof(KillerImage));
            }
            else
                KillerImage = null;

            if (_originalAbilityImageBytes is not null)
            {
                _abilityImage = ImageHelper.ToBitmapImageFromBytes(_originalAbilityImageBytes);
                OnPropertyChanged(nameof(AbilityImage));
            }
            else
                AbilityImage = null;

            OnPropertyChanged(nameof(HasChanges));
        }

        public override void CommitChanges(KillerSoloResponse newModel)
        {
            base.CommitChanges(newModel);

            _originalKillerImageBytes = ImageHelper.ToBytes(KillerImage);
            _originalAbilityImageBytes = ImageHelper.ToBytes(AbilityImage);

            OnPropertyChanged(nameof(HasChanges));
        }

        public override KillerSoloResponse ToModel()
            => _model with
            {
                OldId = this.OldId,
                Name = this.Name,
                KillerImageKey = this.KillerImageKey,
                AbilityImageKey = this.AbilityImageKey,
            };

        internal void SetKillerImage(ImageSource? imageSource, string? path)
        {
            KillerImage = imageSource;
            KillerImagePath = path;
        }

        internal void SetAbilityImage(ImageSource? imageSource, string? path)
        {
            AbilityImage = imageSource;
            AbilityImagePath = path;
        }

        public void SetLocalID(Guid id) => LocalId = id.ToString();

        internal void SetKillerImageKey(string killerImageKey) => KillerImageKey = killerImageKey;

        internal void SetAbilityImageKey(string abilityImageKey) => AbilityImageKey = abilityImageKey;
    }
}