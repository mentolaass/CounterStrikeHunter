using CounterStrikeHunter.Core.Regedit.ShellBag;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit.AppCompatCache
{
    public class AppCompatCacheReadResult : IRegistryReadResult<AppCompatCacheEntry>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public ObservableCollection<AppCompatCacheEntry> Result { get; set; }
    }
}
