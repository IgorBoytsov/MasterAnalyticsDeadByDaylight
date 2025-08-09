using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Domain.Enums;

namespace DBDAnalytics.Application.Services.Realization
{
    public class CalculationSurvivorService(ICalculationGeneralService calculationGeneralService,
                                            ICreatingApplicationModelsService creatingApplicationModelsService) : ICalculationSurvivorService
    {
        private readonly ICalculationGeneralService _calculatorGeneralService = calculationGeneralService;
        private readonly ICreatingApplicationModelsService _creatingApplicationModelsService = creatingApplicationModelsService;

        /*--Детализация-----------------------------------------------------------------------------------*/

        public List<LabeledValue> DetailingPlatform(List<DetailsMatchDTO> matches, List<PlatformDTO> platforms)
        {
            return platforms.Select(platform =>
            {
                int countPlatform = matches
                    .SelectMany(match => GetSurvivorValues(match, s => s.IdPlatform))
                    .Count(idPlatform => idPlatform == platform.IdPlatform);

                return new LabeledValue
                {
                    Name = platform.PlatformName,
                    Value = _calculatorGeneralService.Percentage(countPlatform, matches.Count * 4),
                };

            }).ToList();
        }

        public List<LabeledValue> DetailingDisconnect(List<DetailsMatchDTO> matches)
        {
            int totalSurvivorCount = matches.Count * 4;
            int countDisconnect = matches
                .SelectMany(match => GetSurvivorValues(match, s => s.Bot))
                .Count(bot => bot == true);

            int countNoDisconnect = matches
                .SelectMany(match => GetSurvivorValues(match, s => s.Bot))
                .Count(bot => bot == false);

            return
            [
                new() {
                    Name = "Отключились: ",
                    Value = _calculatorGeneralService.Percentage(countDisconnect, totalSurvivorCount)
                },
                new() {
                    Name = "Не отключились: ",
                    Value = _calculatorGeneralService.Percentage(countNoDisconnect, totalSurvivorCount)
                }
            ];
        }

        public List<LabeledValue> DetailingAnonymous(List<DetailsMatchDTO> matches)
        {
            int totalSurvivorCount = matches.Count * 4;
            int countAnonymous = matches
                .SelectMany(match => GetSurvivorValues(match, s => s.Anonymous))
                .Count(anonymous => anonymous == true);

            int countNoAnonymous = matches
                .SelectMany(match => GetSurvivorValues(match, s => s.Anonymous))
                .Count(anonymous => anonymous == false);

            return
            [
                new()
                {
                    Name = "Анонимных: ",
                    Value = _calculatorGeneralService.Percentage(countAnonymous, totalSurvivorCount)
                },
                new()
                {
                    Name = "Не анонимных: ",
                    Value = _calculatorGeneralService.Percentage(countNoAnonymous, totalSurvivorCount)
                }
            ];
        }

        public List<LabeledValue> DetailingTypeDeath(List<DetailsMatchDTO> matches, List<TypeDeathDTO> typeDeaths)
        {
            int totalSurvivorCount = matches.Count * 4;

            return typeDeaths.Select(typeDeath =>
            {
                int countTypeDeaths = matches
                    .SelectMany(match => GetSurvivorValues(match, s => s.IdTypeDeath))
                    .Count(idTypeDeath => idTypeDeath == typeDeath.IdTypeDeath);

                return new LabeledValue
                {
                    Name = typeDeath.TypeDeathName,
                    Value = _calculatorGeneralService.Percentage(countTypeDeaths, totalSurvivorCount),
                };
            }).ToList();
        }

        public List<LabeledValue> DetailingTypeDeath(List<DetailsMatchSurvivorDTO> survivors, List<TypeDeathDTO> typeDeaths)
        {
            int totalSurvivorCount = survivors.Count;

            return typeDeaths.Select(typeDeath =>
            {
                int countTypeDeaths = survivors.Count(x => x.IdTypeDeath == typeDeath.IdTypeDeath);

                return new LabeledValue
                {
                    Name = typeDeath.TypeDeathName,
                    Value = _calculatorGeneralService.Percentage(countTypeDeaths, totalSurvivorCount),
                };
            }).ToList();
        }

        /*--Количество------------------------------------------------------------------------------------*/

        public int SumSurvivorsByTypeDeath(List<DetailsMatchDTO> matches, TypeDeaths typeDeaths, Associations associations = Associations.None)
        {
            int typeDeathId = (int)typeDeaths;
            int associationId = (int)associations;

            return matches.Sum(match =>
            {
                int countInMatch = 0;

                if (SurvivorMatchesConditions(match.FirstSurvivorInfo!, typeDeathId, associationId, associations == Associations.None)) countInMatch++;
                if (SurvivorMatchesConditions(match.SecondSurvivorInfo!, typeDeathId, associationId, associations == Associations.None)) countInMatch++;
                if (SurvivorMatchesConditions(match.ThirdSurvivorInfo!, typeDeathId, associationId, associations == Associations.None)) countInMatch++;
                if (SurvivorMatchesConditions(match.FourthSurvivorInfo!, typeDeathId, associationId, associations == Associations.None)) countInMatch++;

                return countInMatch;
            });
        }

        public int CountSurvivorsInMatches(List<DetailsMatchDTO> matches, int idSurvivor, Associations association)
        {
            if (association == Associations.None || matches == null)
                return 0;

            int associationId = (int)association;

            return matches.Sum(match =>
            {
                int countInMatch = 0;
                if (match.FirstSurvivorInfo?.IdAssociation == associationId && match.FirstSurvivorInfo.IdSurvivor == idSurvivor) countInMatch++;
                if (match.SecondSurvivorInfo?.IdAssociation == associationId && match.SecondSurvivorInfo.IdSurvivor == idSurvivor) countInMatch++;
                if (match.ThirdSurvivorInfo?.IdAssociation == associationId && match.ThirdSurvivorInfo.IdSurvivor == idSurvivor) countInMatch++;
                if (match.FourthSurvivorInfo?.IdAssociation == associationId && match.FourthSurvivorInfo.IdSurvivor == idSurvivor) countInMatch++;
                return countInMatch;
            });
        }

        public int CountMatchesPlayedAsSurvivor(List<DetailsMatchDTO> matches, Associations association)
        {
            if (association == Associations.None)
                return 0;

            int associationId = (int)association;

            return matches.Count(match =>
                match.FirstSurvivorInfo?.IdAssociation == associationId ||
                match.SecondSurvivorInfo?.IdAssociation == associationId ||
                match.ThirdSurvivorInfo?.IdAssociation == associationId ||
                match.FourthSurvivorInfo?.IdAssociation == associationId
            );
        }

        public List<DetailsMatchSurvivorDTO> GetSurvivors(List<DetailsMatchDTO> matches, int idSurvivor, Associations association)
        {
            if (association == Associations.None || matches == null)
                return null;

            int associationId = (int)association;

            return matches.SelectMany(match =>
            {
                List<DetailsMatchSurvivorDTO> survivors = [];

                if (match.FirstSurvivorInfo?.IdAssociation == associationId && match.FirstSurvivorInfo.IdSurvivor == idSurvivor) survivors.Add(match.FirstSurvivorInfo);
                if (match.SecondSurvivorInfo?.IdAssociation == associationId && match.SecondSurvivorInfo.IdSurvivor == idSurvivor) survivors.Add(match.SecondSurvivorInfo);
                if (match.ThirdSurvivorInfo?.IdAssociation == associationId && match.ThirdSurvivorInfo.IdSurvivor == idSurvivor) survivors.Add(match.ThirdSurvivorInfo);
                if (match.FourthSurvivorInfo?.IdAssociation == associationId && match.FourthSurvivorInfo.IdSurvivor == idSurvivor) survivors.Add(match.FourthSurvivorInfo);
                return survivors;
            }).ToList();
        }

        /*--Вспомогательные методы------------------------------------------------------------------------*/

        private static IEnumerable<T?> GetSurvivorValues<T>(DetailsMatchDTO match, Func<DetailsMatchSurvivorDTO, T?> selector)
        {
            yield return match.FirstSurvivorInfo != null ? selector(match.FirstSurvivorInfo) : default;
            yield return match.SecondSurvivorInfo != null ? selector(match.SecondSurvivorInfo) : default;
            yield return match.ThirdSurvivorInfo != null ? selector(match.ThirdSurvivorInfo) : default;
            yield return match.FourthSurvivorInfo != null ? selector(match.FourthSurvivorInfo) : default;
        }

        private static bool SurvivorMatchesConditions(DetailsMatchSurvivorDTO survivorInfo, int typeDeathId, int associationId, bool ignoreAssociation)
        {
            if (survivorInfo == null) return false;

            bool typeMatch = survivorInfo.IdTypeDeath == typeDeathId;
            bool associationMatch = ignoreAssociation || (survivorInfo.IdAssociation == associationId);

            return typeMatch && associationMatch;
        }
    }
}