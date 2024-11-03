using CounterStrikeHunter.Core.Service.Queue.Work;
using CounterStrikeHunter.Model;
using GalaSoft.MvvmLight;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CounterStrikeHunter.ViewModel.Dialog
{
    public class TaskDialogViewModel : ViewModelBase, IModalDialogViewModel
    {
        private bool? _dialogResult;

        public TaskDialogViewModel()
        {
            App.QueueService.CancelTaskEvent += QueueService_CancelTaskEvent;
            App.QueueService.CreateTaskEvent += QueueService_CreateTaskEvent;
        }

        public ICommand Close
        {
            get => new RelayCommand((obj) =>
            {
                App.QueueService.CancelTaskEvent -= QueueService_CancelTaskEvent;
                App.QueueService.CreateTaskEvent -= QueueService_CreateTaskEvent;

                foreach (var task in App.QueueService.Tasks)
                {
                    task.IsExpanded = false;
                }

                DialogResult = true;
            });
        }

        public ObservableCollection<CancellableTask> Tasks
        {
            get => App.QueueService.Tasks;
            set
            {

            }
        }

        private void QueueService_CreateTaskEvent(object sender, System.EventArgs e)
        {
            RaisePropertyChanged(nameof(Tasks));
        }

        private void QueueService_CancelTaskEvent(object sender, System.EventArgs e)
        {
            RaisePropertyChanged(nameof(Tasks));
        }

        public bool? DialogResult
        {
            get => _dialogResult;

            private set => Set(nameof(DialogResult), ref _dialogResult, value);
        }
    }
}
