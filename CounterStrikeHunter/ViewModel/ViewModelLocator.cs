using CommonServiceLocator;
using CounterStrikeHunter.Core.Service.Navigation;
using CounterStrikeHunter.Core.Service.Notification;
using CounterStrikeHunter.ViewModel.Page;
using GalaSoft.MvvmLight.Ioc;
using MvvmDialogs;
using System;

using IDialogService = MvvmDialogs.IDialogService;

namespace CounterStrikeHunter.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<QuickInspectionViewModel>();
            SimpleIoc.Default.Register<AnalyzeBrowsersViewModel>();
            SimpleIoc.Default.Register<AnalyzeFilesViewModel>();
            SimpleIoc.Default.Register<AnalyzeRegistryViewModel>();
            SimpleIoc.Default.Register<AnalyzeSteamViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();

            InitDialogService();

            InitNotificationService();

            InitNavigationService();
        }

        private static void InitDialogService()
        {
            var service = new DialogService();

            SimpleIoc.Default.Register<IDialogService>(() => service);
        }

        private static void InitNotificationService()
        {
            var service = new NotificationService();

            SimpleIoc.Default.Register<INotificationService>(() => service);
        }

        private static void InitNavigationService()
        {
            var service = new FrameNavigationService();

            service.Configure("QuickInspection", new Uri("../View/UI_Page/QuickInspectionPage.xaml", UriKind.Relative));
            service.Configure("AnalyzeFiles", new Uri("../View/UI_Page/AnalyzeFilesPage.xaml", UriKind.Relative));
            service.Configure("AnalyzeBrowsers", new Uri("../View/UI_Page/AnalyzeBrowsersPage.xaml", UriKind.Relative));
            service.Configure("AnalyzeSteam", new Uri("../View/UI_Page/AnalyzeSteamPage.xaml", UriKind.Relative));
            service.Configure("AnalyzeRegistry", new Uri("../View/UI_Page/AnalyzeRegistryPage.xaml", UriKind.Relative));

            SimpleIoc.Default.Register<IFrameNavigationService>(() => service);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public QuickInspectionViewModel QuickInspection
        {
            get
            {
                return ServiceLocator.Current.GetInstance<QuickInspectionViewModel>();
            }
        }

        public AnalyzeBrowsersViewModel AnalyzeBrowsers
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AnalyzeBrowsersViewModel>();
            }
        }

        public AnalyzeRegistryViewModel AnalyzeRegistry
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AnalyzeRegistryViewModel>();
            }
        }

        public AnalyzeFilesViewModel AnalyzeFiles
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AnalyzeFilesViewModel>();
            }
        }

        public AnalyzeSteamViewModel AnalyzeSteam
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AnalyzeSteamViewModel>();
            }
        }

        public static void Cleanup()
        {
            
        }
    }
}