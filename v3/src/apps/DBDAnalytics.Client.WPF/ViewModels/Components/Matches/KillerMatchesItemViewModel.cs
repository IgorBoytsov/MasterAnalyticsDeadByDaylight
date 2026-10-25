using DBDAnalytics.Shared.Contracts.Responses.MatchService;
using Shared.WPF.ViewModels.Base;
using System.Windows.Media;

namespace DBDAnalytics.Client.WPF.ViewModels.Components.Matches
{
    internal sealed class KillerMatchesItemViewModel(MatchKillerItemResponse model) : ObservableObject<MatchKillerItemResponse>(model)
    {
        public string MatchId => _model.MatchId;
        public string KillerId => _model.KillerId;
        public string MapId => _model.MapId;
        public string StartedAt => _model.StartedAt;
        public string Duration => _model.Duration;
        public int CountKills => _model.CountKills;
        public int CountHooks => _model.CountHooks;
        public int CountRecentGenerators => _model.CountRecentGenerators;

        public string? KillerName
        {
            get;
            private set => SetProperty(ref field, value);
        }

        public ImageSource? KillerImage
        {
            get;
            private set => SetProperty(ref field, value);
        }

        public string? MapName
        {
            get;
            private set => SetProperty(ref field, value);
        }

        public override void RevertChanges()
        {
            throw new NotImplementedException();
        }

        public override MatchKillerItemResponse ToModel()
        {
            throw new NotImplementedException();
        }

        public void SetKillerImage(ImageSource? image) => KillerImage = image;
        public void SetKillerName(string name) => KillerName = name;
        public void SetMapName(string name) => MapName = name; 
    }
}