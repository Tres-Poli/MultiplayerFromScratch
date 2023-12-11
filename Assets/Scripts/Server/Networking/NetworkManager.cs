using CharacterControllers;
using Core;
using Cysharp.Threading.Tasks;
using Enforcers;
using ResourceManagement;
using Riptide;

namespace Networking
{
    public sealed class NetworkManager : INetworkManager, IFixedController
    {
        private readonly ILoggerService _logger;
        private readonly IMessageRouter _messageRouter;
        private readonly ICharacterProvider _characterProvider;
        private const string ServerConfigKey = "ServerConfig";

        private IFinite _subscribeHolder;
        private Server _server;
        
        public Server Server { get; private set; }
        public NetworkManager(ITickController tickController, IResourceManager resourceManager, ILoggerService logger, IMessageRouter messageRouter,
            ICharacterProvider characterProvider)
        {
            _logger = logger;
            _messageRouter = messageRouter;
            _characterProvider = characterProvider;
            Initialize(tickController, resourceManager).Forget();
        }

        private async UniTaskVoid Initialize(ITickController tickController, IResourceManager resourceManager)
        {
            _server = new Server();

            ServerConfig config = await resourceManager.LoadConfig<ServerConfig>(ServerConfigKey);
            if (config == null)
            {
                _logger.LogError($"Server cannot be initialized without proper config");
                return;
            }
            
            _server.Start(config.Port, config.MaxClients, useMessageHandlers: false);
            _server.TimeoutTime = 5000;
            _subscribeHolder = tickController.AddController(this);

            _server.ClientConnected += ClientConnected_Callback;
            _server.ConnectionFailed += ClientFailed_Callback;
            _server.ClientDisconnected += ClientDisconnected_Callback;

            Server = _server;
            _server.MessageReceived += MessageReceived_Callback;
        }
        
        public void FixedUpdate(float deltaTime)
        {
            _server.Update();
        }

        private void MessageReceived_Callback(object sender, MessageReceivedEventArgs e)
        {
            _logger.Log($"Message received from {e.FromConnection.Id} with messageId {(MessageEnum)e.MessageId}");
            _messageRouter.RouteMessage(e.MessageId, e.FromConnection.Id, e.Message);
        }
        
        private void ClientConnected_Callback(object sender, ServerConnectedEventArgs e)
        {
            _logger.Log($"Client connected {e.Client.Id}");
            _characterProvider.AddCharacter(CharacterType.Player, e.Client.Id);
        }

        private void ClientFailed_Callback(object sender, ServerConnectionFailedEventArgs e)
        {
            _logger.Log($"Client failed to connect {e.Client.Id}");
        }
        
        private void ClientDisconnected_Callback(object sender, ServerDisconnectedEventArgs e)
        {
            _logger.Log($"Client disconnected {e.Client.Id}");
        }
    }
}