﻿using Core;
using Riptide;

namespace Messages
{
    public interface IMessageRouter
    {
        void Send(ushort clientId, ushort messageType, Message message);
        void Subscribe(ushort messageType, IMessageHandler handler);
        void Unsubscribe(ushort messageType);
    }
}