using CounterStrikeHunter.Core.Service.Queue;
using CounterStrikeHunter.View;
using GalaSoft.MvvmLight.Threading;
using System.Collections.Generic;
using System;
using System.Windows;
using CounterStrikeHunter.Model.Language;
using System.Globalization;
using System.Linq;
using System.Windows.Media.Imaging;

namespace CounterStrikeHunter
{
    public partial class App : Application
    {
        private static QueueService _queueService;
        public static QueueService QueueService
        {
            get => _queueService;
        }

        private static readonly List<Language> _languages = new List<Language>();
        public static List<Language> Languages { get { return _languages; } }

        public static event EventHandler LanguageChanged;

        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                System.Threading.Thread.CurrentThread.CurrentCulture = value;
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                ResourceDictionary dictionary = new ResourceDictionary()
                {
                    Source = new Uri(String.Format("Resources/Localization/{0}.xaml", value.Name), UriKind.Relative)
                };

                ResourceDictionary oldDictionary = Application.Current.Resources.MergedDictionaries.Where((dict) => dict.Source != null && dict.Source.OriginalString.StartsWith("Resources/Localization/")).First();

                if (oldDictionary != null)
                {
                    int indexDictionary = Application.Current.Resources.MergedDictionaries.IndexOf(oldDictionary);
                    Application.Current.Resources.MergedDictionaries.RemoveAt(indexDictionary);
                }

                Application.Current.Resources.MergedDictionaries.Add(dictionary);

                LanguageChanged(Application.Current, new EventArgs());
            }
        }

        public static string GetLocalizedString(string name)
        {
            try
            {
                return Application.Current.Resources[name].ToString();
            } catch
            {
                return "undefined";
            }
        }

        static App()
        {
            DispatcherHelper.Initialize();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            App.LanguageChanged += App_LanguageChanged;

            if (_languages.Count == 0)
            {
                _languages.Clear();
            }

            _languages.Add(new Language()
            {
                Culture = new CultureInfo("en-US"),
                Image = new BitmapImage(new Uri("/Resources/Icon/Localization/en-US.png", UriKind.Relative)),
                Name = "English"
            });

            _languages.Add(new Language()
            {
                Culture = new CultureInfo("ru-RU"),
                Image = new BitmapImage(new Uri("/Resources/Icon/Localization/ru-RU.png", UriKind.Relative)),
                Name = "Русский"
            });

            Language = CounterStrikeHunter.Properties.Settings.Default.DefaultLanguage;

            _queueService = new QueueService(10);

            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();
        }

        private void App_LanguageChanged(object sender, EventArgs e)
        {
            CounterStrikeHunter.Properties.Settings.Default.DefaultLanguage = Language;
            CounterStrikeHunter.Properties.Settings.Default.Save();
        }
    }
}
