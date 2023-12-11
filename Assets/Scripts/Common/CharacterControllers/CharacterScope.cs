using VContainer;

namespace CharacterControllers
{
    public class CharacterScope
    {
        public CharacterScope(IContainerBuilder builder)
        {
            builder.Register<ICharacterProvider, CharacterProvider>(Lifetime.Singleton);
        }
    }
}