namespace DBDAnalytics.Domain.DomainModels
{
    public class PatchDomain
    {
        private PatchDomain(int idPatch, string patchNumber, DateOnly patchDateRelease)
        {
            IdPatch = idPatch;
            PatchNumber = patchNumber;
            PatchDateRelease = patchDateRelease;
        }
        public int IdPatch { get; private set; }

        public string PatchNumber { get; private set; } = null!;

        public DateOnly PatchDateRelease { get; private set; }

        public static (PatchDomain? PatchDomain, string? Message) Create(int idPatch, string patchNumber, DateOnly patchDateRelease)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(patchNumber))
            {
                return (null, "Вы забыли указать номер патча.");
            }
                
            var patch = new PatchDomain(idPatch, patchNumber, patchDateRelease);

            return (patch, message);
        }
    }
}