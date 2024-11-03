using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CounterStrikeHunter.Core
{
    public class Util
    {
        public static string GetTableUser()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "whoami.exe",
                    Arguments = "/user /fo table /nh",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                return process.StandardOutput.ReadLine().Split(' ')[1];
            }

            return "";
        }

        public static string ROT13(string input)
        {
            return !string.IsNullOrEmpty(input) ? new string(input.ToCharArray().Select(s => { return (char)((s >= 97 && s <= 122) ? ((s + 13 > 122) ? s - 13 : s + 13) : (s >= 65 && s <= 90 ? (s + 13 > 90 ? s - 13 : s + 13) : s)); }).ToArray()) : input;
        }

        public static DateTime GetDateTimeFromWhat(long what)
        {
            long seconds = (what / 1000000) - 11644473600;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(seconds).ToLocalTime();
        }

        public static string FindSteamLocation()
        {
            // attempt 1
            RegistryKey key = Registry.CurrentUser;

            using (RegistryKey valve = key.OpenSubKey("SOFTWARE\\Valve\\Steam"))
            {
                if (valve != null)
                {
                    object steamPath = valve.GetValue("SteamPath");

                    if (steamPath != null && steamPath is string strSteamPath)
                    {
                        return strSteamPath;
                    }
                }
            }

            // attempt 2
            Process[] steamProcesses = Process.GetProcessesByName("steam");

            if (steamProcesses.Length == 0)
                return "";

            return Path.GetDirectoryName(steamProcesses[0].MainModule.FileName);
        }

        public static bool ContainsFilteredWords(string input, string filter)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(filter))
                return false;

            input = input.ToLower();
            filter = filter.ToLower();

            string[] terms = filter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string term in terms)
            {
                if (input.IndexOf(term.Trim(), StringComparison.Ordinal) >= 0)
                    return true;
            }

            return false;
        }
    }
}
