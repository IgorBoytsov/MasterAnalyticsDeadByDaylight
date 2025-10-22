using DBDAnalytics.Shared.Contracts.Responses;
using Shared.WPF.ViewModels.Base;

namespace DBDAnalytics.AdminPanel.WPF.ViewModels.Components
{
    internal sealed class PatchViewModel(PatchResponse model) : ObservableObject<PatchResponse>(model)
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

        private DateTime _date = DateTime.Parse(model.Date);
        public DateTime Date
        {
            get => _date;
            set
            {
                if (SetProperty(ref _date, value))
                    OnPropertyChanged(nameof(HasChanges));
            }
        }

        public bool HasChanges => _model.OldId != OldId || _model.Name != Name || DateTime.Parse(_model.Date).Date != Date.Date;

        public override void RevertChanges()
        {
            OldId = _model.OldId;
            Name = _model.Name;
        }

        public override PatchResponse ToModel()
            => _model with
            {
                OldId = this.OldId,
                Name = this.Name
            };
    }
}