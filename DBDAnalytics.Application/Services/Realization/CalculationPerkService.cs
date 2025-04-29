using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Domain.Enums;

namespace DBDAnalytics.Application.Services.Realization
{
    public class CalculationPerkService : ICalculationPerkService
    {
        public List<DetailsMatchSurvivorDTO> GetSurvivors(List<DetailsMatchDTO> matches, int idPerk, Associations association)
        {
            if (association == Associations.None || matches == null)
                return null;

            int associationId = (int)association;

            return matches.SelectMany(match =>
            {
                List<DetailsMatchSurvivorDTO> survivors = [];

                if (match.FirstSurvivorInfo?.IdAssociation == associationId && (match.FirstSurvivorInfo.IdFirstPerk == idPerk || match.FirstSurvivorInfo.IdSecondPerk == idPerk || match.FirstSurvivorInfo.IdThirdPerk == idPerk || match.FirstSurvivorInfo.IdFourthPerk == idPerk)) survivors.Add(match.FirstSurvivorInfo);
                if (match.SecondSurvivorInfo?.IdAssociation == associationId && (match.SecondSurvivorInfo.IdFirstPerk == idPerk || match.SecondSurvivorInfo.IdSecondPerk == idPerk || match.SecondSurvivorInfo.IdThirdPerk == idPerk || match.SecondSurvivorInfo.IdFourthPerk == idPerk)) survivors.Add(match.SecondSurvivorInfo);
                if (match.ThirdSurvivorInfo?.IdAssociation == associationId && (match.ThirdSurvivorInfo.IdFirstPerk == idPerk || match.ThirdSurvivorInfo.IdSecondPerk == idPerk || match.ThirdSurvivorInfo.IdThirdPerk == idPerk || match.ThirdSurvivorInfo.IdFourthPerk == idPerk)) survivors.Add(match.ThirdSurvivorInfo);
                if (match.FourthSurvivorInfo?.IdAssociation == associationId && (match.FourthSurvivorInfo.IdFirstPerk == idPerk || match.FourthSurvivorInfo.IdSecondPerk == idPerk || match.FourthSurvivorInfo.IdThirdPerk == idPerk || match.FourthSurvivorInfo.IdFourthPerk == idPerk)) survivors.Add(match.FourthSurvivorInfo);
                return survivors;

            }).ToList();
        }

        public List<DetailsMatchKillerDTO> GetKillers(List<DetailsMatchDTO> matches, int idPerk, Associations association)
        {
            if (association == Associations.None || matches == null)
                return null;

            int associationId = (int)association;

            return matches.Select(match =>
            {
                if (match.KillerDTO.FirstPerkID == idPerk || match.KillerDTO.SecondPerkID == idPerk || match.KillerDTO.ThirdPerkID == idPerk || match.KillerDTO.FourthPerkID == idPerk)
                    return match.KillerDTO;
                else
                    return null;

            }).Where(x => x != null).ToList()!;
        }
    }
}