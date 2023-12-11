using VContainer;

namespace Networking
{
    public sealed class NetworkingScope
    {
        public NetworkingScope(IContainerBuilder builder)
        {
            builder.Register<INetworkManager, NetworkManager>(Lifetime.Singleton);
        }
    }
}