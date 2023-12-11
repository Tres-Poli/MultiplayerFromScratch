using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using ResourceManagement;
using UnityEngine;

namespace CharacterControllers
{
    internal sealed class CharacterProvider : ICharacterProvider
    {
        private readonly IResourceManager _resourceManager;
        private readonly ITickController _tickController;
        private const string CharacterConfigKey = "CharacterConfig";
        
        private readonly Dictionary<ushort, CharacterEntity> _charactersMap;

        private CharacterConfig _characterConfig;
        
        public CharacterProvider(IResourceManager resourceManager, ITickController tickController)
        {
            _resourceManager = resourceManager;
            _tickController = tickController;
            _charactersMap = new Dictionary<ushort, CharacterEntity>();
            
            Initialize().Forget();
        }

        private async UniTaskVoid Initialize()
        {
            _characterConfig = await _resourceManager.LoadConfig<CharacterConfig>(CharacterConfigKey);
        }
        
        public ICharacterEntity AddCharacter(CharacterType charType, ushort clientId)
        {
            // To factory
            GameObject view = Object.Instantiate<GameObject>(_characterConfig.Prefab);
            CharacterEntity entity = new CharacterEntity();
            entity.Object = view;
            entity.Transform = view.transform;
            entity.Speed = _characterConfig.Speed;
            entity.Health = 100f;

            ICharacterPositionController positionController = new CharacterPositionController(entity, _tickController);
            
            entity.ContainerizeControllers(positionController);
                
            
            // To spawnManager
            entity.Transform.position = Vector3.zero;
            
            _charactersMap.Add(clientId, entity);

            return entity;
        }

        public bool GetCharacter(ushort clientId, out ICharacterEntity character)
        {
            if (!_charactersMap.TryGetValue(clientId, out CharacterEntity internalCharacter))
            {
                character = null;
                return false;
            }

            character = internalCharacter;
            return true;
        }
    }
}