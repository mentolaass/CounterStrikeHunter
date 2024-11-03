using Microsoft.Win32;
using System.Collections.Generic;

namespace CounterStrikeHunter.Core.Regedit.ShellBag
{
    public class ShellBagReader : IRegistryReader<List<byte[]>>
    {
        public List<byte[]> ReadValue(string path, string name)
        {
            List<byte[]> bytes = new List<byte[]>();

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path))
                {
                    if (key == null) return bytes;

                    foreach (string subKeyName in key.GetSubKeyNames())
                    {
                        string subKeyPath = path + "\\" + subKeyName;
                        bytes.AddRange(Read(subKeyPath));
                    }
                }
            } catch
            {

            }

            return bytes;
        }

        private List<byte[]> Read(string path)
        {
            List<byte[]> bytes = new List<byte[]>();

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path))
            {
                if (key == null) return bytes;

                foreach (string valueName in key.GetValueNames())
                {
                    if (int.TryParse(valueName, out _))
                    {
                        object value = key.GetValue(valueName);
                        if (value is byte[] byteArray)
                        {
                            bytes.Add(byteArray);
                        }
                    }
                }

                foreach (string subKeyName in key.GetSubKeyNames())
                {
                    string subKeyPath = path + "\\" + subKeyName;
                    bytes.AddRange(Read(subKeyPath));
                }
            }

            return bytes;
        }
    }
}
