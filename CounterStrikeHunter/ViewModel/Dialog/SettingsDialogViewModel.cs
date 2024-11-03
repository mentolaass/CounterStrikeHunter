using CounterStrikeHunter.Model;
using CounterStrikeHunter.Model.Language;
using GalaSoft.MvvmLight;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CounterStrikeHunter.ViewModel.Dialog
{
    public class SettingsDialogViewModel : ViewModelBase, IModalDialogViewModel
    {
        private bool? _dialogResult;

        private Language _selectedLanguage;

        public SettingsDialogViewModel()
        {
            _selectedLanguage = App.Languages.Where((lang) => lang.Culture.Name == App.Language.Name).First();
        }

        public ObservableCollection<Language> Languages
        {
            get => new ObservableCollection<Language>(App.Languages);
            set { }
        }

        public Language SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;

                App.Language = value.Culture;

                RaisePropertyChanged(nameof(SelectedLanguage));

                Process.Start(Application.ResourceAssembly.Location);

                Application.Current.Shutdown();
            }
        }

        public ICommand Close
        {
            get => new RelayCommand((obj) =>
            {
                DialogResult = true;
            });
        }

        public bool? DialogResult
        {
            get => _dialogResult;

            private set => Set(nameof(DialogResult), ref _dialogResult, value);
        }
    }
}
