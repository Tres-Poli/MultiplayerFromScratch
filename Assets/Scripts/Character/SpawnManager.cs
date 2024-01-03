using CharacterControllers;
using UnityEngine;

namespace Character
{
    public class SpawnManager : ISpawnManager
    {
        public void SpawnCharacter(CharacterView view, Vector3 position)
        {
            view.transform.position = position;
        }
    }
}