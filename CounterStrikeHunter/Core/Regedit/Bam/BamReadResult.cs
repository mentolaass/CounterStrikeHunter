using CounterStrikeHunter.Core.Regedit.AppCompatCache;
using System.Collections.ObjectModel;
namespace CounterStrikeHunter.Core.Regedit.Bam
{
    public class BamReadResult : IRegistryReadResult<BamEntry>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public ObservableCollection<BamEntry> Result { get; set; }
    }
}
