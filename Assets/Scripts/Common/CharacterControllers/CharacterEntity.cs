using UnityEngine;

namespace CharacterControllers
{
    internal sealed class CharacterEntity : ICharacterEntity
    {
        internal float Speed { get; set; }
        internal float Health { get; set; }

        internal GameObject Object { get; set; }
        internal Transform Transform { get; set; }


        public ICharacterPositionController PositionController { get; private set; }

        public void ContainerizeControllers(ICharacterPositionController positionController)
        {
            PositionController = positionController;
        }
    }
}