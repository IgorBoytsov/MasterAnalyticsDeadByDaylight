using DBDAnalytics.Shared.Contracts.Responses.MatchService;
using Shared.WPF.ViewModels.Base;

namespace DBDAnalytics.Client.WPF.ViewModels.Components.Matches
{
    internal sealed class SurvivorMatchesItemViewModel(MatchSurvivorItemResponse model) : ObservableObject<MatchSurvivorItemResponse>(model)
    {
        public override void RevertChanges()
        {
            throw new NotImplementedException();
        }

        public override MatchSurvivorItemResponse ToModel()
        {
            throw new NotImplementedException();
        }
    }
}