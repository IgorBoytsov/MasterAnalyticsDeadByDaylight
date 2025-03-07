using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;
using System.Collections.ObjectModel;

namespace DBDAnalytics.Application.Mappers
{
    internal static class KillersMapper
    {
        public static KillerDTO ToDTO(this KillerDomain killer)
        {
            return new KillerDTO
            {
                IdKiller = killer.IdKiller,
                KillerName = killer.KillerName,
                KillerAbilityImage = killer.KillerAbilityImage,
                KillerImage = killer.KillerImage
            };
        }

        public static List<KillerDTO> ToDTO(this IEnumerable<KillerDomain> killers)
        {
            var list = new List<KillerDTO>();

            foreach (var killer in killers)
            {
                list.Add(killer.ToDTO());
            }

            return list;
        }

        public static KillerLoadoutDTO? ToDTO(this KillerLoadoutDomain? killerLoadout)
        {
            var killerAddonsDTO = new ObservableCollection<KillerAddonDTO>();
            var killerPerksDTO = new ObservableCollection<KillerPerkDTO>();

            foreach (var item in killerLoadout.KillerAddons.ToDTO())
            {
                killerAddonsDTO.Add(item);
            }

            foreach (var item in killerLoadout.KillerPerks.ToDTO())
            {
                killerPerksDTO.Add(item);
            }

            return new KillerLoadoutDTO
            {
                IdKiller = killerLoadout.IdKiller,
                KillerName = killerLoadout.KillerName,
                KillerImage = killerLoadout.KillerImage,
                KillerAbilityImage = killerLoadout.KillerAbilityImage,
                KillerAddons = killerAddonsDTO,
                KillerPerks = killerPerksDTO,
            };
        }

        public static List<KillerLoadoutDTO> ToDTO(this IEnumerable<KillerLoadoutDomain> killersLoadout)
        {
            var list = new List<KillerLoadoutDTO>();

            foreach (var killer in killersLoadout)
            {
                list.Add(killer.ToDTO());
            }

            return list;
        }
    }
}