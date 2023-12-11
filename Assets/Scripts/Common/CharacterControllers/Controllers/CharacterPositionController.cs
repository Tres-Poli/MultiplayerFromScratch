using Core;
using UnityEngine;

namespace CharacterControllers
{
    internal sealed class CharacterPositionController : IFixedController, ICharacterPositionController
    {
        private readonly CharacterEntity _character;
        private readonly IFinite _subscriptionHolder;

        private Vector3 _directionSpeedTaken;
        
        public CharacterPositionController(CharacterEntity character, ITickController tickController)
        {
            _character = character;
            _subscriptionHolder = tickController.AddController(this);
            _directionSpeedTaken = Vector3.zero;
        }
        
        public void FixedUpdate(float deltaTime)
        {
            _character.Transform.position += _directionSpeedTaken * deltaTime;
        }

        public void SetDirection(Vector3 direction)
        {
            _directionSpeedTaken = direction * _character.Speed;
        }
    }
}