namespace DBDAnalytics.Domain.DomainModels
{
    public class PlayerAssociationDomain
    {
        private PlayerAssociationDomain(int idPlayerAssociation, string? playerAssociationName, string? playerAssociationDescription)
        {
            IdPlayerAssociation = idPlayerAssociation;
            PlayerAssociationName = playerAssociationName;
            PlayerAssociationDescription = playerAssociationDescription;
        }

        public int IdPlayerAssociation { get; private set; }

        public string? PlayerAssociationName { get; private set; }

        public string? PlayerAssociationDescription { get; private set; }

        public static (PlayerAssociationDomain? CreatedPlayerAssociationDomain, string? Message) Create(int idPlayerAssociation, string? playerAssociationName, string? playerAssociationDescription)
        {
            string message = string.Empty;
            string newPlayerAssociationDescription = "Описание отсутствует.";

            if (string.IsNullOrWhiteSpace(playerAssociationName))
            {
                return (null, "Вы не указали названия игровой ассоциации.");
            }

            if (playerAssociationName?.Length > 50)
            {
                return (null, "Длинна название игровой ассоциации не может быть больше 20 символов.");
            }

            if (playerAssociationDescription?.Length > 1000)
            {
                return (null, "Слишком длинное описание! Максимально допустимая длинна не более 1000 символов.");
            }

            if (!string.IsNullOrWhiteSpace(playerAssociationDescription))
            {
                newPlayerAssociationDescription = playerAssociationDescription;
            }

            var newDomain = new PlayerAssociationDomain(idPlayerAssociation, playerAssociationName, newPlayerAssociationDescription);

            return (newDomain, message);
        }
    }
}