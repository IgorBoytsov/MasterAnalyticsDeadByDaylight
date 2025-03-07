using DBDAnalytics.Application.UseCases.Realization.AssociationCase;
using DBDAnalytics.Domain.DomainModels;
using System.Collections.ObjectModel;

namespace DBDAnalytics.Application.DTOs
{
    public class SurvivorWithPerksDTO
    {
        public int IdSurvivor { get; set; }

        public string SurvivorName { get; set; } = null!;

        public byte[]? SurvivorImage { get; set; }

        public string? SurvivorDescription { get; set; }

        public ObservableCollection<SurvivorPerkDTO> SurvivorPerks { get; set; } = new ObservableCollection<SurvivorPerkDTO>();
    }
}