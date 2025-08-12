using DBDAnalytics.Shared.Domain.Results;

namespace DBDAnalytics.Shared.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public Error Error { get; } = null!;

        public DomainException(Error error) : base(error.Message)
        {
            
        }

        public DomainException(Error error, Exception? innerException) : base(error.Message, innerException)
        {
            
        }
    }
}