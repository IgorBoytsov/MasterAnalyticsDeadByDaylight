using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Domain.Enums;

namespace DBDAnalytics.Application.Services.Realization
{
    public class CalculationOfferingService : ICalculationOfferingService
    {
        public List<DetailsMatchSurvivorDTO> GetSurvivors(List<DetailsMatchDTO> matches, int idOffering, Associations association)
        {
            if (association == Associations.None || matches == null)
                return null;

            int associationId = (int)association;

            return matches.SelectMany(match =>
            {
                List<DetailsMatchSurvivorDTO> survivors = [];

                if (match.FirstSurvivorInfo?.IdAssociation == associationId && match.FirstSurvivorInfo.IdOffering == idOffering) survivors.Add(match.FirstSurvivorInfo);
                if (match.SecondSurvivorInfo?.IdAssociation == associationId && match.SecondSurvivorInfo.IdOffering == idOffering) survivors.Add(match.SecondSurvivorInfo);
                if (match.ThirdSurvivorInfo?.IdAssociation == associationId && match.ThirdSurvivorInfo.IdOffering == idOffering) survivors.Add(match.ThirdSurvivorInfo);
                if (match.FourthSurvivorInfo?.IdAssociation == associationId && match.FourthSurvivorInfo.IdOffering == idOffering) survivors.Add(match.FourthSurvivorInfo);
                return survivors;

            }).ToList();
        }

        public List<DetailsMatchKillerDTO> GetKillers(List<DetailsMatchDTO> matches, int idOffering, Associations association)
        {
            if (association == Associations.None || matches == null)
                return null;

            int associationId = (int)association;

            return matches.Select(match =>
            {
                if (match.KillerDTO.OfferingID == idOffering)
                    return match.KillerDTO;
                else
                    return null;

            }).Where(x => x != null).ToList()!;
        }
    }
}
