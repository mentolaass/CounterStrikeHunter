using Microsoft.Win32;
using System.Collections.Generic;
using System.Text;

namespace CounterStrikeHunter.Core.Regedit._7Zip
{
    public class _7ZipReader : IRegistryReader<List<string>>
    {
        public List<string> ReadValue(string path, string name)
        {
            List<string> response = new List<string>();

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path))
                {
                    var arcList = (byte[])key.GetValue("ArcHistory");

                    if (arcList != null)
                    {
                        var arcs = Encoding.Unicode.GetString(arcList).Split('\0');

                        foreach (var arc in arcs)
                        {
                            if (arc.Trim().Length == 0)
                            {
                                continue;
                            }

                            response.Add(arc);
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
