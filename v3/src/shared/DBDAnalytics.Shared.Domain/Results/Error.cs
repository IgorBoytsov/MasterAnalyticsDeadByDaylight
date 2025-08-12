namespace DBDAnalytics.Shared.Domain.Results
{
    public sealed record Error(ErrorCode Code, string Message);
}