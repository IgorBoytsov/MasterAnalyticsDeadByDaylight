using DBDAnalytics.Application.DTOs.DetailsViewDTOs;
using DBDAnalytics.Domain.DomainModels.DetailsMatchView;

namespace DBDAnalytics.Application.Mappers
{
    public static class DetailsMatchesViewMapper
    {
        public static DetailsMatchViewDTO ToDTO(this DetailsMatchViewDomain domain)
        {
            return new DetailsMatchViewDTO()
            {
                MapImage = domain.MapImage,
                MapName = domain.MapName,
                GameEvent = domain.GameEvent,
                GameMode = domain.GameMode,
                DateTimeMatch = domain.DateTimeMatch,
                MatchDuration = domain.MatchDuration,
                MatchImage = domain.MatchImage,

                Killer = domain.Killer.ToDTO(),

                FirstSurvivor = domain.FirstSurvivor.ToDTO(),
                SecondSurvivor = domain.SecondSurvivor.ToDTO(),
                ThirdSurvivor = domain.ThirdSurvivor.ToDTO(),
                FourthSurvivor = domain.FourthSurvivor.ToDTO(),

                
            };
        }

        public static DetailsMatchKillerViewDTO ToDTO(this DetailsMatchKillerViewDomain domain)
        {
            return new DetailsMatchKillerViewDTO()
            {
                Image = domain.Image,
                Ability = domain.Ability,
                Name = domain.Name,
                Prestige = domain.Prestige,
                Score = domain.Score,
                IsAnonymous = domain.IsAnonymous,
                IsBot = domain.IsBot,
                PlayerAssociation = domain.PlayerAssociation,
                Platform = domain.Platform,

                FirstAddonImage = domain.FirstAddonImage,
                FirstAddonName = domain.FirstAddonName,

                SecondAddonImage = domain.SecondAddonImage,
                SecondAddonName = domain.SecondAddonName,

                FirstPerkImage = domain.FirstPerkImage,
                FirstPerkName = domain.FirstPerkName,

                SecondPerkImage = domain.SecondPerkImage,
                SecondPerkName = domain.SecondPerkName,

                ThirdPerkImage = domain.ThirdPerkImage,
                ThirdPerkName = domain.ThirdPerkName,

                FourthPerkImage = domain.FourthPerkImage,
                FourthPerkName = domain.FourthPerkName,

                OfferingImage = domain.OfferingImage,
                OfferingName = domain.OfferingName,
            };
        }

        public static DetailsMatchSurvivorViewDTO ToDTO(this DetailsMatchSurvivorViewDomain domain)
        {
            return new DetailsMatchSurvivorViewDTO()
            {
                Image = domain.Image,
                Name = domain.Name,
                Prestige = domain.Prestige,
                Score = domain.Score,
                IsAnonymous = domain.IsAnonymous,
                IsBot = domain.IsBot,
                PlayerAssociation = domain.PlayerAssociation,
                Platform = domain.Platform,
                TypeDeath = domain.TypeDeath,

                ItemImage = domain.ItemImage,
                ItemName = domain.ItemName,

                FirstItemAddonImage = domain.FirstItemAddonImage,
                FirstItemAddonName = domain.FirstItemAddonName,

                SecondItemAddonImage = domain.SecondItemAddonImage,
                SecondItemAddonName = domain.SecondItemAddonName,

                FirstPerkImage = domain.FirstPerkImage,
                FirstPerkName = domain.FirstPerkName,

                SecondPerkImage = domain.SecondPerkImage,
                SecondPerkName = domain.SecondPerkName,

                ThirdPerkImage = domain.ThirdPerkImage,
                ThirdPerkName = domain.ThirdPerkName,

                FourthPerkImage = domain.FourthPerkImage,
                FourthPerkName = domain.FourthPerkName,

                OfferingImage = domain.OfferingImage,
                OfferingName = domain.OfferingName,
            };
        }
    }
}