using CounterStrikeHunter.Core.Steam.VdfParser;
using CounterStrikeHunter.Model.Steam;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CounterStrikeHunter.Core.Steam
{
    public class SteamAccountParser
    {
        public static readonly string STEAM_PROFILE_URL = "https://steamcommunity.com/profiles/";

        public static IEnumerable<SteamAccount> Parse(VdfUserDictionary vdfUserDictionary)
        {
            using (HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
            {
                foreach (var user in vdfUserDictionary.Source)
                {
                    string response = client.GetStringAsync(STEAM_PROFILE_URL + user.Key)
                        .GetAwaiter()
                        .GetResult();

                    bool isVacBanned = (!Regex.Match(response, "<div class=\"profile_ban\">([^\">]+)").Success) ? false : true;

                    yield return new SteamAccount()
                    {
                        Id = user.Key,
                        Loginned = user.Value.MostRecent == 1 ? true : false,
                        AccountName = user.Value.AccountName,
                        LoginTimestamp = user.Value.Timestamp,
                        IsVacBanned = isVacBanned,
                    };
                }
            }
        }
    }
}
