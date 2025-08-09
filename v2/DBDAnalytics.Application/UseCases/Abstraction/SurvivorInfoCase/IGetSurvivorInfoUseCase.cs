namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorInfoCase
{
    public interface IGetSurvivorInfoUseCase
    {
        (List<int>? IdRecords, string? Message) GetLastNRecordsId(int countRecords);
    }
}