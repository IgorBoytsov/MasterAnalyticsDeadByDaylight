using DBDAnalytics.Shared.Contracts.Responses.Maps;
using Shared.WPF.ViewModels.Base;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Components
{
    internal class MeasurementViewModel(MeasurementSoloResponse model) : ObservableObject<MeasurementSoloResponse>(model)
    {
        public string Id => _model.Id;

        private string? _localId;
        public string? LocalId
        {
            get => _localId;
            private set
            {
                SetProperty(ref _localId, value);
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

        private bool _hasSave;
        public bool HasSave
        {
            get => _hasSave;
            set => SetProperty(ref _hasSave, value);
        }

        public bool HasChanges => _model.Name != Name;

        public override void RevertChanges()
        {
            Name = _model.Name;

            OnPropertyChanged(nameof(HasChanges));
        }

        public override MeasurementSoloResponse ToModel() 
            => _model with
            {
                Id = this.Id,
                OldId = this.OldId,
                Name = this.Name,
            };

        public void ChangeSaveStatus(bool hasSve) => HasSave = hasSve;

        public void SetLocalID(Guid id) => LocalId = id.ToString();
    }
}