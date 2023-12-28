namespace UI.Infrastructure
{
    public interface ILoggerController : IUiController
    {
        void LogEntry(string message);
    }
}