using System.Collections.Generic;
using Riptide;

namespace Enforcers
{
    internal sealed class MessageRouter : IMessageRouter
    {
        private Dictionary<ushort, IEnforcer> _enforcersMap;
        
        public MessageRouter(IReadOnlyList<IEnforcer> enforcers)
        {
            _enforcersMap = new Dictionary<ushort, IEnforcer>();
            for (int i = 0; i < enforcers.Count; i++)
            {
                IEnforcer currEnforcer = enforcers[i];
                _enforcersMap.Add((ushort)currEnforcer.Type, currEnforcer);
            }
        }

        public void RouteMessage(ushort messageId, ushort clientId, Message message)
        {
            if (_enforcersMap.TryGetValue(messageId, out IEnforcer enforcer))
            {
                enforcer.Enforce(clientId, message);
            }
            
            message.Release();
        }
    }
}