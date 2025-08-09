using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;
using System.Collections.ObjectModel;

namespace DBDAnalytics.Application.Mappers
{
    internal static class SurvivorsMapper
    {
        public static SurvivorDTO ToDTO(this SurvivorDomain survivor)
        {
            return new SurvivorDTO
            {
                IdSurvivor = survivor.IdSurvivor,
                SurvivorDescription = survivor.SurvivorDescription,
                SurvivorImage = survivor.SurvivorImage,
                SurvivorName = survivor.SurvivorName,
            };
        }

        public static List<SurvivorDTO> ToDTO(this IEnumerable<SurvivorDomain> survivors)
        {
            var list = new List<SurvivorDTO>();

            foreach (var survivor in survivors)
            {
                list.Add(survivor.ToDTO());
            }

            return list;
        }

        public static SurvivorWithPerksDTO ToSurvivorWithPerksDTO(this SurvivorWithPerksDomain survivorWithPerks)
        {
            var survivorPerksDTO = new ObservableCollection<SurvivorPerkDTO>();

            foreach (var perk in survivorWithPerks.SurvivorPerks.ToDTO())
            {
                survivorPerksDTO.Add(perk);
            }

            return new SurvivorWithPerksDTO
            {
                IdSurvivor = survivorWithPerks.IdSurvivor,
                SurvivorDescription = survivorWithPerks.SurvivorDescription,
                SurvivorImage = survivorWithPerks.SurvivorImage,
                SurvivorName = survivorWithPerks.SurvivorName,
                SurvivorPerks = survivorPerksDTO
            };
        }

        public static List<SurvivorWithPerksDTO?> ToSurvivorsWithPerksDTO(this IEnumerable<SurvivorWithPerksDomain> survivorsWithPerks)
        {

            var survivorsWithPerksDTO = new List<SurvivorWithPerksDTO?>();

            foreach (var item in survivorsWithPerks)
            {
                var survivorPerksDTO = new ObservableCollection<SurvivorPerkDTO>();

                foreach (var perk in item.SurvivorPerks.ToDTO())
                {
                    survivorPerksDTO.Add(perk);
                }

                survivorsWithPerksDTO.Add(new SurvivorWithPerksDTO
                {
                    IdSurvivor = item.IdSurvivor,
                    SurvivorDescription = item.SurvivorDescription,
                    SurvivorImage = item.SurvivorImage,
                    SurvivorName = item.SurvivorName,
                    SurvivorPerks = survivorPerksDTO,
                });    
            }
            return survivorsWithPerksDTO;
        }
    }
}