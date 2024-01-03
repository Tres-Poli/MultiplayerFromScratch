using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CharacterControllers
{
    public interface ICharacterFactory
    {
        UniTaskVoid CreateCharacter(ushort id);
        UniTaskVoid CreateAICharacter(ushort id, Vector3[] points);
    }
}