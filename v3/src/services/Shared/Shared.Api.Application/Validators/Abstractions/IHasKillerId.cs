namespace Shared.Api.Application.Validators.Abstractions
{
    public interface IHasKillerId
    {
        public Guid KillerId { get; }
    }
}