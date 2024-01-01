using System.Collections.Generic;
using Core;
using Messages;
using Riptide;

namespace Networking
{
    public class Reconciliation : IFinite, IMessageHandler, IReconciliation
    {
        private readonly IMessageRouter _messageRouter;

        private readonly Dictionary<ushort, uint> _reconIdMap;

        public Reconciliation(IMessageRouter messageRouter)
        {
            _reconIdMap = new Dictionary<ushort, uint>();
            _messageRouter = messageRouter;
            _messageRouter.Subscribe((ushort)MessageType.ReconciliationSync, this);
        }

        public void HandleMessage(ushort id, Message message)
        {
            ReconciliationSyncMessage reconMessage = new ReconciliationSyncMessage();
            reconMessage.Deserialize(message);
            _reconIdMap[id] = reconMessage.ReconciliationId;
        }

        public void Finite()
        {
            _messageRouter.Unsubscribe((ushort)MessageType.ReconciliationSync);
        }

        public uint GetReconId(ushort clientId)
        {
            if (_reconIdMap.ContainsKey(clientId))
            {
                return _reconIdMap[clientId];
            }

            return 0;
        }
    }
}