using Networking;
using Riptide;

namespace Enforcers
{
    public interface IEnforcer
    {
        MessageEnum Type { get; }
        void Enforce(ushort clientId, Message message);
    }
}