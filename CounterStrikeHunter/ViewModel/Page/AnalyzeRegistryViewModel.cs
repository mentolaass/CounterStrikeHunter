using CounterStrikeHunter.Core;
using CounterStrikeHunter.Core.Regedit;
using CounterStrikeHunter.Core.Regedit._7Zip;
using CounterStrikeHunter.Core.Regedit.AppCompatCache;
using CounterStrikeHunter.Core.Regedit.Bam;
using CounterStrikeHunter.Core.Regedit.Radar;
using CounterStrikeHunter.Core.Regedit.ShellBag;
using CounterStrikeHunter.Core.Regedit.UserAssist;
using CounterStrikeHunter.Core.Regedit.WinRar;
using CounterStrikeHunter.Core.Service.Navigation;
using CounterStrikeHunter.Core.Service.Queue.Work;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace CounterStrikeHunter.ViewModel.Page
{
    public class AnalyzeRegistryViewModel : ViewModelBase
    {
        private IFrameNavigationService _navigationService;

        private RegistryReader _reader = new RegistryReader();

        private ObservableCollection<RegistryReadResultWrapper> _registryReadResults = new ObservableCollection<RegistryReadResultWrapper>();

        private ObservableCollection<AppCompatCacheEntry> _appCompatCacheEntries;

        private ObservableCollection<ShellBagEntry> _shellBagEntries;

        private ObservableCollection<RadarEntry> _radarEntries;

        private ObservableCollection<BamEntry> _bamEntries;

        private ObservableCollection<_7ZipEntry> _7zipEntries;

        private ObservableCollection<WinRarEntry> _winRarEntries;

        private ObservableCollection<UserAssistEntry> _userAssistEntries;

        private string _searchPattern;

        public AnalyzeRegistryViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            RunRegistryCollectDataTask();
        }

        public ObservableCollection<RegistryReadResultWrapper> RegistryReadResults
        {
            get => _registryReadResults;
            set
            {
                _registryReadResults = value;

                RaisePropertyChanged(nameof(RegistryReadResults));
            }
        }

        public ObservableCollection<IRegistryEntry> UserAssistEntries
        {
            get => new ObservableCollection<IRegistryEntry>(_registryReadResults
                .First(r => r.Name == "UserAssist").Result);
            set
            {
                _registryReadResults
                    .First(r => r.Name == "UserAssist").Result = value;

                RaisePropertyChanged(nameof(UserAssistEntries));
            }
        }

        public ObservableCollection<IRegistryEntry> WinRarEntries
        {
            get => new ObservableCollection<IRegistryEntry>(_registryReadResults
                .First(r => r.Name == "WinRar Archive History").Result);
            set
            {
                _registryReadResults
                    .First(r => r.Name == "WinRar Archive History").Result = value;

                RaisePropertyChanged(nameof(WinRarEntries));
            }
        }

        public ObservableCollection<IRegistryEntry> _7ZipEntries
        {
            get => new ObservableCollection<IRegistryEntry>(_registryReadResults
                .First(r => r.Name == "7Zip Archive History").Result);
            set
            {
                _registryReadResults
                    .First(r => r.Name == "7Zip Archive History").Result = value;

                RaisePropertyChanged(nameof(_7ZipEntries));
            }
        }

        public ObservableCollection<IRegistryEntry> BamEntries
        {
            get => new ObservableCollection<IRegistryEntry>(_registryReadResults
                .First(r => r.Name == "Bam").Result);
            set
            {
                _registryReadResults
                    .First(r => r.Name == "Bam").Result = value;

                RaisePropertyChanged(nameof(BamEntries));
            }
        }

        public ObservableCollection<IRegistryEntry> RadarEntries
        {
            get => new ObservableCollection<IRegistryEntry>(_registryReadResults
                .First(r => r.Name == "Radar").Result);
            set
            {
                _registryReadResults
                    .First(r => r.Name == "Radar").Result = value;

                RaisePropertyChanged(nameof(RadarEntries));
            }
        }

        public ObservableCollection<IRegistryEntry> AppCompatCacheEntries
        {
            get => new ObservableCollection<IRegistryEntry>(_registryReadResults
                .First(r => r.Name == "App Compat Cache").Result);
            set
            {
                _registryReadResults
                    .First(r => r.Name == "App Compat Cache").Result = value;

                RaisePropertyChanged(nameof(AppCompatCacheEntries));
            }
        }

        public ObservableCollection<IRegistryEntry> ShellBagEntries
        {
            get => new ObservableCollection<IRegistryEntry>(_registryReadResults
                .First(r => r.Name == "Shell Bag").Result);
            set
            {
                _registryReadResults
                    .First(r => r.Name == "Shell Bag").Result = value;

                RaisePropertyChanged(nameof(ShellBagEntries));
            }
        }

        public string SearchPattern
        {
            get => _searchPattern;
            set
            {
                _searchPattern = value;
                RaisePropertyChanged(nameof(SearchPattern));
                UpdateRegistryData();
            }
        }

        private void UpdateRegistryData()
        {
            new Task(() =>
            {
                var shellBag = new ObservableCollection<IRegistryEntry>(_shellBagEntries);
                var appCompatCache = new ObservableCollection<IRegistryEntry>(_appCompatCacheEntries);
                var radar = new ObservableCollection<IRegistryEntry>(_radarEntries);
                var bam = new ObservableCollection<IRegistryEntry>(_bamEntries);
                var _7zip = new ObservableCollection<IRegistryEntry>(_7zipEntries);
                var winRar = new ObservableCollection<IRegistryEntry>(_winRarEntries);
                var userAssist = new ObservableCollection<IRegistryEntry>(_userAssistEntries);

                if (_searchPattern != null && _searchPattern.Length != 0)
                {
                    shellBag = new ObservableCollection<IRegistryEntry>(shellBag.Where(p =>
                        Util.ContainsFilteredWords(p.Path, SearchPattern)));

                    appCompatCache = new ObservableCollection<IRegistryEntry>(appCompatCache.Where(p =>
                        Util.ContainsFilteredWords(p.Path, SearchPattern)));

                    radar = new ObservableCollection<IRegistryEntry>(radar.Where(p =>
                        Util.ContainsFilteredWords(p.Path, SearchPattern)));

                    bam = new ObservableCollection<IRegistryEntry>(bam.Where(p =>
                        Util.ContainsFilteredWords(p.Path, SearchPattern)));

                    _7zip = new ObservableCollection<IRegistryEntry>(_7zip.Where(p =>
                        Util.ContainsFilteredWords(p.Path, SearchPattern)));

                    winRar = new ObservableCollection<IRegistryEntry>(winRar.Where(p =>
                        Util.ContainsFilteredWords(p.Path, SearchPattern)));

                    userAssist = new ObservableCollection<IRegistryEntry>(userAssist.Where(p =>
                        Util.ContainsFilteredWords(p.Path, SearchPattern)));
                }

                ShellBagEntries = shellBag;
                AppCompatCacheEntries = appCompatCache;
                RadarEntries = radar;
                BamEntries = bam;
                WinRarEntries = winRar;
                _7ZipEntries = _7zip;
                UserAssistEntries = userAssist;
            }).Start();
        }

        private void InitRegistryReader()
        {
            _reader.RegisterPath(
                "App Compat Cache",
                App.GetLocalizedString("AppCompatCache_Description"),
                "SYSTEM\\CurrentControlSet\\Control\\Session Manager\\AppCompatCache",
                new AppCompatCacheConverter(),
                new AppCompatCacheReader());

            _reader.RegisterPath(
                "Shell Bag",
                "(UNFINISHED) " + App.GetLocalizedString("ShellBag_Description"),
                "SOFTWARE\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\Shell\\BagMRU",
                new ShellBagConverter(),
                new ShellBagReader());

            _reader.RegisterPath(
                "Radar",
                App.GetLocalizedString("Radar_Description"),
                "SOFTWARE\\Microsoft\\RADAR\\HeapLeakDetection",
                new RadarConverter(),
                new RadarReader());

            _reader.RegisterPath(
                "Bam",
                App.GetLocalizedString("Bam_Description"),
                $"SYSTEM\\CurrentControlSet\\Services\\bam\\State\\UserSettings\\{Util.GetTableUser()}",
                new BamConverter(),
                new BamReader());

            _reader.RegisterPath(
                "7Zip Archive History",
                App.GetLocalizedString("7Zip_Description"),
                "Software\\7-Zip\\Compression",
                new _7ZipConverter(),
                new _7ZipReader());

            _reader.RegisterPath(
                "WinRar Archive History",
                App.GetLocalizedString("WinRar_Description"),
                "Software\\WinRAR",
                new WinRarConverter(),
                new WinRarReader());

            _reader.RegisterPath(
                "UserAssist",
                App.GetLocalizedString("UserAssist_Description"),
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\UserAssist",
                new UserAssistConverter(),
                new UserAssistReader());
        }

        public void RunRegistryCollectDataTask()
        {
            App.QueueService.PutToQueue(new CancellableTask(Tasks.REGISTRY_READ_DATA_TASK, token =>
            {
                InitRegistryReader();

                var _appCompatCacheResult = _reader.GetValue<byte[], AppCompatCacheReadResult>("App Compat Cache");
                var _shellBagReadResult = _reader.GetValue<List<byte[]>, ShellBagReadResult>("Shell Bag");
                var _radarReadResult = _reader.GetValue<List<string>, RadarReadResult>("Radar");
                var _bamReadResult = _reader.GetValue<List<string>, BamReadResult>("Bam");
                var _winRarReadResult = _reader.GetValue<List<string>, WinRarReadResult>("WinRar Archive History");
                var _7zipReadResult = _reader.GetValue<List<string>, _7ZipReadResult>("7Zip Archive History");
                var _userAssistReadResult = _reader.GetValue<List<string>, UserAssistReadResult>("UserAssist");

                _appCompatCacheEntries = _appCompatCacheResult.Result;
                _shellBagEntries = _shellBagReadResult.Result;
                _radarEntries = _radarReadResult.Result;
                _bamEntries = _bamReadResult.Result;
                _7zipEntries = _7zipReadResult.Result;
                _winRarEntries = _winRarReadResult.Result;
                _userAssistEntries = _userAssistReadResult.Result;

                App.Current.Dispatcher.Invoke(() =>
                {
                    RegistryReadResults.Add(new RegistryReadResultWrapper()
                    {
                        Name = _appCompatCacheResult.Name,
                        Description = _appCompatCacheResult.Description,
                        Result = new ObservableCollection<IRegistryEntry>(_appCompatCacheResult.Result.Select((e) => (IRegistryEntry)e)),
                        Path = _appCompatCacheResult.Path
                    });

                    RegistryReadResults.Add(new RegistryReadResultWrapper()
                    {
                        Name = _shellBagReadResult.Name,
                        Description = _shellBagReadResult.Description,
                        Result = new ObservableCollection<IRegistryEntry>(_shellBagReadResult.Result.Select((e) => (IRegistryEntry)e)),
                        Path = _shellBagReadResult.Path
                    });

                    RegistryReadResults.Add(new RegistryReadResultWrapper()
                    {
                        Name = _radarReadResult.Name,
                        Description = _radarReadResult.Description,
                        Result = new ObservableCollection<IRegistryEntry>(_radarReadResult.Result.Select((e) => (IRegistryEntry)e)),
                        Path = _radarReadResult.Path
                    });

                    RegistryReadResults.Add(new RegistryReadResultWrapper()
                    {
                        Name = _bamReadResult.Name,
                        Description = _bamReadResult.Description,
                        Result = new ObservableCollection<IRegistryEntry>(_bamReadResult.Result.Select((e) => (IRegistryEntry)e)),
                        Path = _bamReadResult.Path
                    });

                    RegistryReadResults.Add(new RegistryReadResultWrapper()
                    {
                        Name = _winRarReadResult.Name,
                        Description = _winRarReadResult.Description,
                        Result = new ObservableCollection<IRegistryEntry>(_winRarReadResult.Result.Select((e) => (IRegistryEntry)e)),
                        Path = _winRarReadResult.Path
                    });

                    RegistryReadResults.Add(new RegistryReadResultWrapper()
                    {
                        Name = _7zipReadResult.Name,
                        Description = _7zipReadResult.Description,
                        Result = new ObservableCollection<IRegistryEntry>(_7zipReadResult.Result.Select((e) => (IRegistryEntry)e)),
                        Path = _7zipReadResult.Path
                    });

                    RegistryReadResults.Add(new RegistryReadResultWrapper()
                    {
                        Name = _userAssistReadResult.Name,
                        Description = _userAssistReadResult.Description,
                        Result = new ObservableCollection<IRegistryEntry>(_userAssistReadResult.Result.Select((e) => (IRegistryEntry)e)),
                        Path = _userAssistReadResult.Path
                    });
                });

                App.QueueService.CancelTask(Tasks.REGISTRY_READ_DATA_TASK);
            }));

            App.QueueService.RunTask(Tasks.REGISTRY_READ_DATA_TASK);
        }
    }
}
