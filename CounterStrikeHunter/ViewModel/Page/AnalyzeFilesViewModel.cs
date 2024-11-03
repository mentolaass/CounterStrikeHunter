using CounterStrikeHunter.Core;
using CounterStrikeHunter.Core.NTFSUsnJournal;
using CounterStrikeHunter.Core.Service.Navigation;
using CounterStrikeHunter.Core.Service.Notification;
using CounterStrikeHunter.Core.Service.Queue.Work;
using CounterStrikeHunter.Model.FileData;
using CounterStrikeHunter.Model.Sorting;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CounterStrikeHunter.ViewModel.Page
{
    public class AnalyzeFilesViewModel : ViewModelBase
    {
        private IFrameNavigationService _navigationService;

        private INotificationService _notificationService;

        private ObservableCollection<UsnFileData> _usnFileDatas = new ObservableCollection<UsnFileData>();

        private ObservableCollection<UsnFileData> _usnTempFileDatas = new ObservableCollection<UsnFileData>();

        private ObservableCollection<SortingType> _sortingTypes;

        private SortingType _selectedSortingType;

        private string _searchPattern;

        public AnalyzeFilesViewModel(IFrameNavigationService navigationService, INotificationService notificationService)
        {
            _navigationService = navigationService;

            _notificationService = notificationService;

            RunLocalFilesGettingTask();

            SortTypes = InitializeSortingTypes();

            SelectedSortingType = SortTypes[2];
        }

        public ObservableCollection<UsnFileData> UsnFileDatas
        {
            get => _usnTempFileDatas;
            set
            {
                _usnTempFileDatas = value;

                RaisePropertyChanged(nameof(UsnFileDatas));
            }
        }

        public ObservableCollection<SortingType> SortTypes
        {
            get => _sortingTypes;
            set
            {
                _sortingTypes = value;

                RaisePropertyChanged(nameof(SortTypes));
            }
        }

        public SortingType SelectedSortingType
        {
            get => _selectedSortingType;
            set
            {
                _selectedSortingType = value;

                RaisePropertyChanged(nameof(SelectedSortingType));

                UpdateCurrentLocalFilesData();
            }
        }

        public string SearchPattern
        {
            get => _searchPattern;
            set
            {
                _searchPattern = value;

                RaisePropertyChanged(nameof(SearchPattern));

                UpdateCurrentLocalFilesData();
            }
        }

        private ObservableCollection<SortingType> InitializeSortingTypes()
        {
            return new ObservableCollection<SortingType>()
            {
                new SortingType
                {
                    Id = (int)SortType.DELETED,
                    Icon = Geometry.Parse("M 183.500 22.871 C 181.300 23.876, 178.026 26.213, 176.225 28.065 C 170.864 33.575, 170 38.451, 170 63.182 L 170 85 125.250 85.022 C 83.826 85.042, 80.203 85.182, 76.500 86.896 C 61.369 93.901, 59.602 115.350, 73.385 124.707 L 77.500 127.500 254.850 127.758 L 432.199 128.016 437.056 125.373 C 443.604 121.809, 447.263 116.187, 447.807 108.856 C 448.320 101.936, 446.770 96.947, 442.653 92.257 C 436.255 84.971, 436.838 85.047, 386.750 85.022 L 342 85 342 63.182 C 342 38.451, 341.136 33.575, 335.775 28.065 C 328.618 20.707, 332.444 21.044, 256 21.044 C 191.788 21.044, 187.250 21.158, 183.500 22.871 M 214 74.500 L 214 85 256 85 L 298 85 298 74.500 L 298 64 256 64 L 214 64 214 74.500 M 99.010 150.316 C 93.951 151.927, 87.628 158.671, 86.150 164.033 C 85.198 167.489, 84.989 197.122, 85.227 295 C 85.518 414.647, 85.635 421.852, 87.389 428 C 91.945 443.975, 99.772 456.836, 111.768 468.059 C 123.230 478.783, 133.332 484.376, 149.121 488.738 C 155.018 490.367, 163.097 490.500, 256 490.500 C 348.903 490.500, 356.982 490.367, 362.879 488.738 C 378.777 484.345, 389.067 478.647, 400.326 468 C 412.347 456.632, 420.075 443.904, 424.611 428 C 426.365 421.852, 426.482 414.647, 426.773 295 C 427.011 197.122, 426.802 167.489, 425.850 164.033 C 421.820 149.411, 401.851 144.173, 390.661 154.803 C 384.030 161.101, 384.551 150.560, 384 289.500 L 383.500 415.500 381.215 421.153 C 376.782 432.117, 368.117 440.782, 357.153 445.215 L 351.500 447.500 256 447.500 L 160.500 447.500 154.748 445.177 C 143.740 440.732, 135.267 432.236, 130.782 421.145 L 128.500 415.500 128 289.500 C 127.544 174.532, 127.355 163.237, 125.839 160.500 C 120.582 151.004, 109.755 146.894, 99.010 150.316 M 204.157 193.693 C 199.810 195.834, 195.709 199.986, 193.647 204.331 C 192.160 207.465, 192 215.597, 192 288.016 L 192 368.229 194.635 373.071 C 201.595 385.858, 219.539 387.894, 229.370 377.012 C 231.293 374.883, 233.344 371.422, 233.928 369.321 C 234.635 366.774, 234.983 339.323, 234.972 287 C 234.957 212.718, 234.857 208.285, 233.104 204.500 C 229.420 196.543, 221.993 191.983, 212.782 192.023 C 209.877 192.035, 205.996 192.787, 204.157 193.693 M 289.500 193.842 C 287.300 194.890, 284.286 197.042, 282.803 198.624 C 276.772 205.056, 276.998 201.547, 277.022 288.218 C 277.043 363.254, 277.143 367.714, 278.896 371.500 C 285.901 386.631, 307.350 388.398, 316.707 374.615 L 319.500 370.500 319.500 288 L 319.500 205.500 316.844 201.656 C 312.504 195.375, 307.550 192.608, 300 192.247 C 295.179 192.017, 292.467 192.429, 289.500 193.842"),
                    Name = "File_Sort_Deleted"
                },
                new SortingType
                {
                    Id = (int)SortType.EXISTS,
                    Icon = Geometry.Parse("M 70.010 21.750 C 69.387 24.913, 67.991 31.031, 66.907 35.346 C 61.759 55.842, 53.047 63.364, 28.410 68.582 C 21.903 69.960, 16.374 71.293, 16.123 71.544 C 15.209 72.458, 16.794 72.958, 28.410 75.418 C 55.081 81.067, 62.933 88.919, 68.582 115.590 C 71.042 127.206, 71.542 128.791, 72.456 127.877 C 72.707 127.626, 74.040 122.097, 75.418 115.590 C 81.420 87.253, 88.976 80.541, 122.250 73.990 C 125.412 73.368, 128 72.472, 128 72 C 128 71.528, 125.412 70.632, 122.250 70.010 C 87.210 63.111, 80.862 56.764, 73.987 21.750 C 73.366 18.587, 72.472 16, 72 16 C 71.528 16, 70.632 18.587, 70.010 21.750 M 438.010 21.750 C 437.387 24.913, 435.991 31.031, 434.907 35.346 C 429.759 55.842, 421.047 63.364, 396.410 68.582 C 389.903 69.960, 384.374 71.293, 384.123 71.544 C 383.209 72.458, 384.794 72.958, 396.410 75.418 C 423.081 81.067, 430.933 88.919, 436.582 115.590 C 437.960 122.097, 439.293 127.626, 439.544 127.877 C 440.458 128.791, 440.958 127.206, 443.418 115.590 C 449.420 87.253, 456.976 80.541, 490.250 73.990 C 493.413 73.368, 496 72.472, 496 72 C 496 71.528, 493.413 70.632, 490.250 70.010 C 455.210 63.111, 448.862 56.764, 441.987 21.750 C 441.366 18.587, 440.472 16, 440 16 C 439.528 16, 438.632 18.587, 438.010 21.750 M 250.174 26.314 C 248.202 28.414, 248 29.698, 248 40.140 C 248 50.880, 248.155 51.797, 250.314 53.826 C 253.486 56.805, 258.957 56.739, 261.826 53.686 C 263.798 51.586, 264 50.302, 264 39.860 C 264 29.120, 263.845 28.203, 261.686 26.174 C 258.514 23.195, 253.043 23.261, 250.174 26.314 M 244.500 105.346 C 233.127 109.112, 224.509 116.601, 219.582 126.997 C 216.748 132.977, 216.500 134.345, 216.500 144 C 216.500 153.655, 216.748 155.023, 219.582 161.003 C 223.530 169.333, 230.667 176.470, 238.997 180.418 C 244.977 183.252, 246.345 183.500, 256 183.500 C 265.655 183.500, 267.023 183.252, 273.003 180.418 C 281.333 176.470, 288.470 169.333, 292.418 161.003 C 295.252 155.023, 295.500 153.655, 295.500 144 C 295.500 134.345, 295.252 132.977, 292.418 126.997 C 288.451 118.627, 281.328 111.517, 273.003 107.619 C 267.519 105.051, 265.089 104.530, 257.500 104.297 C 251.970 104.128, 246.958 104.532, 244.500 105.346 M 135.236 162.982 C 129.742 164.280, 124.415 168.511, 122.005 173.490 C 119.091 179.510, 119.497 188.183, 122.928 193.190 C 124.201 195.047, 142.062 210.029, 162.621 226.483 L 200 256.400 200 279.691 L 200 302.983 191 341.666 C 186.050 362.941, 182 381.990, 182 383.996 C 182 393.015, 188.060 402.306, 196.419 406.103 C 206.127 410.512, 220.034 407.078, 226.038 398.788 C 227.991 396.093, 233.098 382.221, 242.187 354.923 C 249.499 332.965, 255.715 315, 256 315 C 256.285 315, 262.501 332.965, 269.813 354.923 C 278.902 382.221, 284.009 396.093, 285.962 398.788 C 291.966 407.078, 305.873 410.512, 315.581 406.103 C 323.949 402.302, 330 393.015, 330 383.973 C 330 381.954, 325.950 362.891, 321 341.610 L 312 302.919 312 279.689 L 312 256.460 345.659 229.480 C 385.192 197.791, 388.848 194.607, 390.644 190.310 C 395.997 177.498, 386.403 162.704, 372.752 162.721 C 368.130 162.727, 363.638 164.482, 335.283 177.364 L 303.066 192 256.065 192 L 209.065 192 177.282 177.630 C 159.802 169.726, 143.925 163.024, 142 162.735 C 140.075 162.446, 137.031 162.557, 135.236 162.982 M 26.174 250.314 C 23.195 253.486, 23.261 258.957, 26.314 261.826 C 28.414 263.798, 29.698 264, 40.140 264 C 50.880 264, 51.797 263.845, 53.826 261.686 C 56.805 258.514, 56.739 253.043, 53.686 250.174 C 51.586 248.202, 50.302 248, 39.860 248 C 29.120 248, 28.203 248.155, 26.174 250.314 M 458.174 250.314 C 455.195 253.486, 455.261 258.957, 458.314 261.826 C 460.414 263.798, 461.698 264, 472.140 264 C 482.880 264, 483.797 263.845, 485.826 261.686 C 488.805 258.514, 488.739 253.043, 485.686 250.174 C 483.586 248.202, 482.302 248, 471.860 248 C 461.120 248, 460.203 248.155, 458.174 250.314 M 70.010 389.750 C 69.387 392.913, 67.991 399.031, 66.907 403.346 C 61.759 423.842, 53.047 431.364, 28.410 436.582 C 16.794 439.042, 15.209 439.542, 16.123 440.456 C 16.374 440.707, 21.903 442.040, 28.410 443.418 C 55.081 449.067, 62.933 456.919, 68.582 483.590 C 71.042 495.206, 71.542 496.791, 72.456 495.877 C 72.707 495.626, 74.040 490.097, 75.418 483.590 C 81.420 455.253, 88.976 448.541, 122.250 441.990 C 125.412 441.368, 128 440.472, 128 440 C 128 439.528, 125.412 438.632, 122.250 438.010 C 87.210 431.111, 80.862 424.764, 73.987 389.750 C 73.366 386.587, 72.472 384, 72 384 C 71.528 384, 70.632 386.587, 70.010 389.750 M 438.010 389.750 C 437.387 392.913, 435.991 399.031, 434.907 403.346 C 429.759 423.842, 421.047 431.364, 396.410 436.582 C 389.903 437.960, 384.374 439.293, 384.123 439.544 C 383.209 440.458, 384.794 440.958, 396.410 443.418 C 423.081 449.067, 430.933 456.919, 436.582 483.590 C 437.960 490.097, 439.293 495.626, 439.544 495.877 C 440.458 496.791, 440.958 495.206, 443.418 483.590 C 449.420 455.253, 456.976 448.541, 490.250 441.990 C 493.413 441.368, 496 440.472, 496 440 C 496 439.528, 493.413 438.632, 490.250 438.010 C 455.210 431.111, 448.862 424.764, 441.987 389.750 C 441.366 386.587, 440.472 384, 440 384 C 439.528 384, 438.632 386.587, 438.010 389.750 M 250.174 458.314 C 248.202 460.414, 248 461.698, 248 472.140 C 248 482.880, 248.155 483.797, 250.314 485.826 C 253.486 488.805, 258.957 488.739, 261.826 485.686 C 263.798 483.586, 264 482.302, 264 471.860 C 264 461.120, 263.845 460.203, 261.686 458.174 C 258.514 455.195, 253.043 455.261, 250.174 458.314"),
                    Name = "File_Sort_Exists"
                },
                new SortingType
                {
                    Id = (int)SortType.ALL,
                    Icon = Geometry.Parse("M 73.265 38.542 C 64.808 40.819, 57.972 44.843, 51.407 51.407 C 44.691 58.124, 40.797 64.843, 38.492 73.693 C 36.244 82.321, 36.244 365.679, 38.492 374.307 C 40.797 383.157, 44.691 389.876, 51.407 396.593 C 58.124 403.309, 64.843 407.203, 73.693 409.508 C 82.321 411.756, 365.679 411.756, 374.307 409.508 C 383.157 407.203, 389.876 403.309, 396.593 396.593 C 403.309 389.876, 407.203 383.157, 409.508 374.307 C 411.756 365.679, 411.756 82.321, 409.508 73.693 C 407.203 64.843, 403.309 58.124, 396.593 51.407 C 389.876 44.691, 383.157 40.797, 374.307 38.492 C 365.958 36.317, 81.350 36.364, 73.265 38.542 M 77.241 71.995 C 75.448 73.088, 73.088 75.448, 71.995 77.241 C 70.036 80.453, 70.007 82.574, 70.004 224.102 L 70 367.704 72.270 371.102 C 73.518 372.971, 76.106 375.289, 78.020 376.254 C 81.296 377.905, 89.913 377.993, 225.177 377.754 C 368.724 377.500, 368.856 377.498, 371.677 375.429 C 373.230 374.290, 375.287 371.815, 376.248 369.929 C 377.889 366.712, 377.997 357.692, 377.997 224 C 377.997 90.328, 377.889 81.284, 376.248 78.020 C 375.287 76.106, 372.971 73.518, 371.102 72.270 L 367.704 70 224.102 70.004 C 82.574 70.007, 80.453 70.036, 77.241 71.995 M 464.914 122.956 C 460.450 124.308, 456.955 127.168, 454.942 131.113 C 453.044 134.835, 453 138.450, 453 292.305 C 453 443.507, 452.928 449.755, 451.171 451.345 C 449.486 452.870, 437.015 453, 292.131 453 C 137.721 453, 134.852 453.035, 131.061 454.969 C 123.992 458.575, 120.519 467.519, 123.457 474.552 C 125.611 479.706, 127.698 481.914, 132.500 484.119 C 136.327 485.877, 143.285 485.957, 293.201 485.978 C 405.492 485.994, 451.403 485.681, 455.201 484.876 C 470.334 481.665, 481.665 470.334, 484.876 455.201 C 485.681 451.403, 485.994 405.492, 485.978 293.201 C 485.957 142.991, 485.880 136.334, 484.110 132.500 C 480.376 124.409, 472.704 120.595, 464.914 122.956 M 304.190 152.134 C 300.771 153.120, 294.385 159.455, 230.995 224.750 C 215.377 240.838, 202.349 254, 202.044 254 C 201.740 254, 194.067 247.179, 184.995 238.843 C 175.923 230.507, 163.712 219.289, 157.859 213.916 C 146.320 203.321, 142.138 201.189, 135.379 202.457 C 130.255 203.418, 123.541 210.133, 122.602 215.235 C 122.224 217.290, 122.384 220.713, 122.958 222.843 C 124.183 227.392, 126.580 229.796, 166.366 266.360 C 194.059 291.811, 197.049 294.035, 203.500 293.968 C 208.746 293.914, 213.875 289.555, 238.168 264.500 C 252.033 250.200, 276.984 224.498, 293.614 207.384 C 319.601 180.641, 324.014 175.663, 325.009 171.967 C 327.244 163.667, 322.063 154.313, 313.942 151.983 C 309.926 150.832, 308.641 150.851, 304.190 152.134"),
                    Name = "File_Sort_All"
                },
            };
        }

        private void UpdateCurrentLocalFilesData()
        {
            if (_usnFileDatas.Count() != 0)
            {
                _notificationService.PutToPool(new Notification()
                {
                    Message = App.GetLocalizedString("Local_Files_Searching"),
                    MessageType = Notification.Type.INFO
                });

                new Task(() =>
                {
                    var temp = new ObservableCollection<UsnFileData>(_usnFileDatas);

                    if (SelectedSortingType.Id == (int)SortType.EXISTS)
                    {
                        temp = new ObservableCollection<UsnFileData>(temp.Where(p => !p.IsDeleted));
                    }
                    else if (SelectedSortingType.Id == (int)SortType.DELETED)
                    {
                        temp = new ObservableCollection<UsnFileData>(temp.Where(p => p.IsDeleted));
                    }

                    if (!string.IsNullOrEmpty(_searchPattern))
                    {
                        temp = new ObservableCollection<UsnFileData>(temp.Where(p =>
                            Util.ContainsFilteredWords(p.Path, _searchPattern)));
                    }

                    UsnFileDatas = temp;
                }).Start();
            }
        }

        private void RunLocalFilesGettingTask()
        {
            _notificationService.PutToPool(new Notification()
            {
                Message = App.GetLocalizedString("Local_Files_Updating"),
                MessageType = Notification.Type.INFO
            });

            App.QueueService.PutToQueue(new CancellableTask(Tasks.LOCAL_FILES_READ_DATA_TASK, token =>
            {
                List<UsnFileData> fileDatas = new List<UsnFileData>();
                
                NtfsUsnJournal _usnJournal = null;
                Win32Api.USN_JOURNAL_DATA _usnCurrentJournalState = new Win32Api.USN_JOURNAL_DATA();
                Win32Api.USN_JOURNAL_DATA newUsnState;
                List<Win32Api.UsnEntry> usnEntries;

                uint reasonMask = Win32Api.USN_REASON_DATA_OVERWRITE |
    Win32Api.USN_REASON_DATA_EXTEND |
    Win32Api.USN_REASON_NAMED_DATA_OVERWRITE |
    Win32Api.USN_REASON_NAMED_DATA_TRUNCATION |
    Win32Api.USN_REASON_FILE_CREATE |
    Win32Api.USN_REASON_FILE_DELETE |
    Win32Api.USN_REASON_EA_CHANGE |
    Win32Api.USN_REASON_SECURITY_CHANGE |
    Win32Api.USN_REASON_RENAME_OLD_NAME |
    Win32Api.USN_REASON_RENAME_NEW_NAME |
    Win32Api.USN_REASON_INDEXABLE_CHANGE |
    Win32Api.USN_REASON_BASIC_INFO_CHANGE |
    Win32Api.USN_REASON_HARD_LINK_CHANGE |
    Win32Api.USN_REASON_COMPRESSION_CHANGE |
    Win32Api.USN_REASON_ENCRYPTION_CHANGE |
    Win32Api.USN_REASON_OBJECT_ID_CHANGE |
    Win32Api.USN_REASON_REPARSE_POINT_CHANGE |
    Win32Api.USN_REASON_STREAM_CHANGE |
    Win32Api.USN_REASON_CLOSE;

                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    try
                    {
                        if (!(drive.IsReady && 0 == string.Compare(drive.DriveFormat, "ntfs", true)))
                        {
                            continue;
                        }

                        _usnJournal = new NtfsUsnJournal(drive);

                        NtfsUsnJournal.UsnJournalReturnCode rtn = _usnJournal.CreateUsnJournal(1000 * 1024, 16 * 1024);

                        if (rtn != NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                        {
                            continue;
                        }

                        Win32Api.USN_JOURNAL_DATA journalState = new Win32Api.USN_JOURNAL_DATA();

                        NtfsUsnJournal.UsnJournalReturnCode returnCode = _usnJournal.GetUsnJournalState(ref journalState);

                        if (returnCode == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                        {
                            _usnCurrentJournalState = journalState;

                            List<Win32Api.UsnEntry> fileList;

                            List<Win32Api.UsnEntry> folders;

                            NtfsUsnJournal.UsnJournalReturnCode rtnFileCode = _usnJournal.GetFilesMatchingFilter("*", out fileList);

                            _usnCurrentJournalState = journalState;

                            NtfsUsnJournal.UsnJournalReturnCode rtnCode1 = _usnJournal.GetNtfsVolumeFolders(out folders);

                            _usnCurrentJournalState = journalState;

                            if (journalState.UsnJournalID != 0)
                            {
                                NtfsUsnJournal.UsnJournalReturnCode rtnCode = _usnJournal.GetUsnJournalEntries(_usnCurrentJournalState, reasonMask, out usnEntries, out newUsnState);

                                _usnCurrentJournalState = journalState;

                                List<Win32Api.UsnEntry> allEntries = new List<Win32Api.UsnEntry>();
                                allEntries.AddRange(fileList);
                                allEntries.AddRange(usnEntries);

                                foreach (Win32Api.UsnEntry entry in allEntries)
                                {
                                    if (token.IsCancellationRequested)
                                    {
                                        break;
                                    }

                                    string path;

                                    NtfsUsnJournal.UsnJournalReturnCode usnRtnCode = _usnJournal.GetPathFromFileReference(entry.ParentFileReferenceNumber, out path);

                                    string fullPath = drive.Name.Replace(":\\", ":") + Path.Combine(path, entry.Name);

                                    bool deleted = (entry.Reason & nWin32Api.USN_REASON_FILE_DELETE) != 0;

                                    fileDatas.Add(new UsnFileData()
                                    {
                                        Path = fullPath,
                                        IsDeleted = deleted
                                    });
                                }
                            }
                        }

                        rtn = _usnJournal.DeleteUsnJournal(_usnCurrentJournalState);
                    } catch
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            _notificationService.PutToPool(new Notification()
                            {
                                Message = App.GetLocalizedString("Local_Files_Updating_Error"),
                                MessageType = Notification.Type.ERROR
                            });
                        });

                        App.QueueService.CancelTask(Tasks.LOCAL_FILES_READ_DATA_TASK);

                        return;
                    }
                }

                App.Current.Dispatcher.Invoke(() =>
                {
                    _notificationService.PutToPool(new Notification()
                    {
                        Message = App.GetLocalizedString("Local_Files_Updating_Success"),
                        MessageType = Notification.Type.SUCCESS
                    });
                });

                _usnFileDatas = new ObservableCollection<UsnFileData>(fileDatas);

                UsnFileDatas = new ObservableCollection<UsnFileData>(fileDatas);

                App.QueueService.CancelTask(Tasks.LOCAL_FILES_READ_DATA_TASK);
            }));

            App.QueueService.RunTask(Tasks.LOCAL_FILES_READ_DATA_TASK);
        }
    }

    public enum SortType
    {
        DELETED = 0,
        EXISTS = 1,
        ALL = 2
    }
}
