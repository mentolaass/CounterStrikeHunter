using CounterStrikeHunter.Core.Service.Queue.Work;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CounterStrikeHunter.Core.Service.Queue
{
    public class QueueService : IQueueService
    {
        private readonly ObservableCollection<CancellableTask> _tasks = new ObservableCollection<CancellableTask>();

        public event EventHandler CreateTaskEvent;
        public event EventHandler CancelTaskEvent;

        public int CountTasks => _tasks.Count;
        public int CountWorkingTasks => _tasks.Count(task => task.IsWorking);
        public ObservableCollection<CancellableTask> Tasks => _tasks;

        private readonly int _poolSize;

        public QueueService(int poolSize)
        {
            _poolSize = poolSize;
        }

        public void RaiseEvents()
        {
            CancelTaskEvent?.Invoke(this, EventArgs.Empty);
            CreateTaskEvent?.Invoke(this, EventArgs.Empty);
        }

        public void CancelAllTasks()
        {
            RaiseCancelEvent();

            foreach (var task in _tasks)
            {
                task.Cancel();
            }
            _tasks.Clear();
        }

        public void RunTask(TaskMetaInfo meta)
        {
            RaiseCreateEvent();

            var task = _tasks.FirstOrDefault(t => string.Equals(t.Name, meta.Name, StringComparison.OrdinalIgnoreCase));
            task?.Run();
        }

        public void CancelTask(TaskMetaInfo meta)
        {
            RaiseCancelEvent();

            var task = _tasks.FirstOrDefault(t => string.Equals(t.Name, meta.Name, StringComparison.OrdinalIgnoreCase));
            if (task != null)
            {
                task.Cancel();
                App.Current.Dispatcher.Invoke(() => _tasks.Remove(task));
            }
        }

        public CancellableTask GetFirstTask()
        {
            return _tasks.FirstOrDefault(task => task.IsWorking);
        }

        public void CancelFirstTask()
        {
            RaiseCancelEvent();

            var task = GetFirstTask();
            if (task != null)
            {
                task.Cancel();
                App.Current.Dispatcher.Invoke(() => _tasks.Remove(task));
            }
        }

        public bool HasPutToQueue() => CountTasks < _poolSize;

        public void PutToQueue(CancellableTask task)
        {
            if (!HasPutToQueue() || _tasks.Any(t => string.Equals(t.Name, task.Name, StringComparison.OrdinalIgnoreCase)))
                return;

            _tasks.Add(task);
        }

        private void RaiseCreateEvent() => CreateTaskEvent?.Invoke(this, EventArgs.Empty);
        private void RaiseCancelEvent() => CancelTaskEvent?.Invoke(this, EventArgs.Empty);
    }

}
