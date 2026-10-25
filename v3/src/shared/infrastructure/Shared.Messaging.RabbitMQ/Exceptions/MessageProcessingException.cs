using Shared.Kernel.Results;

namespace Shared.Messaging.RabbitMQ.Exceptions
{
    public class MessageProcessingException(Error error) : Exception(error.Message)
    {
    }
}