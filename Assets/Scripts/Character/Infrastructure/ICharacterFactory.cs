using UnityEngine;

namespace Character
{
    public interface ICharacterFactory
    {
        CharacterView CreateCharacter(ushort id);
        void CreateAICharacter(ushort id, Vector3[] points);
    }
}