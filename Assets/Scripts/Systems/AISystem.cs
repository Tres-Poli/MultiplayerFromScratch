using CharacterControllers;
using Components;
using Configs;
using Leopotam.Ecs;
using ResourceManagement;
using UnityEngine;

namespace Systems
{
    public sealed class AISystem : IEcsInitSystem, IEcsRunSystem
    {
        private const string AIConfigKey = "SimpleAIConfig";
        
        private readonly IResourceManager _resourceManager;
        private readonly ICharacterFactory _characterFactory;

        private EcsFilter<AIComponent, MoveComponent, BodyComponent> _filter;
        private SimpleAIConfig _config;
        
        public AISystem(IResourceManager resourceManager, ICharacterFactory characterFactory)
        {
            _resourceManager = resourceManager;
            _characterFactory = characterFactory;
        }

        public async void Init()
        {
            _config = await _resourceManager.LoadConfig<SimpleAIConfig>(AIConfigKey);
            _characterFactory.CreateAICharacter(16000, _config.Positions);
        }

        public void Run()
        {
            for (int i = 0; i < _filter.GetEntitiesCount(); i++)
            {
                ref AIComponent aiComponent = ref _filter.Get1(i);
                ref MoveComponent moveComponent = ref _filter.Get2(i);
                ref BodyComponent bodyComponent = ref _filter.Get3(i);

                Vector3 position = bodyComponent.Body.position;

                if ((aiComponent.Points[aiComponent.TargetPointId % aiComponent.Points.Length] - position).sqrMagnitude <= 0.05f)
                {
                    moveComponent.PrevPosition = position;
                    moveComponent.TargetPosition = aiComponent.Points[++aiComponent.TargetPointId % aiComponent.Points.Length];
                    moveComponent.Interpolation = 0f;
                    moveComponent.CalculateInterpolationFactor();
                }
            }
        }
    }
}