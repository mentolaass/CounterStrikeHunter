using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit.UserAssist
{
    public class UserAssistReadResult : IRegistryReadResult<UserAssistEntry>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public ObservableCollection<UserAssistEntry> Result { get; set; }
    }
}
