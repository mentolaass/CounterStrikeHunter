using CounterStrikeHunter.Core.Regedit.Radar;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit.WinRar
{
    public class RadarConverter : IConverter<List<string>, RadarReadResult>
    {
        public RadarReadResult Convert(RegisrtyViewModel meta, List<string> value)
        {
            List<RadarEntry> entries = new List<RadarEntry>();

            foreach (var item in value)
            {
                entries.Add(new RadarEntry()
                {
                    Path = item
                });
            }

            return new RadarReadResult()
            {
                Description = meta.Description,
                Path = meta.Path,
                Name = meta.Name,
                Result = new ObservableCollection<RadarEntry>(entries)
            };
        }
    }
}
