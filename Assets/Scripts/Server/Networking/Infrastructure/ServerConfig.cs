using UnityEngine;

namespace Networking
{
    [CreateAssetMenu(fileName = "ServerConfig", menuName = "Configs/ServerConfig")]
    public sealed class ServerConfig : ScriptableObject
    {
        [field: SerializeField] public ushort Port { get; private set; } 
        [field: SerializeField] public ushort MaxClients { get; private set; } 
    }
}