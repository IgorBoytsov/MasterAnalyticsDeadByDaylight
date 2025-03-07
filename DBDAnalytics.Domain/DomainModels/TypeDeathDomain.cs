namespace DBDAnalytics.Domain.DomainModels
{
    public class TypeDeathDomain
    {
        private TypeDeathDomain(int IidTypeDeath, string typeDeathName, string? typeDeathDescription)
        {
            IdTypeDeath = IidTypeDeath;
            TypeDeathName = typeDeathName;
            TypeDeathDescription = typeDeathDescription;
        }

        public int IdTypeDeath { get; private set; }

        public string TypeDeathName { get; private set; } = null!;

        public string? TypeDeathDescription { get; private set; }

        public static (TypeDeathDomain? TypeDeathDomain, string? Message) Create(int idTypeDeath, string typeDeathName, string? typeDeathDescription)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(typeDeathName))
                return (null, "Вы забыли указать название.");

            var typeDeath = new TypeDeathDomain(idTypeDeath, typeDeathName, typeDeathDescription);

            return (typeDeath, message);
        }
    }
}