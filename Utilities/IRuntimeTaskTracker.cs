namespace GlennDemo.Utilities
{
    public interface IRuntimeTaskTracker
    {
        void AddTaskToTrack(Task task);

        // TODO: future work - refactor to return generic statistics model; output modality is not a concern of this interface
        void LogTaskStatisticsToConsole();
    }
}
