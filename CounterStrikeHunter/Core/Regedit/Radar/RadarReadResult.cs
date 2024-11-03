using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit.Radar
{
    public class RadarReadResult : IRegistryReadResult<RadarEntry>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public ObservableCollection<RadarEntry> Result { get; set; }
    }
}
