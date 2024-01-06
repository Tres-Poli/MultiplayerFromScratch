using System.Collections.Generic;
using Components;
using Leopotam.Ecs;
using Messages;
using Networking;
using Riptide;

namespace Systems
{
    public sealed class CharacterSyncSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private readonly IConnectionSyncManager _connectionSyncManager;
        private readonly IMessageRouter _messageRouter;
        private EcsFilter<IdComponent, BodyComponent> _filter;

        public CharacterSyncSystem(IConnectionSyncManager connectionSyncManager, IMessageRouter messageRouter)
        {
            _connectionSyncManager = connectionSyncManager;
            _messageRouter = messageRouter;
        }
        
        public void Init()
        {
            _connectionSyncManager.OnClientConnected += ClientConnectedCallback;
            _connectionSyncManager.OnClientDisconnected += ClientDisconnected_Callback;
        }

        private void ClientConnectedCallback(ushort id)
        {
            int entitiesCount = _filter.GetEntitiesCount();
            List<CharacterSyncElem> syncCharacters = new List<CharacterSyncElem>(entitiesCount - 1);
            for (int i = 0; i < entitiesCount; i++)
            {
                ref IdComponent idComponent = ref _filter.Get1(i);
                ref BodyComponent bodyComponent = ref _filter.Get2(i);
                syncCharacters.Add(new CharacterSyncElem(idComponent.Id, idComponent.CharacterType, bodyComponent.Body.position));
            }

            Message message = Message.Create(MessageSendMode.Reliable, (ushort)MessageType.CharactersSync);
            CharactersSyncMessage syncMessage = new CharactersSyncMessage();
            syncMessage.Characters = syncCharacters.ToArray();
            syncMessage.Size = syncCharacters.Count;
            message.AddSerializable(syncMessage);
            
            _messageRouter.Send(id, message);
        }

        private void ClientDisconnected_Callback(ushort id)
        {
            for (int i = 0; i < _filter.GetEntitiesCount(); i++)
            {
                ref IdComponent idComponent = ref _filter.Get1(i);
                if (idComponent.Id == id)
                {
                    _filter.GetEntity(i).Destroy();
                    
                    Message message = Message.Create(MessageSendMode.Reliable, (ushort)MessageType.RemoveClient);
                    RemoveClientMessage removeMessage = new RemoveClientMessage();
                    removeMessage.Id = id;
                    message.AddSerializable(removeMessage);
                    _messageRouter.SendToAll(message);
                    break;
                }
            }
        }

        public void Destroy()
        {
            _connectionSyncManager.OnClientConnected -= ClientConnectedCallback;
            _connectionSyncManager.OnClientDisconnected -= ClientDisconnected_Callback;
        }
    }
}