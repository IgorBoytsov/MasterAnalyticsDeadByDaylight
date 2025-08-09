using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Domain.Enums;

namespace DBDAnalytics.Application.Services.Realization
{
    public class CalculationItemService : ICalculationItemService
    {
        public List<DetailsMatchSurvivorDTO> GetSurvivors(List<DetailsMatchDTO> matches, int idItem, Associations association)
        {
            if (association == Associations.None || matches == null)
                return null;

            int associationId = (int)association;

            return matches.SelectMany(match =>
            {
                List<DetailsMatchSurvivorDTO> survivors = [];

                if (match.FirstSurvivorInfo?.IdAssociation == associationId && match.FirstSurvivorInfo.IdItem == idItem) survivors.Add(match.FirstSurvivorInfo);
                if (match.SecondSurvivorInfo?.IdAssociation == associationId && match.SecondSurvivorInfo.IdItem == idItem) survivors.Add(match.SecondSurvivorInfo);
                if (match.ThirdSurvivorInfo?.IdAssociation == associationId && match.ThirdSurvivorInfo.IdItem == idItem) survivors.Add(match.ThirdSurvivorInfo);
                if (match.FourthSurvivorInfo?.IdAssociation == associationId && match.FourthSurvivorInfo.IdItem == idItem) survivors.Add(match.FourthSurvivorInfo);
                return survivors;
            }).ToList();
        }
    }
}