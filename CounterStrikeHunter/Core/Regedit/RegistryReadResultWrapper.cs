using CounterStrikeHunter.Model;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CounterStrikeHunter.Core.Regedit
{
    public class RegistryReadResultWrapper : ViewModelBase, IRegistryReadResult<IRegistryEntry>
    {
        private bool _expanded;

        public bool IsExpanded
        {
            get => _expanded;
            set
            {
                _expanded = value;

                RaisePropertyChanged(nameof(IsExpanded));
            }
        }

        public bool HasExpand { get; set; } = true;
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }

        private ObservableCollection<IRegistryEntry> _result;
        public ObservableCollection<IRegistryEntry> Result
        {
            get => _result;
            set
            {
                _result = value;

                RaisePropertyChanged(nameof(Result));
            }
        }

        public ICommand Expand => new RelayCommand(_ => IsExpanded = !IsExpanded);
    }
}
