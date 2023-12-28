using Leopotam.Ecs;
using Components;
using Core;
using Messages;
using MessageStructs;
using Networking;
using Riptide;
using UnityEngine;

namespace Transformations
{
    public class MoveSystem : IEcsRunSystem, IEcsDestroySystem, IMessageHandler
    {
        private readonly IMessageRouter _messageRouter;
        private EcsWorld _world;
        private EcsFilter<IdComponent, MoveComponent, BodyComponent> _filter;

        public MoveSystem(IMessageRouter messageRouter)
        {
            _messageRouter = messageRouter;
            _messageRouter.Subscribe((ushort)MessageType.Position, this);
        }

        public void Run()
        {
            for (int i = 0; i < _filter.GetEntitiesCount(); i++)
            {
                ref MoveComponent moveComponent = ref _filter.Get2(i);
                ref BodyComponent charComponent = ref _filter.Get3(i);
                charComponent.Body.MovePosition(charComponent.Body.position + moveComponent.DirectionSpeedTaken * Time.deltaTime);
            }
        }

        public void HandleMessage(ushort id, Message message)
        {
            PositionControlMessage positionMessage = new PositionControlMessage();
            positionMessage.Deserialize(message);

            for (int i = 0; i < _filter.GetEntitiesCount(); i++)
            {
                ref IdComponent component = ref _filter.Get1(i);
                if (component.Id == id)
                {
                    ref MoveComponent moveComponent = ref _filter.Get2(i);
                    moveComponent.DirectionSpeedTaken = positionMessage.Direction * moveComponent.Speed;
                    break;
                }
            }
        }

        public void Destroy()
        {
            _messageRouter.Unsubscribe((ushort)MessageType.Position);
        }
    }
}