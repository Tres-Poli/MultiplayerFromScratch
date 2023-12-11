using VContainer;

namespace Enforcers
{
    public class EnforcersScope
    {
        public EnforcersScope(IContainerBuilder builder)
        {
            builder.Register<IMessageRouter, MessageRouter>(Lifetime.Singleton);

            builder.Register<IEnforcer, PositionEnforcer>(Lifetime.Scoped);
        }
    }
}