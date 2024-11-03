using CounterStrikeHunter.Core.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit.Bam
{
    public class BamConverter : IConverter<List<string>, BamReadResult>
    {
        public BamReadResult Convert(RegisrtyViewModel meta, List<string> value)
        {
            List<BamEntry> entries = new List<BamEntry>();

            foreach (var item in value)
            {
                entries.Add(new BamEntry()
                {
                    Path = DevicePathMapper.FromDevicePath(item)
                });
            }

            return new BamReadResult()
            {
                Description = meta.Description,
                Path = meta.Path,
                Name = meta.Name,
                Result = new ObservableCollection<BamEntry>(entries)
            };
        }
    }
}
