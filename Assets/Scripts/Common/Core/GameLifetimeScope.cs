using CharacterControllers;
using Common.UI;
using Enforcers;
using Networking;
using ResourceManagement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core
{
    internal sealed class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            GameObject tickControllerObject = new GameObject("TickController");
            TickController tickController = tickControllerObject.AddComponent<TickController>();
            builder.RegisterInstance<ITickController>(tickController);

            builder.Register<ILoggerService, Logger>(Lifetime.Singleton);
            
            new ResourceManagementScope(builder);
            new NetworkingScope(builder);
            new UiScope(builder);
            new EnforcersScope(builder);
            new CharacterScope(builder);
        }
    }
}