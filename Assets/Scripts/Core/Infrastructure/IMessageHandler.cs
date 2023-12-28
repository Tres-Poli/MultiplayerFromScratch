using Riptide;

namespace Core
{
    public interface IMessageHandler
    {
        void HandleMessage(ushort id, Message message);
    }
}