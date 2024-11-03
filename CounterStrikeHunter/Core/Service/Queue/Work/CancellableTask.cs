using CounterStrikeHunter.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CounterStrikeHunter.Core.Service.Queue.Work
{
    public class CancellableTask : ViewModelBase
    {
        private CancellationTokenSource _token;

        private Task _task;

        private bool _working;

        private string _name, _desc, _date;

        private Flag[] _flags;

        private Action<CancellationTokenSource> _taskAction;

        #region UI States

        private bool _expanded;

        #endregion UI States

        public CancellableTask(TaskMetaInfo meta, Action<CancellationTokenSource> action)
        {
            _token = new CancellationTokenSource();

            _name = meta.Name;

            _desc = meta.Description;

            _flags = meta.Flags;

            _taskAction = action;

            _date = DateTimeOffset.Now.ToString("yyyy.MM.dd HH:mm:ss");

            _task = new Task(() => action.Invoke(_token), _token.Token);
        }

        public ICommand TryRun
        {
            get => new RelayCommand((obj) => Run());
        }

        public ICommand TryCancel
        {
            get => new RelayCommand((obj) => Cancel());
        }

        public ICommand Expand
        {
            get => new RelayCommand((obj) =>
            {
                this.IsExpanded = !this.IsExpanded;
            });
        }

        public void Run()
        {
            if (_working)
            {
                return;
            }

            if (_task.IsCompleted)
            {
                _token = new CancellationTokenSource();

                _task = new Task(() =>
                {
                    _taskAction.Invoke(_token);
                });
            }

            _task.Start();

            IsWorking = true;
        }

        public void Cancel()
        {
            if (!_working)
            {
                return;
            }

            _token.Cancel();

            IsWorking = false;
        }

        public bool IsWorking
        {
            get => _working;
            set
            {
                _working = value;

                RaisePropertyChanged(nameof(IsWorking));
            }
        }

        public bool IsExpanded
        {
            get => _expanded;
            set
            {
                _expanded = value;

                RaisePropertyChanged(nameof(IsExpanded));
            }
        }

        public ObservableCollection<Flag> Flags
        {
            get => new ObservableCollection<Flag>(_flags);
        }

        public string Name
        {
            get => _name;
        }

        public string Description
        {
            get => _desc;
        }

        public string Date
        {
            get => _date;
        }
    }
}
