using System.Collections.Concurrent;

namespace GlennDemo.Utilities
{
    // TODO: this will cause a memory leak if statistics reports aren't generated as the task queue will never empty
    // .. a redesign with an internal timer and a configurable fail-safe bail out on runaway queue growth are needed before production
    public class RuntimeTaskTracker : IRuntimeTaskTracker
    {
        private ConcurrentQueue<Task> _tasksForDebugTracking = new();

        private uint _countFaultedTasksAllTime = 0;

        public void AddTaskToTrack(Task task)
        {
            _tasksForDebugTracking.Enqueue(task);

            // TODO: evaluate the use of continuation to increment the task counts and remove the task
            // .. from the concurrent collection to minimize the memory footprint + leak condition
        }

        public void LogTaskStatisticsToConsole()
        {
            uint countPendingTasks = 0;
            List<Task> tasksToReAddToQueue = new List<Task>();
            Task parseTask;
            while (_tasksForDebugTracking.TryDequeue(out parseTask))
            {
                switch (parseTask.Status)
                {
                    case TaskStatus.Faulted:
                        _countFaultedTasksAllTime++;
                        break;
                    case TaskStatus.RanToCompletion:
                        break;
                    case TaskStatus.Created:
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
                _tasksForDebugTracking.Enqueue(taskToReAdd);

            Console.WriteLine("{0} pending tasks (since last report)", countPendingTasks);
            Console.WriteLine("{0} faulted tasks (all time)", _countFaultedTasksAllTime);
            Console.WriteLine();
        }
    }
}
