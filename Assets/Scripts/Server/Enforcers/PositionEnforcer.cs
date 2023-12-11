using CharacterControllers;
using Core;
using Enforcers;
using Networking;
using Riptide;
using UnityEngine;

namespace Enforcers
{
    internal sealed class PositionEnforcer : IEnforcer
    {
        private readonly ILoggerService _logger;
        private readonly ICharacterProvider _characterProvider;

        public MessageEnum Type => MessageEnum.PositionControl;
        public PositionEnforcer(ILoggerService logger, ICharacterProvider characterProvider)
        {
            _logger = logger;
            _characterProvider = characterProvider;
        }

        public void Enforce(ushort clientId, Message message)
        {
            if (!_characterProvider.GetCharacter(clientId, out ICharacterEntity character))
            {
                _logger.Log($"No character for client {clientId}");
                return;
            }
            
            Vector3 moveDirection;
            moveDirection.x = message.GetFloat();
            moveDirection.y = message.GetFloat();
            moveDirection.z = message.GetFloat();
            
            character.PositionController.SetDirection(moveDirection);
        }
    }
}