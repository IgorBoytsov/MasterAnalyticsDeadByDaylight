namespace DBDAnalytics.Shared.Contracts.Requests.Shared
{
    public sealed record FileInput(
        Stream Content,
        string FileName,
        string ContentType 
    );
}