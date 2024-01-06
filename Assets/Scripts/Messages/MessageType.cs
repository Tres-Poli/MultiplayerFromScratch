namespace Networking
{
    public enum MessageType
    {
        Test = 0,
        
        DirectionInput = 1,
        Position = 2,
        ReconciliationSync = 3,
        CharactersSync = 4,
        RemoveClient = 5,
    }
}