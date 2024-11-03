using CounterStrikeHunter.Core;
using CounterStrikeHunter.Core.Service.Browser;
using CounterStrikeHunter.Core.Service.Browser.Pair;
using CounterStrikeHunter.Core.Service.Browser.Reader;
using CounterStrikeHunter.Core.Service.Navigation;
using CounterStrikeHunter.Core.Service.Notification;
using CounterStrikeHunter.Core.Service.Queue.Work;
using CounterStrikeHunter.Model.Sorting;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CounterStrikeHunter.ViewModel.Page
{
    public class AnalyzeBrowsersViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _navigationService;

        private readonly INotificationService _notificationService;

        private BrowserData _browserYandexData, _browserChromeData, _browserEdgeData, _browserBraveData, _browserOperaData;

        private ObservableCollection<BrowserViewModel> _currentFilteredAndSortedPairs;

        private Browser _selectedBrowser;

        private DataPairType _selectedPairType;

        private SortingType _selectedSortingType;

        private string _searchPattern;

        public ObservableCollection<DataPairType> PairTypes { get; }
        public ObservableCollection<SortingType> SortingTypes { get; }

        public AnalyzeBrowsersViewModel(IFrameNavigationService navigationService, INotificationService notificationService)
        {
            _navigationService = navigationService;
            PairTypes = InitializePairTypes();
            SortingTypes = InitializeSortingTypes();
            SelectedBrowser = (int) Browser.NONE;
            _notificationService = notificationService;
        }

        public ObservableCollection<BrowserViewModel> CurrentBrowserPairs
        {
            get => _currentFilteredAndSortedPairs;
            private set
            {
                _currentFilteredAndSortedPairs = value;
                RaisePropertyChanged(nameof(CurrentBrowserPairs));
            }
        }

        public string SearchPattern
        {
            get => _searchPattern;
            set
            {
                _searchPattern = value;
                RaisePropertyChanged(nameof(SearchPattern));
                UpdateCurrentBrowserData();
            }
        }

        public int SelectedBrowser
        {
            get => (int) _selectedBrowser;
            set
            {
                switch (value)
                {
                    case 0:
                        _selectedBrowser = Browser.YANDEX;
                        break;
                    case 1:
                        _selectedBrowser = Browser.CHROME;
                        break;
                    case 2:
                        _selectedBrowser = Browser.EDGE;
                        break;
                    case 3:
                        _selectedBrowser = Browser.OPERA;
                        break;
                    case 4:
                        _selectedBrowser = Browser.BRAVE;
                        break;
                }

                RaisePropertyChanged(nameof(SelectedBrowser));
                RunBrowsersCollectDataTask();
                UpdateCurrentBrowserData();
            }
        }

        public DataPairType SelectedPairType
        {
            get => _selectedPairType;
            set
            {
                _selectedPairType = value;
                RaisePropertyChanged(nameof(SelectedPairType));
                UpdateCurrentBrowserData();
            }
        }

        public SortingType SelectedSortingType
        {
            get => _selectedSortingType;
            set
            {
                _selectedSortingType = value;
                RaisePropertyChanged(nameof(SelectedSortingType));
                UpdateCurrentBrowserData();
            }
        }

        private ObservableCollection<DataPairType> InitializePairTypes()
        {
            return new ObservableCollection<DataPairType>()
            {
                new DataPairType 
                { 
                    Id = (int)PairType.HISTORY, 
                    Icon = Geometry.Parse("M 511.5,232.5 C 511.5,248.167 511.5,263.833 511.5,279.5C 500.132,368.357 454.132,432.191 373.5,471C 298.664,501.36 226.331,496.36 156.5,456C 129.129,438.297 105.962,416.13 87,389.5C 81.106,376.634 83.9394,366.134 95.5,358C 105.866,353.94 115.032,355.773 123,363.5C 160.999,416.842 212.999,444.842 279,447.5C 358.854,443.429 416.521,405.762 452,334.5C 479.731,265.297 471.731,200.631 428,140.5C 379.946,83.4139 318.779,60.2472 244.5,71C 168.437,88.2292 118.771,134.063 95.5,208.5C 95.8333,208.833 96.1667,209.167 96.5,209.5C 107.827,201.501 119.494,194.001 131.5,187C 151.38,183.879 160.88,192.379 160,212.5C 159.479,215.874 158.146,218.874 156,221.5C 129.714,240.449 102.88,258.616 75.5,276C 65.4353,278.858 56.9353,276.358 50,268.5C 32.88,242.907 16.0466,217.241 -0.5,191.5C -0.5,186.167 -0.5,180.833 -0.5,175.5C 6.36432,162.747 16.6977,158.914 30.5,164C 33.2953,165.793 35.7953,167.96 38,170.5C 43.3333,178.5 48.6667,186.5 54,194.5C 78.9039,110.596 133.071,55.4295 216.5,29C 308.17,7.34199 386.67,30.1753 452,97.5C 486.01,136.192 505.843,181.192 511.5,232.5 Z M 275.5,125.5 C 287.006,125.99 294.506,131.657 298,142.5C 298.333,177.167 298.667,211.833 299,246.5C 320.761,267.258 341.428,288.925 361,311.5C 364.908,332.873 356.075,342.706 334.5,341C 331.554,340.194 328.888,338.861 326.5,337C 303.638,314.472 281.138,291.639 259,268.5C 258,266.5 257,264.5 256,262.5C 255.333,221.833 255.333,181.167 256,140.5C 259.556,131.492 266.056,126.492 275.5,125.5 Z"), 
                    Name = "Browser_Data_History"
                },
                new DataPairType 
                { 
                    Id = (int)PairType.DOWNLOAD, 
                    Icon = Geometry.Parse("M 247.5,-0.5 C 252.833,-0.5 258.167,-0.5 263.5,-0.5C 274.35,3.51991 281.183,11.1866 284,22.5C 284.333,114.5 284.667,206.5 285,298.5C 306.833,276.667 328.667,254.833 350.5,233C 367.968,222.244 382.801,225.077 395,241.5C 400.033,252.226 399.366,262.56 393,272.5C 352.833,312.667 312.667,352.833 272.5,393C 261.204,400.325 249.871,400.325 238.5,393C 198.333,352.833 158.167,312.667 118,272.5C 108.789,255.552 111.956,241.385 127.5,230C 139.071,224.724 150.071,225.724 160.5,233C 182.333,254.833 204.167,276.667 226,298.5C 226.333,206.5 226.667,114.5 227,22.5C 229.817,11.1866 236.65,3.51991 247.5,-0.5 Z M 511.5,361.5 C 511.5,378.5 511.5,395.5 511.5,412.5C 500.833,467.833 467.833,500.833 412.5,511.5C 307.833,511.5 203.167,511.5 98.5,511.5C 43.6231,500.957 10.6231,468.29 -0.5,413.5C -0.5,396.167 -0.5,378.833 -0.5,361.5C 5.6175,345.434 17.2842,338.601 34.5,341C 45.2377,344.071 52.4044,350.904 56,361.5C 56.3333,374.167 56.6667,386.833 57,399.5C 59.0683,423.23 70.9016,440.063 92.5,450C 98.29,452.281 104.29,453.614 110.5,454C 207.167,454.667 303.833,454.667 400.5,454C 423.698,451.463 440.198,439.63 450,418.5C 452.442,412.4 453.775,406.067 454,399.5C 454.333,386.833 454.667,374.167 455,361.5C 462.689,342.819 476.189,336.653 495.5,343C 503.226,347.217 508.559,353.384 511.5,361.5 Z"), 
                    Name = "Browser_Data_Downloads"
                },
            };
        }

        private ObservableCollection<SortingType> InitializeSortingTypes()
        {
            return new ObservableCollection<SortingType>()
            {
                new SortingType 
                { 
                    Id = (int)SortType.BY_DATE_NEAR, 
                    Icon = Geometry.Parse("M 243.5,-0.5 C 250.833,-0.5 258.167,-0.5 265.5,-0.5C 269.4,1.1227 273.067,3.28937 276.5,6C 337,66.5 397.5,127 458,187.5C 465.918,198.198 467.585,209.865 463,222.5C 453.801,237.509 440.634,242.676 423.5,238C 420.339,236.587 417.339,234.92 414.5,233C 378.167,196.667 341.833,160.333 305.5,124C 296.343,118.677 290.176,121.177 287,131.5C 286.667,250.167 286.333,368.833 286,487.5C 282.728,499.601 275.228,507.601 263.5,511.5C 258.167,511.5 252.833,511.5 247.5,511.5C 235.31,507.473 227.81,499.139 225,486.5C 224.667,367.833 224.333,249.167 224,130.5C 220.433,121.203 214.266,119.036 205.5,124C 169.5,160 133.5,196 97.5,232C 84.6469,241.203 71.3136,241.87 57.5,234C 46.1211,224.188 42.6211,212.021 47,197.5C 48.497,193.835 50.497,190.502 53,187.5C 113.528,127.306 173.694,66.8059 233.5,6C 236.728,3.56031 240.061,1.39364 243.5,-0.5 Z"), 
                    Name = "Browser_Sort_By_Date_Near"
                },
                new SortingType 
                { 
                    Id = (int)SortType.BY_DATE_FAR, 
                    Icon = Geometry.Parse("M 247.5,-0.5 C 252.833,-0.5 258.167,-0.5 263.5,-0.5C 275.228,3.39862 282.728,11.3986 286,23.5C 286.333,142.5 286.667,261.5 287,380.5C 289.561,390.197 295.394,393.031 304.5,389C 340.5,353 376.5,317 412.5,281C 429.321,268.704 445.154,269.871 460,284.5C 467.834,298.311 467.167,311.644 458,324.5C 397.167,385.333 336.333,446.167 275.5,507C 272.761,508.026 270.428,509.526 268.5,511.5C 260.833,511.5 253.167,511.5 245.5,511.5C 241.428,509.29 237.428,506.79 233.5,504C 173.333,443.833 113.167,383.667 53,323.5C 45.0818,312.802 43.4152,301.135 48,288.5C 57.0901,272.936 70.2567,267.769 87.5,273C 90.6612,274.413 93.6612,276.08 96.5,278C 132.833,314.333 169.167,350.667 205.5,387C 211.257,391.374 216.757,390.874 222,385.5C 222.862,383.913 223.529,382.246 224,380.5C 224.333,261.833 224.667,143.167 225,24.5C 227.81,11.8608 235.31,3.52747 247.5,-0.5 Z"),
                    Name = "Browser_Sort_By_Date_Far"
                },
            };
        }

        private void UpdateCurrentBrowserData()
        {
            new Task(() =>
            {
                var pairs = GetBrowserPairsByType();

                if (_selectedPairType != null)
                {
                    switch (_selectedPairType.Id)
                    {
                        case (int)PairType.HISTORY:
                            pairs = new ObservableCollection<BrowserViewModel>(pairs.Where(p => p.IsVisitHistory));
                            break;
                        case (int)PairType.DOWNLOAD:
                            pairs = new ObservableCollection<BrowserViewModel>(pairs.Where(p => p.IsDownloadHistory));
                            break;
                    }
                }

                if (_selectedSortingType != null)
                {
                    switch (_selectedSortingType.Id)
                    {
                        case (int)SortType.BY_DATE_NEAR:
                            pairs = new ObservableCollection<BrowserViewModel>(pairs.OrderByDescending(p => p.Date));
                            break;
                        case (int)SortType.BY_DATE_FAR:
                            pairs = new ObservableCollection<BrowserViewModel>(pairs.OrderBy(p => p.Date));
                            break;
                    }
                }

                if (_searchPattern != null && _searchPattern.Length != 0)
                {
                    pairs = new ObservableCollection<BrowserViewModel>(pairs.Where(p =>
                        Util.ContainsFilteredWords(p.Url, SearchPattern) ||
                        Util.ContainsFilteredWords(p.Title, SearchPattern) ||
                        Util.ContainsFilteredWords(p.File, SearchPattern)));
                }

                CurrentBrowserPairs = pairs;
            }).Start();
        }

        private ObservableCollection<BrowserViewModel> GetBrowserPairsByType()
        {
            switch (_selectedBrowser)
            {
                case Browser.YANDEX:
                    return new ObservableCollection<BrowserViewModel>(_browserYandexData?.Pairs ?? Enumerable.Empty<BrowserViewModel>());
                case Browser.CHROME:
                    return new ObservableCollection<BrowserViewModel>(_browserChromeData?.Pairs ?? Enumerable.Empty<BrowserViewModel>());
                case Browser.EDGE:
                    return new ObservableCollection<BrowserViewModel>(_browserEdgeData?.Pairs ?? Enumerable.Empty<BrowserViewModel>());
                case Browser.OPERA:
                    return new ObservableCollection<BrowserViewModel>(_browserOperaData?.Pairs ?? Enumerable.Empty<BrowserViewModel>());
                case Browser.BRAVE:
                    return new ObservableCollection<BrowserViewModel>(_browserBraveData?.Pairs ?? Enumerable.Empty<BrowserViewModel>());
                default:
                    return new ObservableCollection<BrowserViewModel>();
            }
        }

        public void RunBrowsersCollectDataTask()
        {
            App.QueueService.PutToQueue(new CancellableTask(Tasks.BROWSER_READ_DATA_TASK, token =>
            {
                LoadBrowserData();

                RaisePropertyChanged(nameof(CurrentBrowserPairs));
                RaisePropertyChanged(nameof(SelectedBrowser));

                App.QueueService.CancelTask(Tasks.BROWSER_READ_DATA_TASK);
            }));

            App.QueueService.RunTask(Tasks.BROWSER_READ_DATA_TASK);
        }

        private void LoadBrowserData()
        {
            _browserYandexData = new ChromiumBrowserDataReader(Browsers.YANDEX).Read();
            _browserChromeData = new ChromiumBrowserDataReader(Browsers.CHROME).Read();
            _browserEdgeData = new ChromiumBrowserDataReader(Browsers.EDGE).Read();
            _browserOperaData = new ChromiumBrowserDataReader(Browsers.OPERA).Read();
            _browserBraveData = new ChromiumBrowserDataReader(Browsers.BRAVE).Read();
        }

        public bool YandexExists
        {
            get => new ChromiumBrowserDataReader(Browsers.YANDEX).CanRead();
            set { }
        }

        public bool ChromeExists
        {
            get => new ChromiumBrowserDataReader(Browsers.CHROME).CanRead();
            set { }
        }

        public bool EdgeExists
        {
            get => new ChromiumBrowserDataReader(Browsers.EDGE).CanRead();
            set { }
        }

        public bool BraveExists
        {
            get => new ChromiumBrowserDataReader(Browsers.BRAVE).CanRead();
            set { }
        }

        public bool OperaExists
        {
            get => new ChromiumBrowserDataReader(Browsers.OPERA).CanRead();
            set { }
        }

        public enum Browser
        {
            NONE = -1,
            YANDEX = 0,
            CHROME = 1,
            EDGE = 2,
            OPERA = 3,
            BRAVE = 4
        }

        public enum SortType
        {
            BY_DATE_NEAR = 0,
            BY_DATE_FAR = 1
        }

        public enum PairType
        {
            HISTORY = 0,
            DOWNLOAD = 1
        }
    }
}
