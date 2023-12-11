using UnityEngine;

namespace CharacterControllers
{
    public interface ICharacterPositionController
    {
        void SetDirection(Vector3 direction);
    }
}