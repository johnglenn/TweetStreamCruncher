namespace GlennDemo.Utilities
{
    // TODO: rename to StreamTaskMachine?
    public class StreamingService : IStreamTaskMachine
    {
        private IRuntimeTaskTracker _taskTracker;

        public StreamingService(IRuntimeTaskTracker taskTracker)
        {
            _taskTracker = taskTracker;
        }

        public void LogTaskStatisticsToConsole() => _taskTracker.LogTaskStatisticsToConsole();

        public async Task MakeTasksFromLinesInStream(Stream stream, Func<string?, IEnumerable<Task>> valueHandler,
            Action<Exception> errorHandler, CancellationToken cancellationToken
            )
        {
            using (var reader = new StreamReader(stream))
            {
                // iterate over each tweet
                while (reader.EndOfStream == false && cancellationToken.IsCancellationRequested == false)
                {
                    var tasksSpawnedForThisTweet = valueHandler(await reader.ReadLineAsync());
                    foreach (var task in tasksSpawnedForThisTweet)
                    {
                        // setup error handling for this task
                        _ = task.ContinueWith(
                            (faultedTask, _) =>
                            {
                                if (faultedTask.Exception != null)
                                    errorHandler(faultedTask.Exception);
                            },
                            cancellationToken,
                            TaskContinuationOptions.OnlyOnFaulted);

                        _taskTracker.AddTaskToTrack(task);
                    }
                }
            }
        }
    }
}
