using Microsoft.Win32;
using System.Collections.Generic;

namespace CounterStrikeHunter.Core.Regedit.UserAssist
{
    public class UserAssistReader : IRegistryReader<List<string>>
    {
        public List<string> ReadValue(string path, string name)
        {
            List<string> response = new List<string>();

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path))
                {
                    foreach (var sub in key.GetSubKeyNames())
                    {
                        if (sub.Contains("CEBFF5CD"))
                        {
                            RegistryKey executablesHistory = key.OpenSubKey(sub).OpenSubKey("Count");

                            foreach (var valueName in executablesHistory.GetValueNames())
                            {
                                response.Add(Util.ROT13(valueName));
                            }
                        }
                    }
                }
            } catch
            {

            }

            return response;
        }
    }
}
