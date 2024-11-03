using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit.WinRar
{
    public class WinRarConverter : IConverter<List<string>, WinRarReadResult>
    {
        public WinRarReadResult Convert(RegisrtyViewModel meta, List<string> value)
        {
            List<WinRarEntry> entries = new List<WinRarEntry>();

            foreach (var item in value)
            {
                entries.Add(new WinRarEntry()
                {
                    Path = item
                });
            }

            return new WinRarReadResult()
            {
                Description = meta.Description,
                Path = meta.Path,
                Name = meta.Name,
                Result = new ObservableCollection<WinRarEntry>(entries)
            };
        }
    }
}
