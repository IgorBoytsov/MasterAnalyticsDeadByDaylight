namespace DBDAnalytics.Application.UseCases.Abstraction.AssociationCase
{
    public interface IDeleteAssociationUseCase
    {
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idPlayerAssociation);
    }
}