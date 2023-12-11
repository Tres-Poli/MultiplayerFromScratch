namespace CharacterControllers
{
    public interface ICharacterEntity
    {
        ICharacterPositionController PositionController { get; }
    }
}