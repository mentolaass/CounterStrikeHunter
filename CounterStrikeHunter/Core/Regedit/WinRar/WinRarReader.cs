using Microsoft.Win32;
using System.Collections.Generic;

namespace CounterStrikeHunter.Core.Regedit.WinRar
{
    public class WinRarReader : IRegistryReader<List<string>>
    {
        public List<string> ReadValue(string path, string name)
        {
            List<string> response = new List<string>();

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path))
                {
                    foreach (var item in key.GetSubKeyNames())
                    {
                        if (item == "ArcHistory")
                        {
                            foreach (string subKey in key.OpenSubKey(item).GetSubKeyNames())
                            {
                                response.Add(subKey);
                            }
                        }
                        else if (item == "DialogEditHistory")
                        {
                            foreach (string subKey in key.OpenSubKey(item).GetSubKeyNames())
                            {
                                RegistryKey temp = key.OpenSubKey(path);

                                foreach (var item1 in temp.GetValueNames())
                                {
                                    response.Add(temp.GetValue(item1).ToString());
                                }
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
