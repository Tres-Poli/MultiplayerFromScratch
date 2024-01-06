using System;
using Character;

namespace Networking
{
    public sealed class ConnectionSyncManager : IConnectionSyncManager
    {
        private readonly ICharacterProvider _characterProvider;

        public event Action<ushort> OnClientConnected;
        public event Action<ushort> OnClientDisconnected;

        public ConnectionSyncManager(ICharacterProvider characterProvider)
        {
            _characterProvider = characterProvider;
        }

        public void ConnectClient(ushort id)
        {
            _characterProvider.CreateCharacter(id);
            OnClientConnected?.Invoke(id);
        }

        public void DisconnectClient(ushort id)
        {
            _characterProvider.RemoveCharacter(id);
            OnClientDisconnected?.Invoke(id);
        }
    }
}