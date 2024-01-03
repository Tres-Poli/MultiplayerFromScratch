using Character;
using UnityEngine;

namespace CharacterControllers
{
    public interface ISpawnManager
    {
        void SpawnCharacter(CharacterView view, Vector3 position);
    }
}