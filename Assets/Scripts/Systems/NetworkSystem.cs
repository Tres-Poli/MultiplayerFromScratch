using Character;
using Configs;
using Core;
using Leopotam.Ecs;
using Messages;
using Networking;
using ResourceManagement;
using Riptide;

namespace Systems
{
    public sealed class NetworkSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private const string ServerConfigKey = "ServerConfig";
        public static Server Server { get; private set; }
        
        private readonly ILoggerService _logger;
        private readonly IResourceManager _resourceManager;
        private readonly IMessageRouter _messageRouter;
        private readonly IConnectionSyncManager _connectionSyncManager;

        private IFinite _subscribeHolder;
        private Server _server;
        
        public NetworkSystem(ILoggerService logger, IResourceManager resourceManager, IMessageRouter messageRouter, 
            IConnectionSyncManager connectionSyncManager)
        {
            _logger = logger;
            _resourceManager = resourceManager;
            _messageRouter = messageRouter;
            _connectionSyncManager = connectionSyncManager;
        }

        public async void Init()
        {
            _server = new Server();
            Server = _server;

            ServerConfig config = await _resourceManager.LoadConfig<ServerConfig>(ServerConfigKey);
            if (config == null)
            {
                _logger.LogError($"Server cannot be initialized without proper config");
                return;
            }
            
            _server.Start(config.Port, config.MaxClients, useMessageHandlers: false);
            _server.TimeoutTime = 5000;

            _server.ClientConnected += ClientConnected_Callback;
            _server.ConnectionFailed += ClientFailed_Callback;
            _server.ClientDisconnected += ClientDisconnected_Callback;
            
            _server.MessageReceived += MessageReceived_Callback;
        }

        public void Run()
        {
            _server.Update();
        }

        private void MessageReceived_Callback(object sender, MessageReceivedEventArgs e)
        {
            //_logger.Log($"Message received from {e.FromConnection.Id} with messageId {(MessageType)e.MessageId}");
            _messageRouter.Handle(e.FromConnection.Id, e.MessageId, e.Message);
        }
        
        private void ClientConnected_Callback(object sender, ServerConnectedEventArgs e)
        {
            _logger.Log($"Client connected {e.Client.Id}");
            //_characterProvider.CreateCharacter(e.Client.Id);
            _connectionSyncManager.ConnectClient(e.Client.Id);
        }

        private void ClientFailed_Callback(object sender, ServerConnectionFailedEventArgs e)
        {
            _logger.Log($"Client failed to connect {e.Client.Id}");
        }
        
        private void ClientDisconnected_Callback(object sender, ServerDisconnectedEventArgs e)
        {
            _logger.Log($"Client disconnected {e.Client.Id}");
            _connectionSyncManager.DisconnectClient(e.Client.Id);
        }

        public void Destroy()
        {
            _server.ClientConnected -= ClientConnected_Callback;
            _server.ConnectionFailed -= ClientFailed_Callback;
            _server.ClientDisconnected -= ClientDisconnected_Callback;
            
            _server.MessageReceived -= MessageReceived_Callback;
        }
    }
}