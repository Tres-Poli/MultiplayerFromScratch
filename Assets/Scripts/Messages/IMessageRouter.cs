using Core;
using Riptide;

namespace Messages
{
    public interface IMessageRouter
    {
        void SendToAll(Message message);
        void Send(ushort clientId, Message message);
        void Handle(ushort clientId, ushort messageType, Message message);
        void Subscribe(ushort messageType, IMessageHandler handler);
        void Unsubscribe(ushort messageType);
    }
}