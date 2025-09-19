using DBDAnalytics.CatalogService.Domain.ValueObjects.Patch;
using DBDAnalytics.Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Patch : AggregateRoot<PatchId>
    {
        public int OldId { get; private set; }
        public PatchName Name { get; private set; } = null!;
        public PatchDate Date { get; private set; }

        private Patch() { }

        private Patch(int oldId, PatchName name, PatchDate date) 
        { 
            OldId = oldId;
            Name = name;
            Date = date;
        }

        public static Patch Create(int oldId, string name, DateTime date)
        {
            var nameVo = PatchName.Create(name);
            var dateVo = PatchDate.Create(date);

            return new Patch(oldId, nameVo, dateVo);
        }

        public void UpdateName(PatchName patchName)
        {
            if (Name != patchName)
                Name = patchName;
        }

        public void UpdateDate(PatchDate date)
        {
            if (Date != date)
                Date = date;
        }
    }
}