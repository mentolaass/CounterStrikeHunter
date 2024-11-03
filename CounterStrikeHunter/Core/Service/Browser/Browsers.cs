using System;
using System.IO;

namespace CounterStrikeHunter.Core.Service.Browser
{
    public static class Browsers
    {
        private static readonly string UserDataPath = $"C:\\Users\\{Environment.UserName}\\AppData";

        public static readonly BrowserMeta CHROME = new BrowserMeta("Chrome", Path.Combine(UserDataPath, "Local\\Google\\Chrome\\User Data\\Default"));
        public static readonly BrowserMeta YANDEX = new BrowserMeta("Yandex", Path.Combine(UserDataPath, "Local\\Yandex\\YandexBrowser\\User Data\\Default"));
        public static readonly BrowserMeta EDGE = new BrowserMeta("Edge", Path.Combine(UserDataPath, "Local\\Microsoft\\Edge\\User Data\\Default"));
        public static readonly BrowserMeta OPERA = new BrowserMeta("Opera", Path.Combine(UserDataPath, "Roaming\\Opera Software\\Opera Stable\\Default"));
        public static readonly BrowserMeta BRAVE = new BrowserMeta("Brave", Path.Combine(UserDataPath, "Local\\BraveSoftware\\Brave-Browser\\User Data\\Default"));
    }
}
