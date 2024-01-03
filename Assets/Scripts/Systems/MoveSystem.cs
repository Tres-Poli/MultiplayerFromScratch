using Leopotam.Ecs;
using Components;
using Core;
using Cysharp.Threading.Tasks;
using Messages;
using Networking;
using Riptide;
using UnityEngine;

namespace Systems
{
    public class MoveSystem : IEcsRunSystem, IEcsDestroySystem, IMessageHandler
    {
        private readonly IMessageRouter _messageRouter;
        private readonly DebugDispatcher _dispatcher;
        private readonly IReconciliation _reconciliation;
        private EcsFilter<IdComponent, MoveComponent, BodyComponent> _filter;

        public MoveSystem(IMessageRouter messageRouter, DebugDispatcher dispatcher, IReconciliation reconciliation)
        {
            _messageRouter = messageRouter;
            _dispatcher = dispatcher;
            _reconciliation = reconciliation;
            _messageRouter.Subscribe((ushort)MessageType.DirectionInput, this);
        }

        public void Run()
        {
            for (int i = 0; i < _filter.GetEntitiesCount(); i++)
            {
                ref IdComponent idComponent = ref _filter.Get1(i);
                ref MoveComponent moveComponent = ref _filter.Get2(i);
                ref BodyComponent bodyComponent = ref _filter.Get3(i);
                
                moveComponent.Interpolation += TickController.TimeStep;
                Vector3 newPosition = Vector3.Lerp(moveComponent.PrevPosition, moveComponent.TargetPosition,
                    moveComponent.Interpolation / moveComponent.InterpolationFactor);
                bodyComponent.Body.MovePosition(newPosition);
                
                PositionMessage positionMessage = new PositionMessage();
                positionMessage.Id = idComponent.Id;
                positionMessage.ReconciliationId = _reconciliation.GetReconId(idComponent.Id);
                positionMessage.Position = bodyComponent.Body.position;

                _dispatcher.Enqueue(() =>
                {
                    Message message = Message.Create(MessageSendMode.Unreliable, (ushort)MessageType.Position);
                    message.AddSerializable(positionMessage);
                    _messageRouter.SendToAll(message);
                });
            }
        }

        public void HandleMessage(ushort id, Message message)
        {
            MoveInputMessage moveInputMessage = new MoveInputMessage();
            moveInputMessage.Deserialize(message);

            for (int i = 0; i < _filter.GetEntitiesCount(); i++)
            {
                ref IdComponent idComponent = ref _filter.Get1(i);
                if (idComponent.Id == id)
                {
                    _dispatcher.Enqueue(() =>
                    {
                        ref BodyComponent bodyComponent = ref _filter.Get3(i);
                        ref MoveComponent moveComponent = ref _filter.Get2(i);
                        moveComponent.PrevPosition = bodyComponent.Body.position;
                        moveComponent.TargetPosition = moveInputMessage.Point;
                        moveComponent.Interpolation = 0f;
                        moveComponent.CalculateInterpolationFactor();
                    });
                    
                    break;
                }
            }
        }

        public void Destroy()
        {
            _messageRouter.Unsubscribe((ushort)MessageType.DirectionInput);
        }
    }
}