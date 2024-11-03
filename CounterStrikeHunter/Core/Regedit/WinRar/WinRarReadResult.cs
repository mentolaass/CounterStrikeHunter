using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit.WinRar
{
    public class WinRarReadResult : IRegistryReadResult<WinRarEntry>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public ObservableCollection<WinRarEntry> Result { get; set; }
    }
}
