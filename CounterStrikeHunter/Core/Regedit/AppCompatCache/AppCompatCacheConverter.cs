using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CounterStrikeHunter.Core.Regedit.AppCompatCache
{
    public class AppCompatCacheConverter : IConverter<byte[], AppCompatCacheReadResult>
    {
        // https://winreg-kb.readthedocs.io/en/latest/sources/system-keys/Application-compatibility-cache.html
        public AppCompatCacheReadResult Convert(RegisrtyViewModel meta, byte[] value)
        {
            List<AppCompatCacheEntry> entries = new List<AppCompatCacheEntry>();

            int offset = BitConverter.ToInt32(value, 0);

            int index = offset;

            while (index < value.Length)
            {
                try
                {
                    AppCompatCacheEntry entry = new AppCompatCacheEntry();

                    index += 12;

                    entry.PathSize = BitConverter.ToUInt16(value, index);

                    index += 2;

                    entry.Path = Encoding.Unicode.GetString(value, index, entry.PathSize).Replace(@"\??\", "");

                    index += entry.PathSize;

                    entry.LastModifiedTimeUTC = DateTimeOffset.FromFileTime(BitConverter.ToInt64(value, index)).ToUniversalTime();

                    index += 8;

                    var dataSize = BitConverter.ToInt32(value, index);

                    index += 4 + dataSize;

                    entries.Add(entry);
                } catch
                {

                }
            }

            return new AppCompatCacheReadResult()
            {
                Name = meta.Name,
                Path = meta.Path,
                Description = meta.Description,
                Result = new ObservableCollection<AppCompatCacheEntry>(entries)
            };
        }
    }
}
