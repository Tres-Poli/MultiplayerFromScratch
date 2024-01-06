using System;
using UnityEngine;

namespace Character
{
    public interface ICharacterProvider
    {
        void CreateCharacter(ushort id);
        void CreateAICharacter(ushort id, Vector3[] positions);
        void RemoveCharacter(ushort id);
    }
}