using Microsoft.Win32;

namespace CounterStrikeHunter.Core.Regedit.AppCompatCache
{
    public class AppCompatCacheReader : IRegistryReader<byte[]>
    {
        public byte[] ReadValue(string path, string name)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path))
            {
                try
                {
                    return (byte[])key.GetValue("AppCompatCache", new byte[] { });
                } catch
                {
                    return new byte[] { };
                }
            }
        }
    }
}
