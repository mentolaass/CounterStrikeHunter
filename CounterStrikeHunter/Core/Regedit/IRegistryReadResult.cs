using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit
{
    public interface IRegistryReadResult<T> where T : IRegistryEntry
    {
        string Name { get; set; }
        string Path { get; set; }
        string Description { get; set; }
        ObservableCollection<T> Result { get; set; }
    }
}
