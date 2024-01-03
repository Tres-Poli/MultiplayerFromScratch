using CharacterControllers;
using Riptide;
using UnityEngine;

namespace Messages
{
    public struct CharactersSyncMessage : IMessageSerializable
    {
        public struct CharacterSyncElem
        {
            public ushort Id;
            public CharacterType Type;
            public Vector3 Position;
        }

        public CharacterSyncElem[] Characters;
        
        public void Serialize(Message message)
        {
            throw new System.NotImplementedException();
        }

        public void Deserialize(Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}