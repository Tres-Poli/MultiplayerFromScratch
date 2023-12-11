using Riptide;

namespace Enforcers
{
    public interface IMessageRouter
    {
        void RouteMessage(ushort messageId, ushort clientId, Message message);
    }
}