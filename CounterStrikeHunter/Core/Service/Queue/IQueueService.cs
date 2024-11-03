using CounterStrikeHunter.Core.Service.Queue.Work;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Service.Queue
{
    public interface IQueueService
    {
        bool HasPutToQueue();

        void PutToQueue(CancellableTask task);

        CancellableTask GetFirstTask();

        void RunTask(TaskMetaInfo name);

        void CancelTask(TaskMetaInfo name);

        void CancelFirstTask();

        void CancelAllTasks();

        int CountTasks { get; }

        int CountWorkingTasks { get; }

        ObservableCollection<CancellableTask> Tasks { get; }
    }
}
