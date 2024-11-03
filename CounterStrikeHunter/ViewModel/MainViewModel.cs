using GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using CounterStrikeHunter.ViewModel.Navigation;
using System.Windows.Shapes;
using CounterStrikeHunter.View;
using CounterStrikeHunter.Model;
using CounterStrikeHunter.Core.Service.Navigation;
using System.Collections.ObjectModel;
using CounterStrikeHunter.Core.Service.Queue.Work;
using System.Linq;
using MvvmDialogs;
using CounterStrikeHunter.ViewModel.Dialog;
using CounterStrikeHunter.Core.Service.Notification;
using System;
using CounterStrikeHunter.Model.Dimension;
using CommonServiceLocator;
using System.Threading.Tasks;

namespace CounterStrikeHunter.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public event EventHandler ResizeWindowEvent;
        
        private readonly IFrameNavigationService _navigationService;

        private readonly IDialogService _dialogService;

        private readonly INotificationService _notificationService;

        private bool _isResizing, _isDialogOpened, _isMaximized, _isMenuBarExpanded, _isExtendingWindowDimensions;

        private int _menuBarIndex;

        private MainWindow _window;

        public MainViewModel(IFrameNavigationService navigationService, IDialogService dialogService, INotificationService notificationService)
        {
            foreach (var window in Application.Current.Windows)
            {
                if (!(window is MainWindow mainWindow))
                {
                    continue;
                }

                this._window = mainWindow;
            }

            _navigationService = navigationService;

            _dialogService = dialogService;

            _notificationService = notificationService;

            _notificationService.UpdateEvent += _notificationService_UpdateEvent;

            ResizeWindowEvent += MainViewModel_ResizeWindowEvent;

            App.QueueService.PutToQueue(new CancellableTask(Tasks.BACKGROUND_TASK_META, async (token) => {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    INotificationService service = ServiceLocator.Current.GetInstance<INotificationService>();

                    if (service != null && service.GetAllNotifications().Count != 0)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            service.Update();
                        });
                    }

                    App.QueueService.RaiseEvents();

                    await Task.Delay(500);
                }
            }));

            App.QueueService.RunTask(Tasks.BACKGROUND_TASK_META);
        }

        private void MainViewModel_ResizeWindowEvent(object sender, EventArgs e)
        {
            if (sender is Dimension dimension)
            {
                if (dimension.Width >= 1200)
                {
                    MenuBarIsExpanded = true;
                    IsExtendingWindowDimensions = true;
                } else
                {
                    MenuBarIsExpanded = false;
                    IsExtendingWindowDimensions = false;
                }
            }
        }

        private void _notificationService_UpdateEvent(object sender, System.EventArgs e)
        {
            RaisePropertyChanged(nameof(AppNotifications));
        }

        public ObservableCollection<Notification> AppNotifications
        {
            get => _notificationService.GetAllNotifications();
            set { }
        }

        #region Queue 
        public ObservableCollection<CancellableTask> WorkingTasks
        {
            get => new ObservableCollection<CancellableTask>(App.QueueService.Tasks.Where(task => task.IsWorking));
            set { }
        }

        public ObservableCollection<CancellableTask> QueueTasks
        {
            get => new ObservableCollection<CancellableTask>(App.QueueService.Tasks);
            set { }
        }

        public bool IsExtendingWindowDimensions
        {
            get => _isExtendingWindowDimensions;
            set
            {
                _isExtendingWindowDimensions = value;

                RaisePropertyChanged(nameof(IsExtendingWindowDimensions));
            }
        }

        public bool IsDialogOpened
        {
            get => _isDialogOpened;
            set
            {
                _isDialogOpened = value;

                RaisePropertyChanged(nameof(IsDialogOpened));
            }
        }

        public bool TasksWorking
        {
            get => App.QueueService.CountTasks > 0;
            set { }
        }

        public int WorkingTasksCount
        {
            get => App.QueueService.CountWorkingTasks;
            set { }
        }

        public ICommand OpenSettingsMenu
        {
            get => new RelayCommand((obj) =>
            {
                var settingsDialogViewModel = new SettingsDialogViewModel();

                IsDialogOpened = true;

                try
                {
                    bool status = _dialogService.ShowDialog(this, settingsDialogViewModel) ?? true;

                    if (status)
                    {
                        IsDialogOpened = false;
                    }
                }
                catch
                {
                    IsDialogOpened = false;
                }
            });
        }

        public ICommand OpenTaskMenu
        {
            get => new RelayCommand((obj) =>
            {
                var taskDialogViewModel = new TaskDialogViewModel();

                IsDialogOpened = true;

                try
                {
                    bool status = _dialogService.ShowDialog(this, taskDialogViewModel) ?? true;

                    if (status)
                    {
                        IsDialogOpened = false;
                    }
                } catch
                {
                    IsDialogOpened = false;
                }
            });
        }
        #endregion Queue

        #region Loaded

        public ICommand Loaded
        {
            get => new RelayCommand((obj) =>
            {
                MenuBarIndex = 0;

                App.QueueService.CreateTaskEvent += (e, a) =>
                {
                    RaisePropertyChanged(nameof(TasksWorking));
                    RaisePropertyChanged(nameof(WorkingTasks));
                    RaisePropertyChanged(nameof(QueueTasks));
                    RaisePropertyChanged(nameof(WorkingTasksCount));
                };

                App.QueueService.CancelTaskEvent += (e, a) =>
                {
                    RaisePropertyChanged(nameof(TasksWorking));
                    RaisePropertyChanged(nameof(WorkingTasks));
                    RaisePropertyChanged(nameof(QueueTasks));
                    RaisePropertyChanged(nameof(WorkingTasksCount));
                };
            });
        }

        #endregion Loaded

        #region Menubar

        public enum Page
        {
            QUICK_INSPECTION = 0,
            ANALYZE_FILES = 1,
            ANALYZE_REGISTRY = 2,
            ANALYZE_BROWSERS = 3,
            ANALYZE_STEAM = 4
        }

        public bool MenuBarIsExpanded
        {
            get => _isMenuBarExpanded;
            set
            {
                _isMenuBarExpanded = value;

                RaisePropertyChanged(nameof(MenuBarIsExpanded));
            }
        }

        public int MenuBarIndex
        {
            get => _menuBarIndex;
            set
            {
                _menuBarIndex = value;

                switch (value)
                {
                    case (int)Page.QUICK_INSPECTION:
                        _navigationService.NavigateTo("QuickInspection");
                        break;
                    case (int)Page.ANALYZE_BROWSERS:
                        _navigationService.NavigateTo("AnalyzeBrowsers");
                        break;
                    case (int)Page.ANALYZE_FILES:
                        _navigationService.NavigateTo("AnalyzeFiles");
                        break;
                    case (int)Page.ANALYZE_REGISTRY:
                        _navigationService.NavigateTo("AnalyzeRegistry");
                        break;
                    case (int)Page.ANALYZE_STEAM:
                        _navigationService.NavigateTo("AnalyzeSteam");
                        break;
                }
                
                RaisePropertyChanged(nameof(MenuBarIndex));
            }
        }

        public ICommand MenuBarSelect
        {
            get => new RelayCommand((obj) =>
            {
                if (obj is SelectionChangedEventArgs args)
                {
                    if (args.AddedItems[0] is NavigationTabViewModel navigationTabViewModel)
                    {
                        MenuBarIndex = navigationTabViewModel.ModelTabIndex;
                    }
                }
            });
        }

        public ICommand MenuBarExpand
        {
            get => new RelayCommand((obj) =>
            {
                MenuBarIsExpanded = true;
            });
        }

        public ICommand MenuBarReset
        {
            get => new RelayCommand((obj) =>
            {
                if (this._window.Width <= 1200)
                {
                    MenuBarIsExpanded = false;
                }
            });
        }

        #endregion Menubar

        #region Window Style Commands

        public ICommand MaximizeWithHeader
        {
            get => new RelayCommand((obj) =>
            {
                if (obj is MouseButtonEventArgs args
                    && args.Source is Border border
                    && border.Name.ToLower() == "UI_Header".ToLower())
                {
                    if (args.LeftButton == MouseButtonState.Pressed
                        && !this._isMaximized)
                    {
                        this._window.DragMove();
                    }

                    if (args.ClickCount == 2)
                    {
                        this.MaximizeWindow();
                    }
                }
            });
        }

        public ICommand Maximize
        {
            get => new RelayCommand((obj) =>
            {
                this.MaximizeWindow();
            });
        }

        private void MaximizeWindow()
        {
            if (_isMaximized)
            {
                this._window.Width = this._window.MinWidth;
                this._window.Height = this._window.MinHeight;
                this._window.Left = SystemParameters.WorkArea.Width / 2 - this._window.Width / 2;
                this._window.Top = SystemParameters.WorkArea.Height / 2 - this._window.Height / 2;
                this._window.WindowState = WindowState.Normal;

                this._isMaximized = false;
            }
            else
            {
                this._window.Left = 0;
                this._window.Top = 0;
                this._window.Width = SystemParameters.WorkArea.Width;
                this._window.Height = SystemParameters.WorkArea.Height;
                this._window.WindowState = WindowState.Normal;

                this._isMaximized = true;
            }

            ResizeWindowEvent.Invoke(new Dimension()
            {
                Width = this._window.Width,
                Height = this._window.Height
            }, new EventArgs());
        }

        public ICommand Minimize
        {
            get => new RelayCommand((obj) =>
            {
                this._window.WindowState = WindowState.Minimized;
            });
        }

        public ICommand Close
        {
            get => new RelayCommand((obj) =>
            {
                this._window.Close();

                if (this._window == App.Current.MainWindow)
                {
                    Application.Current.Shutdown();
                }
            });
        }

        #endregion Window Style Commands

        #region Window Resize Commands

        public ICommand AttemptSidedResize
        {
            get => new RelayCommand((obj) =>
            {
                if (obj is MouseEventArgs args
                    && args.Source is Rectangle rectangle
                    && _isResizing)
                {
                    var position = args.GetPosition(this._window);

                    bool isLeft = rectangle.HorizontalAlignment == HorizontalAlignment.Left;
                    bool isRight = rectangle.HorizontalAlignment == HorizontalAlignment.Right;
                    bool isTop = rectangle.VerticalAlignment == VerticalAlignment.Top;
                    bool isBottom = rectangle.VerticalAlignment == VerticalAlignment.Bottom;

                    double newWidth = this._window.Width;
                    double newHeight = this._window.Height;
                    double newLeft = this._window.Left;
                    double newTop = this._window.Top;

                    if (isRight && position.X > this._window.MinWidth)
                    {
                        newWidth = position.X + 5;
                    }
                    else if (isLeft && this._window.Width - position.X > this._window.MinWidth)
                    {
                        newWidth = this._window.Width - position.X;
                        newLeft = this._window.Left + position.X;
                    }

                    if (isBottom && position.Y > this._window.MinHeight)
                    {
                        newHeight = position.Y + 5;
                    }
                    else if (isTop && this._window.Height - position.Y > this._window.MinHeight)
                    {
                        newHeight = this._window.Height - position.Y;
                        newTop = this._window.Top + position.Y;
                    }

                    this._window.Width = newWidth;
                    this._window.Height = newHeight;
                    this._window.Left = newLeft;
                    this._window.Top = newTop;

                    ResizeWindowEvent.Invoke(new Dimension()
                    {
                        Width = this._window.Width,
                        Height = this._window.Height
                    }, new EventArgs());
                }
            });
        }

        public ICommand AttemptButtonResizeClick
        {
            get => new RelayCommand((obj) =>
            {
                if (obj is MouseButtonEventArgs args
                    && args.Source is Rectangle rectangle)
                {
                    if (args.LeftButton == MouseButtonState.Pressed)
                    {
                        _isResizing = true;
                        rectangle.CaptureMouse();
                    }
                    else if (args.LeftButton == MouseButtonState.Released)
                    {
                        _isResizing = false;
                        rectangle.ReleaseMouseCapture();
                    }
                }
            });
        }

        #endregion Window Resize Commands
    }
}