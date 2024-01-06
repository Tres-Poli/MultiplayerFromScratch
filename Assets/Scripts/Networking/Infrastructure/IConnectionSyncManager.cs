using System;

namespace Networking
{
    public interface IConnectionSyncManager
    {
        event Action<ushort> OnClientConnected;
        event Action<ushort> OnClientDisconnected;
        void ConnectClient(ushort id);
        void DisconnectClient(ushort id);
    }
}