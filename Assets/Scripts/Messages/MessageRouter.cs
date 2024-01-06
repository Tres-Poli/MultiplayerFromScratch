using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using Riptide;
using Systems;
using UnityEngine;

namespace Messages
{
    public sealed class MessageRouter : IMessageRouter
    {
        private const int DebugMessageDelayMs = 70;
        
        private readonly ILoggerService _logger;
        private Dictionary<ushort, IMessageHandler> _subscribers;
        
        public MessageRouter(ILoggerService logger)
        {
            _logger = logger;
            _subscribers = new Dictionary<ushort, IMessageHandler>();
        }

        public void SendToAll(Message message)
        {
            NetworkSystem.Server.SendToAll(message);
            message.Release();
        }

        public void Send(ushort clientId, Message message)
        {
            NetworkSystem.Server.Send(message, clientId);
        }

        public void Handle(ushort clientId, ushort messageType, Message message)
        {
            if (_subscribers.TryGetValue(messageType, out IMessageHandler handler))
            {
                handler.HandleMessage(clientId, message);
            }
            
            message.Release();
        }

        public void Subscribe(ushort messageType, IMessageHandler handler)
        {
            if (_subscribers.ContainsKey(messageType))
            {
                _logger.LogError("Only one message handler per message type is allowed");   
                return;
            }
            
            _subscribers.Add(messageType, handler);
        }

        public void Unsubscribe(ushort messageType)
        {
            _subscribers.Remove(messageType);
        }
    }
}