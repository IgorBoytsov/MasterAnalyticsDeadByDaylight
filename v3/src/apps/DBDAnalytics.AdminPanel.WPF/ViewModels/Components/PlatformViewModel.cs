using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using Shared.WPF.ViewModels.Base;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Components
{
    internal sealed class PlatformViewModel(PlatformResponse model) : ObservableObject<PlatformResponse>(model)
    {
        public int Id => _model.Id;

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

        public bool HasChanges => _model.OldId != OldId || _model.Name != Name;

        public override void RevertChanges()
        {
            OldId = _model.OldId;
            Name = _model.Name;
        }

        public override PlatformResponse ToModel()
            => _model with
            {
                OldId = this.OldId,
                Name = this.Name
            };
    }
}