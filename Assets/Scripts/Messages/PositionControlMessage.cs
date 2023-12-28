using Riptide;
using UnityEngine;

namespace MessageStructs
{
    public struct PositionControlMessage : IMessageSerializable
    {
        public Vector3 Direction { get; private set; } 
        public void Serialize(Message message)
        {
            message.AddFloat(Direction.x);
            message.AddFloat(Direction.y);
            message.AddFloat(Direction.z);
        }

        public void Deserialize(Message message)
        {
            Vector3 newDirection;
            newDirection.x = message.GetFloat();
            newDirection.y = message.GetFloat();
            newDirection.z = message.GetFloat();

            Direction = newDirection;
        }
    }
}