using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character
{
    public sealed class CharacterProvider : ICharacterProvider, IFinite
    {
        private readonly ICharacterFactory _factory;
        private Dictionary<ushort, CharacterView> _charactersViewMap;

        public CharacterProvider(ICharacterFactory factory)
        {
            _factory = factory;
            _charactersViewMap = new Dictionary<ushort, CharacterView>(16);
        }

        public void CreateCharacter(ushort id)
        {
            if (_charactersViewMap.ContainsKey(id))
            {
                Debug.LogError($"Character with id {id} is already instantiated");    
            }
            
            CharacterView view = _factory.CreateCharacter(id);
            _charactersViewMap[id] = view;
        }

        public void CreateAICharacter(ushort id, Vector3[] positions)
        {
            _factory.CreateAICharacter(id, positions);
        }

        public void RemoveCharacter(ushort id)
        {
            if (_charactersViewMap.TryGetValue(id, out CharacterView view))
            {
                Object.Destroy(view.gameObject);
                _charactersViewMap.Remove(id);
            }
        }

        public void Finite()
        {
        }
    }
}