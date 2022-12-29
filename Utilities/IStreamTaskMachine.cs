namespace GlennDemo.Utilities
{
    public interface IStreamTaskMachine
    {
        Task MakeTasksFromLinesInStream(Stream stream, Func<string?, IEnumerable<Task>> valueHandler, Action<Exception> errorHandler, CancellationToken cancellationToken);

        // TODO: future design decision: does gathering these statistics flow through the IStreamTaskMachine or
        // .. do we directly access the tracker for this (assuming a real DI container)
        // as on the IRuntimeTaskTracker, the output target should be genericized here in the future, as well
        void LogTaskStatisticsToConsole();
    }
}
