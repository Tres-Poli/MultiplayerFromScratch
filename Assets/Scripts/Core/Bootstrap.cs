﻿using System.Collections.Generic;
using Character;
using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using Messages;
using Networking;
using ResourceManagement;
using Riptide.Utils;
using Systems;
using UI.Infrastructure;
using UnityEngine;
using VContainer;

namespace Core
{
    public class Bootstrap : MonoBehaviour, ITimestampController
    {
        [SerializeField] private RectTransform _mainCanvas;
        
        public static EcsWorld World { get; private set; }
        private EcsSystems _systems;

        private IFinite _updateSubscription;
        
        void Start() 
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        }
        
        void OnDestroy() 
        {
            _systems.Destroy();
            World.Destroy();
            
            _updateSubscription.Finite();
        }

        [Inject]
        public async UniTaskVoid Initialize(IReadOnlyList<IInitialize> initializeInstances, IUiManager uiManager, ILoggerService logger, 
            IResourceManager resourceManager, ICharacterProvider characterProvider, ITickController tickController, IMessageRouter messageRouter,
            DebugDispatcher dispatcher, IReconciliation reconciliation, IConnectionSyncManager connectionSyncManager)
        {
            World = new EcsWorld ();
            _systems = new EcsSystems(World)
                .Add(new MoveSystem(messageRouter, dispatcher, reconciliation))
                .Add(new NetworkSystem(logger, resourceManager, messageRouter, connectionSyncManager))
                .Add(new AISystem(resourceManager, characterProvider))
                .Add(new CharacterSyncSystem(connectionSyncManager, messageRouter));

            await uiManager.Initialize(_mainCanvas);
            uiManager.AddScreen<ILoggerController>(ScreenType.Logger);

            for (int i = 0; i < initializeInstances.Count; i++)
            {
                await initializeInstances[i].Initialize();
            }

            _systems.Init();
            _updateSubscription = tickController.AddController(this);
        }

        public void UpdateController()
        {
            _systems.Run();
        }
    }
}