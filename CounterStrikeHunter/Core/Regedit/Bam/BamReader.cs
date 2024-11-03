using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

namespace CounterStrikeHunter.Core.Regedit.Bam
{
    public class BamReader : IRegistryReader<List<string>>
    {
        public List<string> ReadValue(string path, string name)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path))
                {
                    return key.GetValueNames().ToList();
                }
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
