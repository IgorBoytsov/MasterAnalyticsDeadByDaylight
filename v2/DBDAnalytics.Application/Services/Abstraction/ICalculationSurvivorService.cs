using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Domain.Enums;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ICalculationSurvivorService
    {
        List<LabeledValue> DetailingAnonymous(List<DetailsMatchDTO> matches);
        List<LabeledValue> DetailingDisconnect(List<DetailsMatchDTO> matches);
        List<LabeledValue> DetailingPlatform(List<DetailsMatchDTO> matches, List<PlatformDTO> platforms);
        List<LabeledValue> DetailingTypeDeath(List<DetailsMatchDTO> matches, List<TypeDeathDTO> typeDeaths);
        List<LabeledValue> DetailingTypeDeath(List<DetailsMatchSurvivorDTO> survivors, List<TypeDeathDTO> typeDeaths);
        int SumSurvivorsByTypeDeath(List<DetailsMatchDTO> matches, TypeDeaths typeDeaths, Associations associations = Associations.None);
        public int CountMatchesPlayedAsSurvivor(List<DetailsMatchDTO> matches, Associations association);
        public int CountSurvivorsInMatches(List<DetailsMatchDTO> matches, int idSurvivor, Associations association);
        public List<DetailsMatchSurvivorDTO> GetSurvivors(List<DetailsMatchDTO> matches, int idSurvivor, Associations association);
    }
}