using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit._7Zip
{
    public class _7ZipReadResult : IRegistryReadResult<_7ZipEntry>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public ObservableCollection<_7ZipEntry> Result { get; set; }
    }
}
