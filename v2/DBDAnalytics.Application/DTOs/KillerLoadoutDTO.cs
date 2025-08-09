using System.Collections.ObjectModel;

namespace DBDAnalytics.Application.DTOs
{
    public class KillerLoadoutDTO
    {
        public int IdKiller { get; set; }

        public string KillerName { get; set; } = null!;

        public byte[]? KillerImage { get; set; }

        public byte[]? KillerAbilityImage { get; set; }

        public ObservableCollection<KillerAddonDTO> KillerAddons { get; set; } = new ObservableCollection<KillerAddonDTO>();

        public ObservableCollection<KillerPerkDTO> KillerPerks { get; set; } = new ObservableCollection<KillerPerkDTO>();
    }
}