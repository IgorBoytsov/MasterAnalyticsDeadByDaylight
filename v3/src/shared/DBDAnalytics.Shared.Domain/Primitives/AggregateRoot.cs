namespace DBDAnalytics.Shared.Domain.Primitives
{
    public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        protected AggregateRoot(TId id) : base(id) { }

        protected AggregateRoot() { }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => [.. _domainEvents];

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }
}