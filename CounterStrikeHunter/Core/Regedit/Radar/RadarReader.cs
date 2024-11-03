using Microsoft.Win32;
using System.Collections.Generic;

namespace CounterStrikeHunter.Core.Regedit.Radar
{
    public class RadarReader : IRegistryReader<List<string>>
    {
        public List<string> ReadValue(string path, string name)
        {
            List<string> response = new List<string>();

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path))
                {
                    using (RegistryKey diagonsed = key.OpenSubKey("DiagnosedApplications"))
                    {
                        foreach (var item in diagonsed.GetSubKeyNames())
                        {
                            response.Add(item);
                        }
                    }

                    using (RegistryKey reflect = key.OpenSubKey("ReflectionApplications"))
                    {
                        foreach (var item in reflect.GetSubKeyNames())
                        {
                            response.Add(item);
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
