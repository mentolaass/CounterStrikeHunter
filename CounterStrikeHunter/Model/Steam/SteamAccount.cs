using CounterStrikeHunter.Core.Steam;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace CounterStrikeHunter.Model.Steam
{
    public class SteamAccount
    {
        public long Id { get; set; }

        public string AccountName { get; set; }

        public long LoginTimestamp { get; set; }

        public string LoginDate
        {
            get => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                        .AddSeconds(LoginTimestamp)
                        .ToLocalTime()
                        .ToString("yyyy-MM-dd HH:mm:ss");
        }

        public bool IsVacBanned { get; set; }

        public bool Loginned { get; set; }

        public ICommand GotoProfile
        {
            get => new RelayCommand((obj) =>
            {
                Process.Start(SteamAccountParser.STEAM_PROFILE_URL + Id);
            });
        }
    }
}
