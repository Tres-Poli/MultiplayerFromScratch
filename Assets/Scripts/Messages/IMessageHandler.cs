using Riptide;

namespace Messages
{
    public interface IMessageHandler
    {
        void HandleMessage(ushort id, Message message);
    }
}