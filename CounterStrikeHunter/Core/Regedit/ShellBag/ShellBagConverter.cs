using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CounterStrikeHunter.Core.Regedit.ShellBag
{
    public class ShellBagConverter : IConverter<List<byte[]>, ShellBagReadResult>
    {
        public ShellBagReadResult Convert(RegisrtyViewModel meta, List<byte[]> value)
        {
            ObservableCollection<ShellBagEntry> entries = new ObservableCollection<ShellBagEntry>();

            foreach (byte[] entry in value)
            {
                try
                {
                    entries.Add(new ShellBagEntry()
                    {
                        Path = ReadPath(entry),
                        LastModifiedTimeUTC = WinTimestampToDateTime(BitConverter.ToUInt32(entry, 0x08))
                    });
                } catch
                {

                }
            }

            return new ShellBagReadResult()
            {
                Name = meta.Name,
                Path = meta.Path,
                Description = meta.Description,
                Result = entries
            };
        }

        public string ReadPath(byte[] data)
        {
            return RemoveInvalidPathChars(Encoding.UTF8.GetString(data)).Trim();
        }

        private DateTime WinTimestampToDateTime(uint timestamp)
        {
            DateTime epoch = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddTicks(timestamp * TimeSpan.TicksPerSecond);
        }

        private string RemoveInvalidPathChars(string input)
        {
            StringBuilder result = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                if (char.IsLetter(c) || c == '.' || c == ':' || c == '/' || c == '\\')
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }
    }
}
