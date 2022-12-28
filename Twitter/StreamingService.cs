using GlennDemo.Twitter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GlennDemo.Twitter
{
    public class StreamingService
    {
        private TwitterClient _client;
        private StreamHandler _handler;
        private Action<Exception> _errorHandler;

        private ConcurrentQueue<Task> _tasks = new ();

        public StreamingService(TwitterClient client, StreamHandler handler, Action<Exception> errorHandler)
        {
            _client = client;
            _handler = handler;
            _errorHandler = errorHandler;
        }

        public delegate Task StreamHandler(string? contents);

        public async Task StartStreaming(CancellationToken cancellationToken)
        {
            var stream = await _client.GetStreamAsync("/2/tweets/sample/stream", cancellationToken);

            // TODO: handle error conditions

            // TODO: handle disconnects and reconnect automatically

            var reader = new StreamReader(stream);

            while (reader.EndOfStream == false && cancellationToken.IsCancellationRequested == false)
            {
                _tasks.Enqueue(_handler(await reader.ReadLineAsync()));
            }
        }

        #region Debug Logging To Remove Before Production
        // TODO: remove or refactor
        uint _countFaultedTasksAllTime = 0;
        public void DEBUG_CountAndLogTaskMetricsToConsole()
        {
            uint countPendingTasks = 0;
            List<Task> tasksToReAddToQueue = new List<Task>();
            Task parseTask;
            while (_tasks.TryDequeue(out parseTask))
            {
                switch (parseTask.Status)
                {
                    case TaskStatus.Faulted:
                        _countFaultedTasksAllTime++;
                        _errorHandler(parseTask.Exception);
                        break;
                    case TaskStatus.RanToCompletion:
                        break;
                    case TaskStatus.WaitingToRun:
                    case TaskStatus.WaitingForActivation:
                    case TaskStatus.WaitingForChildrenToComplete:
                        countPendingTasks++;
                        tasksToReAddToQueue.Add(parseTask);
                        break;
                    default:
                        tasksToReAddToQueue.Add(parseTask);
                        break;
                }
            }
            foreach (var taskToReAdd in tasksToReAddToQueue)
                _tasks.Enqueue(taskToReAdd);

            Console.WriteLine("{0} pending json parsing tasks (this period)", countPendingTasks);
            Console.WriteLine("{0} faulted tasks (all time)", _countFaultedTasksAllTime);
            Console.WriteLine();
        }
        #endregion
    }
}
