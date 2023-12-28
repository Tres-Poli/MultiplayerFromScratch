using UnityEngine;

namespace CharacterControllers
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Configs/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
    }
}