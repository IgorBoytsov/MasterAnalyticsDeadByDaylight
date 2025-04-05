using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Domain.Constants;

namespace DBDAnalytics.Application.Services.Realization
{
    public class CalculationKillerService(ICreatingApplicationModelsService creatingApplicationModelsService) : ICalculationKillerService
    {
        private readonly ICreatingApplicationModelsService _creatingApplicationModelsService = creatingApplicationModelsService;

        public List<LabeledValue> DetailingKills(List<DetailsMatchDTO> matches, string displayNameFormat) => 
            _creatingApplicationModelsService.GenerateLabeledValues(matches, GameRulesConstants.MaxKills, i => matches.Count(x => x.CountKill == i), displayNameFormat);

        public List<LabeledValue> DetailingHooks(List<DetailsMatchDTO> matches, string displayNameFormat) => 
            _creatingApplicationModelsService.GenerateLabeledValues(matches, GameRulesConstants.MaxHooks, i => matches.Count(x => x.CountHook == i), displayNameFormat);
    }
}