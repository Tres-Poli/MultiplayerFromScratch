using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "SimpleAIConfig", menuName = "Configs/SimpleAIConfig")]
    public class SimpleAIConfig : ScriptableObject
    {
        [field: SerializeField] public Vector3[] Positions { get; private set; }
    }
}