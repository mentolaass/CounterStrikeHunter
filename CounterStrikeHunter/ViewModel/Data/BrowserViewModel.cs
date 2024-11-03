using CounterStrikeHunter.Model;
using GalaSoft.MvvmLight;
using System.Windows.Input;

namespace CounterStrikeHunter.Core.Service.Browser.Pair
{
    public class BrowserViewModel : ViewModelBase
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

        public bool HasExpand { get; set; }
        public bool IsDownloadHistory { get; set; }
        public bool IsVisitHistory { get; set; }
        public string Url { get; set; }
        public string File { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public bool Hidden { get; set; }

        public ICommand Expand => new RelayCommand(_ => IsExpanded = !IsExpanded);
    }
}
