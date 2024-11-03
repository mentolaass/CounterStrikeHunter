using CounterStrikeHunter.Core;
using CounterStrikeHunter.Core.Service.Navigation;
using CounterStrikeHunter.Core.Service.Notification;
using CounterStrikeHunter.Core.Service.Queue.Work;
using CounterStrikeHunter.Core.Steam;
using CounterStrikeHunter.Core.Steam.VdfParser;
using CounterStrikeHunter.Model.Steam;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace CounterStrikeHunter.ViewModel.Page
{
    public class AnalyzeSteamViewModel : ViewModelBase
    {
        private IFrameNavigationService _navigationService;

        private INotificationService _notificationService;

        private ObservableCollection<SteamAccount> _steamAccounts;

        public AnalyzeSteamViewModel(IFrameNavigationService navigationService, INotificationService notificationService)
        {
            _navigationService = navigationService;

            _notificationService = notificationService;

            App.QueueService.PutToQueue(new CancellableTask(Tasks.STEAM_ACCOUNT_PARSE_TASK_META, (token) =>
            {
                ObservableCollection<SteamAccount> temporaryCollection = new ObservableCollection<SteamAccount>();

                Notification notificationState = null;

                try
                {
                    temporaryCollection = new ObservableCollection<SteamAccount>(SteamAccountParser.Parse(new VdfParser(Util.FindSteamLocation()).Parse()));

                    notificationState = new Notification()
                    {
                        Message = App.GetLocalizedString("Steam_Data_Updating_Success"),
                        MessageType = Notification.Type.SUCCESS
                    };
                } catch
                {
                    notificationState = new Notification()
                    {
                        Message = App.GetLocalizedString("Steam_Data_Updating_Failed"),
                        MessageType = Notification.Type.ERROR
                    };
                }

                App.Current.Dispatcher.Invoke(() =>
                {
                    if (notificationState != null)
                    {
                        notificationService.PutToPool(notificationState);
                    }
                });

                SteamAccounts = temporaryCollection;

                App.QueueService.CancelTask(Tasks.STEAM_ACCOUNT_PARSE_TASK_META);
            }));

            notificationService.PutToPool(new Notification()
            {
                Message = App.GetLocalizedString("Steam_Data_Updating"),
                MessageType = Notification.Type.INFO
            });

            App.QueueService.RunTask(Tasks.STEAM_ACCOUNT_PARSE_TASK_META);
        }

        public ObservableCollection<SteamAccount> SteamAccounts
        {
            get => _steamAccounts;
            set
            {
                _steamAccounts = value;

                RaisePropertyChanged(nameof(SteamAccounts));
            }
        }
    }
}
