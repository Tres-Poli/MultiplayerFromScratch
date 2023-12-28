namespace Core
{
    public interface ITickController
    {
        IFinite AddController(IUpdateController updateController);
    }
}