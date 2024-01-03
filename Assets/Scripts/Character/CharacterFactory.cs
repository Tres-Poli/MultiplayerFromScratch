using CharacterControllers;
using Leopotam.Ecs;
using ResourceManagement;
using Components;
using Configs;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character
{
    public class CharacterFactory : ICharacterFactory, IInitialize
    {
        private const string PlayerPrefabKey = "Player";
        private const string PlayerConfigKey = "PlayerConfig";
        
        private readonly IResourceManager _resourceManager;
        private readonly ISpawnManager _spawnManager;
        private CharacterConfig _config;

        public CharacterFactory(IResourceManager resourceManager, ISpawnManager spawnManager)
        {
            _resourceManager = resourceManager;
            _spawnManager = spawnManager;
        }

        public async UniTask Initialize()
        {
            _config = await _resourceManager.LoadConfig<CharacterConfig>(PlayerConfigKey);
        }
        
        public async UniTaskVoid CreateCharacter(ushort id)
        {
            CharacterView charView = await _resourceManager.LoadPrefab<CharacterView>(PlayerPrefabKey);
            CharacterView charInstance = Object.Instantiate(charView);
            EcsEntity entity = Bootstrap.World.NewEntity();
            MoveComponent moveComponent = new MoveComponent()
            {
                Speed = _config.Speed
            };

            BodyComponent bodyComponent = new BodyComponent()
            {
                Body = charInstance.Body
            };

            IdComponent idComponent = new IdComponent()
            {
                Id = id
            };
            
            entity.Replace(moveComponent);
            entity.Replace(bodyComponent);
            entity.Replace(idComponent);

            _spawnManager.SpawnCharacter(charInstance, new Vector3(0f, 1f, 0f));
        }
        
        public async UniTaskVoid CreateAICharacter(ushort id, Vector3[] points)
        {
            CharacterView charView = await _resourceManager.LoadPrefab<CharacterView>(PlayerPrefabKey);
            CharacterView charInstance = Object.Instantiate(charView);
            EcsEntity entity = Bootstrap.World.NewEntity();
            MoveComponent moveComponent = new MoveComponent()
            {
                Speed = _config.Speed,
                PrevPosition = points[0],
                TargetPosition = points[0]
            };
            
            moveComponent.CalculateInterpolationFactor();
            moveComponent.Interpolation = 0f;

            BodyComponent bodyComponent = new BodyComponent()
            {
                Body = charInstance.Body
            };

            IdComponent idComponent = new IdComponent()
            {
                Id = id
            };

            AIComponent aiComponent = new AIComponent()
            {
                TargetPointId = 0,
                Points = points
            };
            
            entity.Replace(moveComponent);
            entity.Replace(bodyComponent);
            entity.Replace(idComponent);
            entity.Replace(aiComponent);

            _spawnManager.SpawnCharacter(charInstance, points[0] + new Vector3(0f, 1f, 0f));
        }
    }
}