using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit.ShellBag
{
    public class ShellBagReadResult : IRegistryReadResult<ShellBagEntry>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public ObservableCollection<ShellBagEntry> Result { get; set; }
    }
}
