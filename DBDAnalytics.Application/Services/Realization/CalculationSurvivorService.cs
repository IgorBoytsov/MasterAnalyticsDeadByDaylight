using DBDAnalytics.Application.ApplicationModels.CalculationModels;
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Services.Abstraction;

namespace DBDAnalytics.Application.Services.Realization
{
    public class CalculationSurvivorService(ICalculationGeneralService calculationGeneralService) : ICalculationSurvivorService
    {
        private readonly ICalculationGeneralService _calculatorGeneralService = calculationGeneralService;

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

        private static IEnumerable<T?> GetSurvivorValues<T>(DetailsMatchDTO match, Func<DetailsMatchSurvivorDTO, T?> selector)
        {
            yield return match.FirstSurvivorInfo != null ? selector(match.FirstSurvivorInfo) : default;
            yield return match.SecondSurvivorInfo != null ? selector(match.SecondSurvivorInfo) : default;
            yield return match.ThirdSurvivorInfo != null ? selector(match.ThirdSurvivorInfo) : default;
            yield return match.FourthSurvivorInfo != null ? selector(match.FourthSurvivorInfo) : default;
        }
    }
}
