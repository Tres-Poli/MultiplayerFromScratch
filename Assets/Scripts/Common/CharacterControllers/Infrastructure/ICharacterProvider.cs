using UnityEngine;

namespace CharacterControllers
{
    public interface ICharacterProvider
    {
        ICharacterEntity AddCharacter(CharacterType charType, ushort clientId);
        bool GetCharacter(ushort clientId, out ICharacterEntity character);
    }
}